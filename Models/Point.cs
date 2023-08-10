using System.ComponentModel.DataAnnotations;

namespace UploadCsv.Models;

public class Point
{
    [Key]
    [Required(ErrorMessage = "Campo 'Id' Obrigatório")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Campo 'X' Obrigatório")]
    public double X { get; set; }
    
    [Required(ErrorMessage = "Campo 'Y' Obrigatório")]
    public double Y { get; set; }
    
    [Required(ErrorMessage = "Campo 'Z' Obrigatório")]
    public double Z { get; set; }

    [Required(ErrorMessage = "Campo 'Deslocamento' Obrigatório")]
    public double Deslocamento { get; set; }
}