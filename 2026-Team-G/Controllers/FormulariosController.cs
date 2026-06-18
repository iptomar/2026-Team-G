using _2026_Team_G.Data;
using _2026_Team_G.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _2026_Team_G.Controllers
{
    [Authorize]
    public class FormulariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormulariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formularios
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewBag.ActivePage = "Formularios";
            // Vai buscar os componentes e passa-os para a View
            ViewBag.ComponentesDisponiveis = await _context.Components.ToListAsync();

            var formularios = await _context.Formularios.ToListAsync();
            return View(formularios);
        }

        // GET: Formularios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.ActivePage = "Formularios";
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.Formularios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formulario == null)
            {
                return NotFound();
            }

            return View(formulario);
        }

        // GET: Formularios/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.ActivePage = "Formularios";
            return View();
        }

        // POST: Formularios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsActive")] Formulario formulario)
        {
            ViewBag.ActivePage = "Formularios";
            if (ModelState.IsValid)
            {
                _context.Add(formulario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(formulario);
        }

        // GET: Formularios/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.ActivePage = "Formularios";
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.Formularios.FindAsync(id);
            if (formulario == null)
            {
                return NotFound();
            }
            return View(formulario);
        }

        // POST: Formularios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsActive")] Formulario formulario)
        {
            ViewBag.ActivePage = "Formularios";
            if (id != formulario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(formulario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormularioExists(formulario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(formulario);
        }

        // GET: Formularios/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.ActivePage = "Formularios";
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.Formularios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formulario == null)
            {
                return NotFound();
            }

            return View(formulario);
        }

        // POST: Formularios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.ActivePage = "Formularios";
    
            // 1. Procurar o formulário incluindo os Fields e as Submissões (com as Respostas)
            var formulario = await _context.Formularios
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formulario != null)
            {
                // 2. Procurar e apagar todas as submissões associadas a este formulário
                // Nota: Se as Respostas tiverem Cascade Delete com a Submissão, isto basta.
                var submissoes = await _context.Submissoes
                    .Include(s => s.Respostas)
                    .Where(s => s.FormularioId == id)
                    .ToListAsync();

                if (submissoes.Any())
                {
                    // Remover primeiro as respostas de cada submissão (para evitar o mesmo erro nas respostas)
                    foreach (var sub in submissoes)
                    {
                        if (sub.Respostas != null && sub.Respostas.Any())
                        {
                            _context.Respostas.RemoveRange(sub.Respostas);
                        }
                    }
                    // Remover as submissões em si
                    _context.Submissoes.RemoveRange(submissoes);
                }

                // 3. Remover os campos (Fields) associados ao formulário
                if (formulario.Fields != null && formulario.Fields.Any())
                {
                    _context.RemoveRange(formulario.Fields);
                }

                // 4. Por fim, apagar o próprio formulário
                _context.Formularios.Remove(formulario);
        
                // Guardar todas as alterações de uma só vez na BD
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Disponiveis)); // Redireciona de volta para a lista
        }
        // GET: Formularios/Disponiveis
        [Authorize]
        public async Task<IActionResult> Disponiveis()
        {
            ViewBag.ActivePage = "Disponiveis";
            var formularios = await _context.Formularios
                .Where(f => f.IsActive)
                .Include(f => f.Fields)
                .ToListAsync();
            return View(formularios);
        }

        // GET: Formularios/Preencher/5
        [Authorize]
        public async Task<IActionResult> Preencher(int? id)
        {
            ViewBag.ActivePage = "Disponiveis";
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.Formularios
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (formulario == null || !formulario.IsActive)
            {
                return NotFound();
            }

            // Ordenar os campos pelo OrderIndex
            formulario.Fields = formulario.Fields.OrderBy(f => f.OrderIndex).ToList();

            return View(formulario);
        }

        // POST: Formularios/Submeter/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Submeter(int id, IFormCollection formCollection)
        {
            var formulario = await _context.Formularios
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (formulario == null || !formulario.IsActive)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var submissao = new Submissao
            {
                FormularioId = id,
                UtilizadorId = userId,
                DataSubmissao = DateTime.Now
            };

            foreach (var field in formulario.Fields)
            {
                // Obter o valor submetido pelo ID do campo dinâmico
                var valor = formCollection[field.FieldId].ToString();

                if (field.IsRequired && string.IsNullOrWhiteSpace(valor))
                {
                    ModelState.AddModelError(field.FieldId, $"O campo '{field.Label}' é obrigatório.");
                }

                var resposta = new Resposta
                {
                    FormFieldModelId = field.Id,
                    Valor = valor ?? ""
                };
                submissao.Respostas.Add(resposta);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ActivePage = "Disponiveis";
                formulario.Fields = formulario.Fields.OrderBy(f => f.OrderIndex).ToList();
                return View("Preencher", formulario);
            }

            _context.Submissoes.Add(submissao);
            await _context.SaveChangesAsync();

            return RedirectToAction("HistoricoFormulariosUtilizador", "Home");
        }

        // GET: Formularios/DetalhesSubmissao/5
        [Authorize]
        public async Task<IActionResult> DetalhesSubmissao(int? id)
        {
            ViewBag.ActivePage = "Historico";
            if (id == null)
            {
                return NotFound();
            }

            var submissao = await _context.Submissoes
                .Include(s => s.Formulario)
                .Include(s => s.Respostas)
                    .ThenInclude(r => r.FormFieldModel)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (submissao == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && submissao.UtilizadorId != userId)
            {
                return Challenge();
            }

            return View(submissao);
        }

        [Authorize]
        public async Task<IActionResult> ExportarFormularioPDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.Formularios
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (formulario == null || !formulario.IsActive)
            {
                return NotFound();
            }

            // Ordenar os campos (se tiveres um OrderIndex)
            formulario.Fields = formulario.Fields.OrderBy(f => f.OrderIndex).ToList();

            // Limpar caracteres especiais do título (mesma lógica de segurança)
            string tituloSeguro = formulario.Title
                .Replace("á", "a").Replace("à", "a").Replace("ã", "a").Replace("â", "a").Replace("Á", "A").Replace("Ã", "A")
                .Replace("é", "e").Replace("ê", "e").Replace("É", "E").Replace("Ê", "E")
                .Replace("í", "i").Replace("Í", "I")
                .Replace("ó", "o").Replace("õ", "o").Replace("ô", "o").Replace("Ó", "O").Replace("Õ", "O")
                .Replace("ú", "u").Replace("Ú", "U")
                .Replace("ç", "c").Replace("Ç", "C")
                .Replace(" ", "_");

            return new ViewAsPdf("FormularioVazioPDF", formulario)
            {
                FileName = $"Formulario_Em_Branco_{tituloSeguro}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 15, 15, 15)
            };
        }

        [Authorize]
        public async Task<IActionResult> ExportarPDF (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submissao = await _context.Submissoes
                .Include(s => s.Formulario)
                .Include(s => s.Respostas)
                    .ThenInclude(r => r.FormFieldModel)
                .Include(s => s.Utilizador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (submissao == null)
            {
                return NotFound();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole("Admin") && submissao.UtilizadorId != userId)
            {
                return Challenge();
            }

            string tituloSeguro = submissao.Formulario.Title
                .Replace("á", "a").Replace("à", "a").Replace("ã", "a").Replace("â", "a").Replace("Á", "A").Replace("Ã", "A")
                .Replace("é", "e").Replace("ê", "e").Replace("É", "E").Replace("Ê", "E")
                .Replace("í", "i").Replace("Í", "I")
                .Replace("ó", "o").Replace("õ", "o").Replace("ô", "o").Replace("Ó", "O").Replace("Õ", "O")
                .Replace("ú", "u").Replace("Ú", "U")
                .Replace("ç", "c").Replace("Ç", "C")
                .Replace(" ", "_");

            return new ViewAsPdf("DetalhesSubmissaoPDF", submissao)
            {
                FileName = $"Formulario_{tituloSeguro}_{submissao.Id}.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(10, 10, 10, 10)
            };

        }

        private bool FormularioExists(int id)
        {
            return _context.Formularios.Any(e => e.Id == id);
        }
    }
}