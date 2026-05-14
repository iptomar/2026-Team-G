using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _2026_Team_G.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace _2026_Team_G.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public DbSet<Component> Components { get; set; }

    public DbSet<Form> Forms { get; set; }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Utilizador> Utilizadores { get; set; }

    public DbSet<Formulario> Formularios { get; set; }

    public DbSet<FormFieldModel> FormFieldModels { get; set; }

}

