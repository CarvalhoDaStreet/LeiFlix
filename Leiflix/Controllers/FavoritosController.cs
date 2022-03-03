using Leiflix.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public FavoritosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public IActionResult Index()
        {
            var item = _context.Favoritos.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).ToList();
            return View(item);
        }

        public async Task<IActionResult> RemoveFavorite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Favoritos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

      
        [HttpPost, ActionName("RemoveFavorite")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Favoritos.FindAsync(id);
            _context.Favoritos.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
