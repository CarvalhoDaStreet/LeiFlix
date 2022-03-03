 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Leiflix.Data;
using Leiflix.Models;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Leiflix.ViewModels;
using Microsoft.AspNetCore.Identity;
using Leiflix.Models.ViewModels;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using Stripe;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;

namespace Leiflix.Controllers
{
    [Authorize]
    public class FilmesController : Controller
    {
        
        int count = 1;
        bool flag = true;
   
        private UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;


        public FilmesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager,IEmailSender emailSender)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _emailSender = emailSender;
        }

       
        // GET: Filmes
        [AllowAnonymous]
        public async Task<IActionResult> Index(string filmename)
        {

            if (string.IsNullOrEmpty(filmename))
            {
                var applicationDbContext = _context.Filme.Include(f => f.Categoria);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var searchItem = await _context.Filme.Where(s => s.Name.Contains(filmename) || s.Actors.Contains(filmename) || s.Producers.Contains(filmename) || s.Price.ToString().Contains(filmename)).ToListAsync();
                return View(searchItem);
            }
        }
          
      

        [HttpGet]
        public IActionResult Favorite(int id)
        {
            FavoritosViewModel fm = new FavoritosViewModel();
            var item = _context.Filme.Where(a => a.Id == id).FirstOrDefault();
            var it = _userManager.GetUserId(HttpContext.User);
            fm.FilmeId = item.Id;
            fm.UserId = it;
            fm.FilmeName = item.Name;
            fm.ImageName = item.ImageName;

            return View(fm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Favorite(FavoritosViewModel fm, int Id)
        {
          
            List <Favoritos> favs = new List<Favoritos>();
            int filmeId = fm.FilmeId;
            string userId = fm.UserId;
            string filmeName = fm.FilmeName;
            string imageName = fm.ImageName;
            
            
            
            var fav = new Favoritos
            {
                FilmeId = fm.FilmeId,
                UserId = _userManager.GetUserId(HttpContext.User),
                FilmeName = fm.FilmeName,
                ImageName = fm.ImageName
            };
                _context.Favoritos.Add(fav); 
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
           
        }

      
        [HttpGet]
        public  IActionResult BookNow(int Id)
        { 

            ViewBag.FilmeDate = _context.Sessao.Where(a => a.FilmeId == Id)
                     .Select(i => new SelectListItem
                     {
                         Value = i.Date.ToString(),
                         Text = i.Date.ToString(),
                     }).ToList();

            BookNowViewModel vm = new BookNowViewModel();
            var item = _context.Filme.Where(a => a.Id == Id).FirstOrDefault();
            var it = _context.Sessao.Where(n => n.FilmeId == Id).FirstOrDefault();
            vm.FilmeName = item.Name;
            vm.FilmeDate = it.Date;
            vm.Sala = it.Sala;
            vm.FilmeId = Id;
            vm.Price = Convert.ToInt32(item.Price);
            vm.BuyDate = DateTime.Now;
            vm.Sala = it.Sala;
            TempData["roomSeats"] = it.RoomSeats.ToString();


            return  View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> BookNow(BookNowViewModel vm, int Id,string selected)
        {


            List<Cart> carts = new List<Cart>();
            string seatNum = vm.seatNum.ToString();
            int filmeId = vm.FilmeId;
            var filme = _context.Filme.Where(a => a.Id == Id).FirstOrDefault();
            var sessao = _context.Sessao.Where(n => n.FilmeId == Id).FirstOrDefault();
            vm.FilmeDate = sessao.Date;
            vm.FilmeName = filme.Name;
            vm.Sala = sessao.Sala;
            TempData["roomSeats"] = sessao.RoomSeats.ToString();

            string[] seatNumArray = seatNum.Split(",");
            count = seatNumArray.Length;

                if (checkSeat(seatNum, filmeId, selected) == false)
                {
                    foreach (var item in seatNumArray) {
                    if (int.Parse(item) <= sessao.RoomSeats)
                    {
                        {
                            var cart = new Cart
                            {
                                Price = Convert.ToInt32(filme.Price),
                                MovieId = vm.FilmeId,
                                UserId = _userManager.GetUserId(HttpContext.User),
                                Date = DateTime.Parse(selected),
                                Sala = vm.Sala,
                                seatNum = item,
                                BuyDate = DateTime.Now,
                                FilmeName = vm.FilmeName
                                
                            };
                            TempData["date"] = DateTime.Parse(selected);
                            TempData["seat"] = item;
                            TempData["room"] = vm.Sala;
                            _context.Cart.Add(cart);
                        }
                    }
                    else
                    {
                        TempData["seatMax"] = "Invalid Input, The Room has a max of " + TempData["roomSeats"] + " seats";
                        return RedirectToAction("BookNow");
                    }
                }

                    await _context.SaveChangesAsync();
                    TempData["Sucess"] = "Seat Booked, Click on the pay button";
                   
                }
                else
                {
                    TempData["seatnummsg"] = "Please Change you seat number";
                    return RedirectToAction("BookNow");
                }
            
            return RedirectToAction("BookNow");
        }


        private bool checkSeat(string seatNum, int filmeId,string selected)
        {
            string seats = seatNum;
            string[] seatReserve = seats.Split(',');
            var seatNumList = _context.Cart.Where(a => a.MovieId == filmeId).Where(a=> a.Date == DateTime.Parse(selected)).ToList();
            foreach (var item in seatNumList)
            {
                string alreadyBook = item.seatNum;
                foreach (var item1 in seatReserve)
                {
                    if (item1 == alreadyBook)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult checkSeat(BookNowViewModel bookNowViewModel,string selected)
        {
            string seatNum = string.Empty;
            var filmeList = _context.Cart.Where(a => a.Date == DateTime.Parse(selected)).ToList();

            if (filmeList != null)
            {
                var getSeatNum = filmeList.Where(b => b.MovieId == bookNowViewModel.FilmeId).ToList();
                if (getSeatNum != null)
                {
                    foreach (var item in getSeatNum)
                    {
                        seatNum = seatNum + " " + item.seatNum.ToString();
                    }
                    TempData["bookedSeats"] = seatNum + "   " ;
                   
                }
            }
            return View();
        }

   
        public async Task<IActionResult> Charge(string stripeEmail,string stripeToken,int Id)
        {

            var item = _context.Cart.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).ToList();
            

            var costumers = new CustomerService();
            var charges = new ChargeService();
            var costumer = costumers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(TempData["preco"])*100 ,
                Description = TempData["filmeName"].ToString(),
                Currency = "usd",
                Customer = costumer.Id,
                Metadata = new Dictionary<string, string>()
                {
                    {"Movie Name", TempData["filmeName"].ToString() },
                }
            });

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;

                await _emailSender.SendEmailAsync(stripeEmail, "Digital Ticket",
                 "Movie: " + TempData["filmeName"].ToString() + " ; " + "Room Number: " + TempData["room"].ToString() + " ; " + "Seat Number: " + TempData["seat"].ToString() + " ; " + "Time: " + TempData["date"].ToString());

                return View(item);
             }
            else
            {

            }

             return View(item);
        }

        [AllowAnonymous]
        // GET: Filmes/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .Include(f => f.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }
            return View(filme);
        }

        // GET: Filmes/Create
        [Authorize(Roles = "Admin, Func")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Name");
            return View();
        }

      
        [Authorize(Roles = "Admin, Func")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Actors,Producers,Age,Duration,Date,Price,ImageFile,CategoriaId")] Filme filme)
        {

  
            if (ModelState.IsValid)
            {
                
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(filme.ImageFile.FileName);
                string extension = Path.GetExtension(filme.ImageFile.FileName);
                filme.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await filme.ImageFile.CopyToAsync(fileStream);
                }
                
                _context.Add(filme);
                await _context.SaveChangesAsync();

                
                var getUserEmail = _context.Perfils.Where(b=> b.CategoriaId == filme.CategoriaId  ).ToList();
                foreach (var item in getUserEmail)
                {
                    await _emailSender.SendEmailAsync(item.Email.ToString().TrimEnd(), "New Movie",
                         "A movie of your preference is on exhibition, check our website for more details!!! ");
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Name", filme.CategoriaId);
            return View(filme);
        }



        // GET: Filmes/Edit/5
        [Authorize(Roles = "Admin, Func")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme.FindAsync(id);
            TempData["imagePath"] = filme.ImageName;
            if (filme == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Name", filme.CategoriaId);
            return View(filme);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Func")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Actors,Producers,Age,Duration,Date,Price,ImageFile,CategoriaId")] Filme filme,IFormFile file)
        {
            if (id != filme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(filme.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(filme.ImageFile.FileName);
                    string extension = Path.GetExtension(filme.ImageFile.FileName);
                    filme.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await filme.ImageFile.CopyToAsync(fileStream);
                    }
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", TempData["imagePath"].ToString());
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    filme.ImageName = TempData["imagePath"].ToString();
                    _context.Entry(filme).State = EntityState.Modified;
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }

                }
                try
                {

                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(filme.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Name", filme.CategoriaId);
            return View(filme);
        }

        // GET: Filmes/Delete/5
        [Authorize(Roles = "Admin, Func")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filme
                .Include(f => f.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Func")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filme = await _context.Filme.FindAsync(id);


            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", filme.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);


            _context.Filme.Remove(filme);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return _context.Filme.Any(e => e.Id == id);
        }

        public async Task<IActionResult> FilterCategories(int id)
        {
            var applicationDbContext = _context.Filme.Where(a => a.CategoriaId == id);
            return View(await applicationDbContext.ToListAsync());
        }

    }
}
