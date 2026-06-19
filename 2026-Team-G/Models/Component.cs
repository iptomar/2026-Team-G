using System.ComponentModel.DataAnnotations;

namespace _2026_Team_G.Models;

public class Component 
{
    public int Id { get; set; }
    public string name { get; set; }
    
    public string componentClass { get; set; }
}