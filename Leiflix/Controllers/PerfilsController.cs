using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Leiflix.Data;
using Leiflix.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Leiflix.Controllers
{
    public class PerfilsController : Controller
    {
        private readonly ApplicationDbContext _context;
       

        public PerfilsController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfil()
        {
            return await _context.Perfils.Include(c => c.Categoria).ToListAsync();
        }

        
        public async Task<ActionResult<Perfil>> GetPerfil(int id)
        {
            var perfil = await _context.Perfils.Include(c => c.Categoria).SingleOrDefaultAsync(c => c.Id == id);

            if(perfil == null)
            {
                return NotFound();
            }
            return perfil;
        }

        

        // GET: Perfils
        
        public async Task<IActionResult> Index()
        {
             var model = await _context.Perfils
                            .Where(a => a.Username == HttpContext.User.Identity.Name)
                            .Include(p => p.Categoria)
                            .ToListAsync();
             return View(model);
  
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersList()
        {
            var model = await _context.Perfils.Include(p => p.Categoria).ToListAsync();
            return View(model);
        }

        // GET: Perfils/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            
            if (id == null)
            {
                return NotFound();
            }
 
                var perfil = await _context.Perfils
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (perfil == null)
                {
                    return NotFound();
                }

                return View(perfil);
        }

      
        // GET: Perfils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
          
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils.FindAsync(id);
            if (perfil == null || perfil.Username != User.Identity.Name)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Name", perfil.CategoriaId);
            return View(perfil);
        }

        // POST: Perfils/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id,Email,Nome,Username,CategoriaId")] Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perfil.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Name", perfil.CategoriaId);
            return View(perfil);
        }
    
        private bool PerfilExists(int id)
        {
            return _context.Perfils.Any(e => e.Id == id);
        }

    }
}
