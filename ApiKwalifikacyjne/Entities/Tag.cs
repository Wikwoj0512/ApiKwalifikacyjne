using System.ComponentModel.DataAnnotations;
using ApiKwalifikacyjne.Models;

namespace ApiKwalifikacyjne.Entities;

public class Tag
{
    [Key]
    public string Name { get; set; }
    
    public int Count { get; set; }
    
    public double Share { get; set; }
    
    public bool HasSynonyms { get; set; }
    
    public bool IsModeratorOnly { get; set; }
    
    public bool IsRequired { get; set; }
}