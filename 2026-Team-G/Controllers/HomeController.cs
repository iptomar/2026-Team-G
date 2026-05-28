using _2026_Team_G.Models;
using _2026_Team_G.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
            ViewBag.ActivePage = "Home";
            return View();
        }
        
        public async Task<IActionResult> Componentes()
        {
            ViewBag.ActivePage = "Componentes";
            // 1. Vai buscar a lista à base de dados usando o teu DbContext
            var componentes = await _context.Components.ToListAsync();

            // 2. Guarda na ViewBag com o nome EXATO que usamos na View
            ViewBag.ComponentesDisponiveis = componentes;

            return View();
        }

        public IActionResult Formulario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                // Associar o utilizador autenticado diretamente pelo username
                formulario.CreatorUserName = User.Identity?.Name;

                if (formulario.Id > 0)
                {
                    // Editar formulário existente
                    var existingForm = await _context.Formularios
                        .Include(f => f.Fields)
                        .FirstOrDefaultAsync(f => f.Id == formulario.Id);

                    if (existingForm == null)
                    {
                        return NotFound(new { success = false, message = "Formulário não encontrado." });
                    }

                    // Atualizar metadados
                    existingForm.Title = formulario.Title;
                    existingForm.Description = formulario.Description;
                    existingForm.CreatorUserName = formulario.CreatorUserName;

                    // Remover os campos antigos e associar os novos
                    _context.FormFieldModels.RemoveRange(existingForm.Fields);
                    existingForm.Fields = formulario.Fields;

                    _context.Formularios.Update(existingForm);
                }
                else
                {
                    // Criar novo formulário
                    _context.Formularios.Add(formulario);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, id = formulario.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao guardar o formulário na base de dados: " + ex.Message });
            }
        }

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
