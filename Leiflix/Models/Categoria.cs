using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leiflix.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Required field")]
        public string Name { get; set; }

        public virtual ICollection<Filme> Filmes { get; set; }
        public virtual ICollection<Perfil> Perfils { get; set; }

    }
}
