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

        public IActionResult Index(string shortUrl = null)
        {
            ViewBag.ShortUrl = shortUrl;
            ViewBag.ShortUrlCount = _dbService.GetShortUrlCount();
            ViewBag.LastFiveLinks = _dbService.GetLastFiveShortUrls()
                                              .Select(link => $"{Request.Scheme}://{Request.Host}/{link}")
                                              .ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult ProcessUrl(UrlModel model)
        {
            Console.WriteLine($"URL digitada: {model.InputUrl}");
            string shortUrl = _dbService.CreateShortUrl(model.InputUrl);
            string fullShortUrl = $"{Request.Scheme}://{Request.Host}/{shortUrl}";
            Console.WriteLine($"Link curto gerado: {fullShortUrl}");
            return RedirectToAction("Index", new { shortUrl = fullShortUrl });
        }
        
        [HttpGet("{hash}")]
        public IActionResult RedirectToOriginalUrl(string hash)
        {
            string originalUrl = _dbService.GetOriginalUrl(hash);
            if (originalUrl != null)
            {
                return Redirect(originalUrl);
            }
            return NotFound();
        }

        [HttpGet]
        public JsonResult GetLastFiveLinks()
        {
            var lastFiveLinks = _dbService.GetLastFiveShortUrls()
                                          .Select(link => $"{Request.Scheme}://{Request.Host}/{link}")
                                          .ToList();
            return Json(lastFiveLinks);
        }
    }
}