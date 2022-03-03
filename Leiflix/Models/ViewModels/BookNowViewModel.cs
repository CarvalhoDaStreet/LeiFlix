using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.ViewModels
{
    public class BookNowViewModel
    {
        public string FilmeName { get; set; }
      
        public DateTime FilmeDate { get; set; }

        public string seatNum { get; set; }
        public int Price { get; set; }
        public int FilmeId { get; set;  }

        public string Sala { get; set; }

        public DateTime BuyDate { get; set; }
       
    }
}
