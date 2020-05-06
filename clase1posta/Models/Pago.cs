using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public Contrato Contrato { get; set; }
        public int IdContrato { get; set; }
        public int Cuota { get; set; }
        public decimal Precio { get; set; }
        public Boolean Estado { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
