using System.ComponentModel.DataAnnotations;

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

        public bool IsActive { get; set; } = true;

        // Representa a lista de componentes que foram arrastados na UI
        public List<FormFieldModel> Fields { get; set; } = new List<FormFieldModel>();

        // Nome de utilizador do login atual que criou o formulário
        public string? CreatorUserName { get; set; }
    }
}

