using _2026_Team_G.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _2026_Team_G.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Home";
            return View();
        }

  
        
        public IActionResult Componentes()
        {
            ViewBag.ActivePage = "Componentes";
            return View();
        }
        public IActionResult Formulario()
        {
            return View();
        }
        public IActionResult HistoricoFormulariosUtilizador()
        {
            ViewBag.ActivePage = "Historico";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
