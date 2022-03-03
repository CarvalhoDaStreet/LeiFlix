using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Models
{
    public class Sessao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Room")]
        public string Sala { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Room")]
        public int RoomSeats { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Session Date ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:f}")]
        public DateTime Date { get; set; }

        [ForeignKey("FilmeId")]
        public int? FilmeId { get; set; }
        public  Filme Filme { get; set; }

        
    }
}
