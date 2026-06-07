using _2026_Team_G.Models;
using _2026_Team_G.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        // Pagina inicial - acessivel a todos (sem [Authorize])
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Home";
            return View();
        }

        // Pagina de componentes - acessivel a todos
        public async Task<IActionResult> Componentes()
        {
            ViewBag.ActivePage = "Componentes";
            var componentes = await _context.Components.ToListAsync();
            ViewBag.ComponentesDisponiveis = componentes;
            return View();
        }

        public IActionResult Formulario()
        {
            return View();
        }

        // PROTEGIDO: So Admins podem gravar formularios
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SalvarFormulario([FromBody] Formulario formulario)
        {
            if (formulario == null)
            {
                return BadRequest(new { success = false, message = "Dados invalidos do formulario." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, message = "Erro de validacao: " + string.Join(", ", errors) });
            }

            try
            {
                _context.Formularios.Add(formulario);
                await _context.SaveChangesAsync();
                return Json(new { success = true, id = formulario.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao guardar o formulario na base de dados: " + ex.Message });
            }
        }

        // PROTEGIDO: So utilizadores autenticados podem ver o historico
        [Authorize]
        public async Task<IActionResult> HistoricoFormulariosUtilizador()
        {
            ViewBag.ActivePage = "Historico";
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