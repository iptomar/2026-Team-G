using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using _2026_Team_G.Data;
using _2026_Team_G.Models;

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
            var formulario = await _context.Formularios.FindAsync(id);
            if (formulario != null)
            {
                _context.Formularios.Remove(formulario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Formularios/Disponiveis
        [Authorize]
        public async Task<IActionResult> Disponiveis()
        {
            ViewBag.ActivePage = "Disponiveis";
            var formularios = await _context.Formularios
                .Where(f => f.IsActive)
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

        private bool FormularioExists(int id)
        {
            return _context.Formularios.Any(e => e.Id == id);
        }
    }
}