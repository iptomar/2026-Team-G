using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace _2026_Team_G.Models
{
    public class Formulario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título do formulário é obrigatório")]
        [StringLength(150)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public string? CreatedByUserId { get; set; }
        public IdentityUser? CreatedByUser { get; set; }

        public bool IsActive { get; set; } = true;

        // Representa a lista de componentes que foram arrastados na UI
        public List<FormFieldModel> Fields { get; set; } = new List<FormFieldModel>();
    }
}

