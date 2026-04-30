using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleInventoryApp.Models;
using System.Linq;
using SimpleInventoryApp.Helpers;

namespace SimpleInventoryApp.Controllers
{
    [ServiceFilter(typeof(AuthFilter))] // menambahkan filter untuk semua action di controller ini
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        // TAMPILKAN DATA
        // public IActionResult Index()
        public IActionResult Index(string search, int page = 1)
        {
            // if (HttpContext.Session.GetString("Username") == null) // fix session
            if (HttpContext.Session.GetString(SessionKey.Username) == null) // session key
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // Tampilkan Username
                ViewBag.Username = HttpContext.Session.GetString(SessionKey.Username);
                int pageSize = 5; // jumlah data per halaman
                // var items = _context.Items.ToList();
                var items = _context.Items.AsQueryable();

                // 🔍 SEARCH
                if (!string.IsNullOrEmpty(search))
                {
                    items = items.Where(i => i.Name.Contains(search));
                }

                // PAGINATION
                int totalItems = items.Count();
                var data = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                // 📦 KIRIM DATA KE VIEW
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                ViewBag.Search = search;

                // return View(items);
                // return View(items.ToList());
                return View(data);
                // return PartialView("_ItemTable", data);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while loading the items. Please try again.");
                return View(new List<Item>());
            }
        }
        // 
        public IActionResult GetItems(string search, int page = 1)
        {
            int pageSize = 5;

            var items = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.Name.Contains(search));
            }

            int totalItems = items.Count();

            var data = items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.Search = search;

            return PartialView("_ItemTable", data);
        }
        // FORM TAMBAH DATA
        public IActionResult Create()
        {
            return View();
        }

        // SIMPAN DATA
        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                _context.Items.Add(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the item. Please try again.");
                return View(item);
            }

        }

        // FORM EDIT DATA
        public IActionResult Edit(int id)
        {
            var item = _context.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // UPDATE DATA
        [HttpPost]
        public IActionResult Edit(Item item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                _context.Items.Update(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the item. Please try again.");
                return View(item);
            }
        }

        // DELETE DATA
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            try
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the item. Please try again.");
                return RedirectToAction("Index");
            }
        }
    }
}