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
        public int IdInmueble { get; set; }
        
        public Propietario Propietario { get; set; }
        public int IdPropietario { get; set; }
    
        public string Direccion { get; set; }
        public TipoInmueble Tipo { get; set; }
        [Display(Name = "Tipo De Inmueble")]
        public int IdTipo { get; set; }
        public int CantAmbientes { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
       

    }
}
