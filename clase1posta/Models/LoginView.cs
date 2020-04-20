using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace clase1posta.Models
{
    public class LoginView
    {
        [Required]
        [DataType(DataType.EmailAddress)]    
        public String Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Clave { get; set; }

    }
}
