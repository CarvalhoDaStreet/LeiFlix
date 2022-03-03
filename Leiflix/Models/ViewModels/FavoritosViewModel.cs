using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Models.ViewModels
{
    public class FavoritosViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FilmeId { get; set; }
        public string FilmeName { get; set; }
        public string ImageName { get; set; }

    }
}
