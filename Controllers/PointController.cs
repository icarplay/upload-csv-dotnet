using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using UploadCsv.Data.Dto;
using UploadCsv.Models;
using UploadCsv.Data;

using AutoMapper;


namespace UploadCsv.Controllers;

[ApiController]
[Route("[controller]")]
public class PointController : ControllerBase
{
    private UploadContext _context;
    private IMapper _mapper;

    public PointController(UploadContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary> Retorna pontos com paginação. </summary>
    /// <param name="skip"> Index inicial </param>
    /// <param name="take"> Index Final </param>
    /// <returns> Lista de pontos. </returns>
    /// <response code="200"> Consulta realizada com sucesso. </response>
    [HttpGet]
    public List<ReadPointDto> GetPoints(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10
    )
    {
        return _mapper.Map<List<ReadPointDto>>(
            _context.Points.Skip(skip).Take(take));
    }

    /// <summary> Retorna a média dos deslocamentos de todos os pontos. </summary>
    /// <returns> double com a média </returns>
    /// <response code="200"> Consulta realizada com sucesso. </response>
    [HttpGet("average/")]
    public IActionResult GetAverage()
    {
        var mean = _context.Points.DefaultIfEmpty().Average(c => (double?) c.Deslocamento) ?? 0;

        return Ok(mean);
    }

    /// <summary> Retorna os pontos com maior e menor deslocamento. </summary>
    /// <returns> lista de pontos </returns>
    /// <response code="200"> Consulta realizada com sucesso. </response>
    [HttpGet("max-min/")]
    public List<ReadPointDto> GetMaxMinPoints()
    {
        var query = @"
            
            SELECT * FROM POINTS 
                            WHERE 
                                CellId = (
                                    SELECT CellId FROM POINTS ORDER BY Deslocamento DESC Limit 1
                                )
                            OR 
                                CellId = (
                                    SELECT CellId FROM POINTS ORDER BY Deslocamento ASC Limit 1
                                );
        ";

        var MaxAndMin = _mapper.Map<List<ReadPointDto>>(_context.Points.FromSqlRaw(query).ToList());

        return MaxAndMin;
    }

    /// <summary> Apaga todos os registros de pontos da base de dados. </summary>
    /// <response code="200"> Registros apagados com sucesso. </response>
    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _context.Points.ExecuteDelete();
        
        return Ok();
    }
}