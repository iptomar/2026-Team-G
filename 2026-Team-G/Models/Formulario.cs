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
    }

    public class FormFieldModel
    {
        // Guardamos o ID dinâmico gerado pelo frontend (ex: "field_j9f8ks") para manter a coerência
        public string FieldId { get; set; }

        [Required]
        public string Type { get; set; } // text, textarea, number, select, checkbox, radio, date, title, button

        [Required]
        public string Label { get; set; }

        public string Placeholder { get; set; }

        public bool IsRequired { get; set; }

        // Guardar as opções inseridas no frontend (separadas por vírgula)
        public string Options { get; set; }

        // Ordem do elemento baseada na posição do "Drag and Drop"
        public int OrderIndex { get; set; }
    }
}

