using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class Inmueble
    {
        [Key]
        public int IdInmueble { get; set; }
        [ForeignKey("IdPropietario")]
        public Propietario Propietario { get; set; }
        public int IdPropietario { get; set; }
        public string Direccion { get; set; }
        [ForeignKey("IdTipoInmueble")]
        public TipoInmueble TipoInmueble { get; set; }
        [Display(Name = "Tipo De Inmueble")]
        public int IdTipoInmueble { get; set; }
        public int CantAmbientes { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
       

    }
}
