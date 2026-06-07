using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace _2026_Team_G.Models
{
    public class Submissao
    {
        public int Id { get; set; }

        public int FormularioId { get; set; }
        public Formulario Formulario { get; set; }

        // Vinculação ao utilizador do ASP.NET Identity
        public string UtilizadorId { get; set; }
        public IdentityUser Utilizador { get; set; }

        public DateTime DataSubmissao { get; set; } = DateTime.Now;

        public List<Resposta> Respostas { get; set; } = new List<Resposta>();
    }
}
