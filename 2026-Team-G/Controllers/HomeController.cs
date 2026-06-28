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

        // Pagina inicial - acessivel a todos 
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Home";
            return View();
        }

        public IActionResult Formulario()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DebugFormularios()
        {
            var todos = await _context.Formularios.ToListAsync();
            return Json(todos);
        }
        

        // PROTEGIDO: So Admins podem gravar formularios
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

        // PROTEGIDO: So utilizadores autenticados podem ver o historico de submissoes
        [Authorize]
        public async Task<IActionResult> HistoricoFormulariosUtilizador()
        {
            ViewBag.ActivePage = "Historico";
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                IQueryable<Submissao> query = _context.Submissoes
                    .Include(s => s.Formulario)
                    .Include(s => s.Utilizador);

                // Utilizadores comuns apenas vêem as suas próprias submissões, admins vêem todas
                if (!User.IsInRole("Admin"))
                {
                    query = query.Where(s => s.UtilizadorId == userId);
                }

                var submissoes = await query
                    .OrderByDescending(s => s.DataSubmissao)
                    .ToListAsync();

                // Passar o total de formulários ativos para as estatísticas
                ViewBag.TotalFormulariosAtivos = await _context.Formularios.CountAsync(f => f.IsActive);

                return View(submissoes);
            }
            catch (Exception)
            {
                return View(new List<Submissao>());
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}