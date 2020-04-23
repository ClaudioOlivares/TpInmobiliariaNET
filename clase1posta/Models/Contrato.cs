using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }
        public Inquilino Inquilino { get; set; }
        public int IdInquilino { get; set; }
        public String DniInquilino { get; set; }
        public Inmueble Inmueble { get; set; }
        public int IdInmueble { get; set; }
        public String DireccionInmueble { get; set; }
        public Decimal PrecioInmueble { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }


     




    }
}
