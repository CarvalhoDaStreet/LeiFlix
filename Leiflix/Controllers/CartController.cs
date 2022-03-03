using Leiflix.Data;
using Leiflix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
  
        public IActionResult Index()
        { 

             var item = _context.Cart.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).ToList();
             return View(item); 
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex()
        {
                var model = await _context.Cart.ToListAsync();
                return View(model);
              }
        }

}
