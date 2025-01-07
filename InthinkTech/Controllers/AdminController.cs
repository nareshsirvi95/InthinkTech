using InthinkTech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InthinkTech.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Index action to show the Admin page with buttons
        public IActionResult Index()
        {
            var users = _context.Users.ToList();  // Fetch users for display
            return View(users);
        }

        // Action to create a user
        public IActionResult CreateUser()
        {
            return View(new User());  // A view to show a form for creating users
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.ModifiedDate = DateTime.Now;
                user.CreatedDate = DateTime.Now;
                _context.Users.Add(user);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirect to Admin Index after creating a user
            }
            return View(user);  // If the model is not valid, return the same view
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.ModifiedDate = DateTime.Now;
                    
                    _context.Update(user);  // Updates the user in the context
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));  // Redirect back to the admin dashboard
            }

            return View(user);  // If the model state is invalid, return the same view with the current user data
        }



        // Action to delete a user
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));  // Redirect back to Admin Index after deleting a user
        }
    }
}
