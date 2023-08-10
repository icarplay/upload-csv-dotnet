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

    [HttpGet]
    public List<ReadPointDto> GetPoints(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10
    )
    {
        return _mapper.Map<List<ReadPointDto>>(
            _context.Points.Skip(skip).Take(take));
    }

    [HttpGet("average/")]
    public IActionResult GetAverage()
    {
        var mean = _context.Points.DefaultIfEmpty().Average(c => (double?) c.Deslocamento) ?? 0;

        return Ok(mean);
    }

    [HttpGet("MaxMinRaw/")]
    public List<ReadPointDto> GetMaxMinPointsRaw()
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

    [HttpGet("MaxMin/")]
    public List<ReadPointDto>? GetMaxMinPoints()
    {
        var maxMin = new List<Point>();
        
        var max = _context.Points.OrderByDescending(p => p.Deslocamento).FirstOrDefault();
        if (max != null) maxMin.Add(max);

        var min = _context.Points.OrderBy(p => p.Deslocamento).FirstOrDefault();
        if (min != null ) maxMin.Add(min);

        return _mapper.Map<List<ReadPointDto>>(maxMin);
    }


    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _context.Points.ExecuteDelete();
        
        return NoContent();
    }

}