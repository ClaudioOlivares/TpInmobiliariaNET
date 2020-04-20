using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class Inquilino
    {
        [Key]
        public int idInquilino { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public String dni { get; set; }
        public String trabajo { get; set; }
        public String nombreGarante { get; set; }
        public String apellidoGarante { get; set; }
        public String dniGarante { get; set; }


    }
}
