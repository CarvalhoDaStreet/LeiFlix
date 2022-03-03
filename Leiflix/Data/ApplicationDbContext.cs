using Leiflix.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Leiflix.Models.ViewModels;

namespace Leiflix.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leiflix.Models.Perfil> Perfils { get; set; }
        public DbSet<Leiflix.Models.Categoria> Categoria { get; set; }
        public DbSet<Leiflix.Models.Filme> Filme { get; set; }
       
        public DbSet<Leiflix.Models.Sessao> Sessao { get; set; }
       
       public DbSet<Leiflix.Models.Cart> Cart { get; set; }
        public DbSet<Leiflix.Models.Favoritos> Favoritos { get; set; }

        public DbSet<Leiflix.Models.ViewModels.FavoritosViewModel> FavoritosViewModel { get; set; }


    }
}
