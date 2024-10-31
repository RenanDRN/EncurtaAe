using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using App.Models;

namespace App.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessUrl(UrlModel model)
        {
            // Exibir no console a mensagem que o usuário digitou no formulário
            Console.WriteLine($"URL digitada: {model.InputUrl}");

            // Redirecionar para a página inicial após o processamento
            return RedirectToAction("Index");
        }
    }
}