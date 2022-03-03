using Leiflix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Categoria.Any())
            {
                return;
            }
            var categorias = new Categoria[]
            {
                new Categoria { Name = "Action"},
                new Categoria { Name = "Comedy" },
                new Categoria { Name = "Drama"},
                new Categoria { Name = "Fantasy"},
                new Categoria { Name = "Horror"},
                new Categoria { Name = "Mystery"},
                new Categoria { Name = "Romance"},
                new Categoria { Name = "Thriller"}
            };
            foreach (Categoria c in categorias)
            {
                context.Categoria.Add(c);
            }
            context.SaveChanges();

           
        }
    }
}
