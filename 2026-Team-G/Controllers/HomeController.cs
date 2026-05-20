using _2026_Team_G.Models;
using _2026_Team_G.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _2026_Team_G.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Componentes()
        {
            return View();
        }

        public IActionResult Formulario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarFormulario([FromBody] Formulario formulario)
        {
            if (formulario == null)
            {
                return BadRequest(new { success = false, message = "Dados inválidos do formulário." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, message = "Erro de validação: " + string.Join(", ", errors) });
            }

            try
            {
                _context.Formularios.Add(formulario);
                await _context.SaveChangesAsync();
                return Json(new { success = true, id = formulario.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao guardar o formulário na base de dados: " + ex.Message });
            }
        }

        public async Task<IActionResult> HistoricoFormulariosUtilizador()
        {
            try
            {
                var formularios = await _context.Formularios
                    .Include(f => f.Fields)
                    .OrderByDescending(f => f.Id)
                    .ToListAsync();

                return View(formularios);
            }
            catch (Exception)
            {
                // Fallback to empty list if there's any database issues
                return View(new List<Formulario>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
