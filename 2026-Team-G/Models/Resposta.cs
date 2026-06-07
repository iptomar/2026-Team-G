namespace _2026_Team_G.Models
{
    public class Resposta
    {
        public int Id { get; set; }

        public int SubmissaoId { get; set; }
        public Submissao Submissao { get; set; }

        // Ligação ao campo dinâmico que foi respondido
        public int FormFieldModelId { get; set; }
        public FormFieldModel FormFieldModel { get; set; }

        // O valor preenchido (ex: texto, número, opção selecionada)
        public string Valor { get; set; }
    }
}
