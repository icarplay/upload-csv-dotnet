using Microsoft.AspNetCore.Mvc;
using UploadCsv.Models;
using UploadCsv.Data;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using EFCore.BulkExtensions;

namespace UploadCsv.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{

    private UploadContext _context;
    
    public FileController(UploadContext context) {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFIle(IFormFile file)
    {

        List<Point> points = new List<Point>();

        string[] allowedExtensions = new string[] { ".csv" };

        string ext = System.IO.Path.GetExtension(file.FileName);
        string filename = CreateTempfilePath();

        if (!allowedExtensions.Contains(ext)) return Unauthorized();
        
        using (FileStream filestream = System.IO.File.Create(filename))
        {            
            file.CopyTo(filestream);
            filestream.Flush();
        }

        using (var reader = new StreamReader(filename))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", PrepareHeaderForMatch = (args) => args.Header.Trim() } ))
        {
            points = csv.GetRecords<Point>().ToList();
        }

        using (var transaction = _context.Database.BeginTransaction())
        {
            await _context.BulkInsertAsync(points);
            await transaction.CommitAsync();
        }

        System.IO.File.Delete(filename);

        return Ok("OK!");
    }


    static string CreateTempfilePath()
    {
        var filename = $@"{DateTime.Now.Ticks}.csv";

        var directoryPath = Path.Combine("temp", "uploads");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return Path.Combine(directoryPath, filename);
    }

}