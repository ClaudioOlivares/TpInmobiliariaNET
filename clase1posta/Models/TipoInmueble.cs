﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class TipoInmueble
    {
        [Key]
        public int IdTipoInmueble { get; set; }
      
        public String NombreTipo { get; set; }

    }
}
