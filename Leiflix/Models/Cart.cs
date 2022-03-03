using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string seatNum { get; set; }
        public string UserId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:f}")]
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int MovieId { get; set; }
        public string FilmeName { get; set; }
        public string Sala { get; set; }
        public DateTime BuyDate { get; set; }
    }
}
