using InthinkTech.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace InthinkTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
           return View();

        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                

                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    

                    // Redirect based on role
                    if (user.Role == "Admin")
                        return RedirectToAction("Index", "Admin");
                    else if (user.Role == "Supervisor")
                        return RedirectToAction("Index", "Supervisor");
                    else if (user.Role == "Manufacturer")
                        return RedirectToAction("Index", "Manufacturer");
                    else if (user.Role == "Operator")
                        return RedirectToAction("Index", "Operator");
                    else
                        return RedirectToAction("UserDashboard", "Dashboard");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
