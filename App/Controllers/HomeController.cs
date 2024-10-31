using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Services;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbService _dbService;

        public HomeController(DbService dbService)
        {
            _dbService = dbService;
        }

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

            // Salvar a URL no banco de dados
            _dbService.CreateShortUrl(model.InputUrl);

            // Redirecionar para a página inicial após o processamento
            return RedirectToAction("Index");
        }
    }
}