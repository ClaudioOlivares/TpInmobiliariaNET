using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }
        public Inquilino Inquilino { get; set; }
        public int IdInquilino { get; set; }
        public Inmueble Inmueble { get; set; }
        public int IdInmueble { get; set; }
        public int Duracion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public Decimal Precio { get; set; }

    }
}
