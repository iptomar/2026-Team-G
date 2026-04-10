using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _2026_Team_G.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace _2026_Team_G.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Component> Components { get; set; }
        
        public DbSet<Form> Forms { get; set; }
        
        public DbSet<Log> Logs { get; set; }
        
        public DbSet<User> Users { get; set; }
    }
    
    
}
