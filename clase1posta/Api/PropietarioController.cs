using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using clase1posta.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace clase1posta.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietarioController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public PropietarioController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        // GET: api/Propietario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Propietario>>> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(context.Propietarios.Where(x => x.email == usuario));

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }



        // GET: api/Propietario/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Propietario
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Propietario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Propietario p)
        {
            return Ok("value");

  
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar(Propietario p)
        {

            try
            { 
                if(p.clave != "") //no ingreso ninguna clave
                {
                    if (ModelState.IsValid && context.Propietarios.AsNoTracking().Where(x => x.email == User.Identity.Name) != null)
                    {
                        p.clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                   password: p.clave,
                                   salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                                   prf: KeyDerivationPrf.HMACSHA1,
                                   iterationCount: 1000,
                                   numBytesRequested: 256 / 8));

                        var x = context.Propietarios.AsNoTracking().FirstOrDefault(e => e.email == User.Identity.Name);

                        p.idPropietario = x.idPropietario;

                        context.Propietarios.Update(p);

                        context.SaveChanges();

                        return Ok(p);
                    }
                    else
                    {
                        return BadRequest("ERROR DATOS INCORRECTOS");
                    }
                }
                else
                {
                    if (ModelState.IsValid && context.Propietarios.AsNoTracking().Where(x => x.email == User.Identity.Name) != null)
                    {
                        var x = context.Propietarios.AsNoTracking().FirstOrDefault(e => e.email == User.Identity.Name);

                        p.idPropietario = x.idPropietario;

                        p.clave = x.clave;

                        context.Propietarios.Update(p);

                        context.SaveChanges();

                        return Ok(p);
                    }
                    else
                    {
                        return BadRequest("ERROR DATOS INCORRECTOS");
                    }
                }
               
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }


        }




        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView loginView)
        {
            try
            {
                if (loginView.Email == null || loginView.Email == null)
                {
                    return BadRequest("Ingrese todos los campos");
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                 var p = context.Propietarios.FirstOrDefault(x => x.email == loginView.Email);

                if (p == null || p.clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));

                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.email),

                        new Claim("FullName", p.nombre + " " + p.apellido),

                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
