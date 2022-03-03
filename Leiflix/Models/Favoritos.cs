using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Models
{
    public class Favoritos
    {
         public int Id { get; set; }
         public string UserId { get; set; } 
         public int FilmeId { get; set; }
         public string FilmeName { get; set; }
         public string ImageName { get; set; }

    }
}
