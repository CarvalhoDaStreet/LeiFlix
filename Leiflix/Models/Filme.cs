using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Models
{
    public class Filme
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "length can not exceed {1} characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Actors { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Producers { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Movie Duration")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name ="Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [Display(Name ="Upload File")]
        public IFormFile ImageFile { get; set; }

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

    }
}
