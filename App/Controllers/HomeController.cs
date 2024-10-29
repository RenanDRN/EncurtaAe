using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using App.Models;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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
            // Processar a URL aqui
            // Por exemplo, você pode salvar no banco de dados ou encurtar a URL
            _context.Urls.Add(model);
            _context.SaveChanges();

            // Redirecionar para a página inicial após o processamento
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}