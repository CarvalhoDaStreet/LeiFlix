using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Leiflix.Data;
using Leiflix.Models;
using Microsoft.AspNetCore.Authorization;

namespace Leiflix.Controllers
{
    [Authorize(Roles = "Admin, Func")]
    public class SessaosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessaosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sessaos
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.Sessao.Include(s => s.Filme);

           
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sessaos/Create
        public IActionResult Create()
        {
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Name");
            return View();
        }

        // POST: Sessaos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sala,RoomSeats,Date,FilmeId")] Sessao sessao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sessao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Name", sessao.FilmeId);
            return View(sessao);
        }

        // GET: Sessaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessao.FindAsync(id);
            if (sessao == null)
            {
                return NotFound();
            }
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Name", sessao.FilmeId);
            return View(sessao);
        }

        // POST: Sessaos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sala,RoomSeats,Date,FilmeId")] Sessao sessao)
        {
            if (id != sessao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessaoExists(sessao.Id))
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
            ViewData["FilmeId"] = new SelectList(_context.Filme, "Id", "Name", sessao.FilmeId);
            return View(sessao);
        }

        // GET: Sessaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessao = await _context.Sessao
                .Include(s => s.Filme)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessao == null)
            {
                return NotFound();
            }

            return View(sessao);
        }

        // POST: Sessaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessao = await _context.Sessao.FindAsync(id);
            _context.Sessao.Remove(sessao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessaoExists(int id)
        {
            return _context.Sessao.Any(e => e.Id == id);
        }
    }
}
