using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

using CsvHelper.Configuration;
using CsvHelper;

using UploadCsv.Models;
using UploadCsv.Data;

using EFCore.BulkExtensions;

using System.Globalization;


namespace UploadCsv.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private UploadContext _context;   

    public FileController(UploadContext context)
    {
        _context = context;
    }

    /// <summary> Upload de CSV para inserção no SQLite </summary>
    /// <param name="file"> Arquivo CSV </param>
    /// <returns> IActionResult </returns>
    /// <response code="201"> Inserido com sucesso. </response>
    [HttpPost("upload/")]
    public IActionResult UploadFIle([BindRequired] IFormFile file)
    {
        List<Point>? points = new List<Point>();

        string[] allowedExtensions = new string[] { ".csv" };
        string ext = System.IO.Path.GetExtension(file.FileName);

        if (!allowedExtensions.Contains(ext)) return Unauthorized();

        string filename = CreateTempfilePath();
        
        SaveFile(filename, file);

        points = ReadCsv(filename);

        if (points == null) return BadRequest();

        BulkInsertCsv(points, _context);

        System.IO.File.Delete(filename);

        return Ok();
    }

    static void SaveFile(string filename, IFormFile file)
    {
        using (FileStream filestream = System.IO.File.Create(filename))
        {
            file.CopyTo(filestream);
            filestream.Flush();
        }
    }

    static List<Point>? ReadCsv(string filename)
    {
        var points = new List<Point>();

        using (var reader = new StreamReader(filename))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) 
                                        { Delimiter = ";", PrepareHeaderForMatch = (args) => args.Header.Trim() } ))
        {
            try
            {
                points = csv.GetRecords<Point>().ToList();
            }
            catch (System.Exception)
            {
                points = null;
            }
        }

        return points;
    }

    static async void BulkInsertCsv(List<Point> points, UploadContext _context)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            await _context.BulkInsertAsync(points);
            await transaction.CommitAsync();
        }
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