using InthinkTech.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InthinkTech.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           
            return View();
        }

        
    }
}
