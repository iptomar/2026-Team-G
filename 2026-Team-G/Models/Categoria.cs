using System.ComponentModel.DataAnnotations;

namespace _2026_Team_G.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}
