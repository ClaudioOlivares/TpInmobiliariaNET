﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class Propietario
    {
        [Key]
        public int idPropietario { get; set; }
       
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string dni { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string clave { get; set; }
    }
}
