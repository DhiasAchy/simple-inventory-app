using Microsoft.AspNetCore.Mvc;
using SimpleInventoryApp.Models;
using System.Linq;

namespace SimpleInventoryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = _context.Items.ToList();
            return Ok(items);
        }
    }
}