using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2026_Team_G.Models;

public class User
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
    public string Nome { get; set; }
    
    [StringLength(50)]
    public string? IdentityUserName { get; set; }
}