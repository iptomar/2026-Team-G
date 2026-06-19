using System.ComponentModel.DataAnnotations;

namespace _2026_Team_G.Models
{
    public class FormFieldModel
    {
        public int Id { get; set; }
        public string FieldId { get; set; }

        [Required]
        public string Type { get; set; } //Classe do componente

        [Required]
        public string Label { get; set; }

        public string Placeholder { get; set; }

        public bool IsRequired { get; set; }

        // Guardar as opções inseridas no frontend 
        public string Options { get; set; }

        // Ordem do elemento baseada na posição do "Drag and Drop"
        public int OrderIndex { get; set; }

        // Largura do componente (ex: "100%", "75%", "50%", "33%", "25%")
        public string Width { get; set; } = "100%";
    }
}
