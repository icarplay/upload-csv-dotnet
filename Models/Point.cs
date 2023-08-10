using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace UploadCsv.Models;

public class Point
{
    [Key]
    [Required(ErrorMessage = "Campo 'Id' Obrigatório")]
    [Name("CellId")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Campo 'X' Obrigatório")]
    [Name("X")]
    public double X { get; set; }
    
    [Required(ErrorMessage = "Campo 'Y' Obrigatório")]
    [Name("Y")]
    public double Y { get; set; }
    
    [Required(ErrorMessage = "Campo 'Z' Obrigatório")]
    [Name("Z")]
    public double Z { get; set; }

    [Required(ErrorMessage = "Campo 'Deslocamento' Obrigatório")]
    [Name("Value")]
    public double Deslocamento { get; set; }
}