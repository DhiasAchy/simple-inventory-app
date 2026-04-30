using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleInventoryApp.Models;
using System.Linq;
using SimpleInventoryApp.Helpers;

namespace SimpleInventoryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 🔒 CEK LOGIN
            if (HttpContext.Session.GetString(SessionKey.Username) == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // 📊 DATA
            var totalItems = _context.Items.Count();
            var totalStock = _context.Items.Sum(i => i.Stock);
            var itemNames = _context.Items.Select(i => i.Name).ToList();
            var itemStocks = _context.Items.Select(i => i.Stock).ToList();


            // 📦 KIRIM KE VIEW
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalStock = totalStock;
            ViewBag.Username = HttpContext.Session.GetString(SessionKey.Username);
            ViewBag.ItemNames = itemNames;
            ViewBag.ItemStocks = itemStocks;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
