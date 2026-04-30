using Microsoft.AspNetCore.Mvc;
using SimpleInventoryApp.Models;
using System.Linq;
using SimpleInventoryApp.Helpers;

namespace SimpleInventoryApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // FORM LOGIN
        public IActionResult Login()
        {
            return View();
        }

        // PROSES LOGIN
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var hashedPassword = PasswordHelper.Hash(password);
            // var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            // Console.WriteLine("Login Process" + hashedPassword);

            if (user != null)
            {
                // HttpContext.Session.SetString("Username", user.Username); // ✅ FIX
                // Console.WriteLine("LOGIN SUCCESS"); // 🔥 DEBUG
                // Console.WriteLine("INPUT HASH: " + hashedPassword);
                // ✅ pakai SessionKey
                HttpContext.Session.SetString(SessionKey.Username, user.Username);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }
        }
        // PROSES LOGOUT
        public IActionResult Logout()
        {
            // Hapus informasi login dari session atau cookie sesuai kebutuhan
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        // FORM REGISTER
        public IActionResult Register()
        {
            return View();
        }
        // PROSES REGISTER
        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username dan Password wajib diisi";
                return View();
            }

            // 🔥 CEK USER SUDAH ADA
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                ViewBag.Error = "Username sudah digunakan";
                return View();
            }

            // 🔐 HASH PASSWORD
            var hashedPassword = PasswordHelper.Hash(password);

            // 💾 SIMPAN KE DB
            var user = new User
            {
                Username = username,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}