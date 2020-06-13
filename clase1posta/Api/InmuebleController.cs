using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using clase1posta.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace clase1posta.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

        public InmuebleController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        // GET: api/Inmueble
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inmueble>>> Get()
        {
            try
            {
                var user = User.Identity.Name;

              var j = context.Inmuebles.Include(x => x.Propietario).Include(x => x.TipoInmueble).Where(x => x.Propietario.email == user).ToList();
              //  var j = context.Inmuebles;
                return j;

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // GET: api/Inmueble/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inmueble>> Get(int id)
        {
            try
            {
                var j = context.Inmuebles.Include(x => x.TipoInmueble).Where(x => x.IdInmueble == id);

                return Ok(j);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // POST: api/Inmueble
        [HttpPost]
        public async Task<ActionResult> Post(Inmueble i)
        {
            if(ModelState.IsValid)
            {
                i.IdPropietario = context.Propietarios.Single(e => e.email == User.Identity.Name).idPropietario;
                context.Inmuebles.Add(i);
                context.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = i.IdInmueble }, i);

            }
            return BadRequest();
        }

        // PUT: api/Inmueble/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Inmueble i)
        {
            if (ModelState.IsValid && context.Inmuebles.AsNoTracking().Where(x => x.IdInmueble == id) != null)
            {
               
                var x = context.Inmuebles.AsNoTracking().FirstOrDefault(e => e.IdInmueble == id);

              //  var j = context.TipoInmueble.AsNoTracking().FirstOrDefault(t => t.NombreTipo == i.TipoInmueble.NombreTipo);

                i.IdPropietario = x.IdPropietario;

                i.IdInmueble = id;

                context.Inmuebles.Update(i);

                context.SaveChanges();

                return Ok(i);
            }
            else
            {
                return BadRequest("ERROR DATOS INCORRECTOS");
            }
        }

        // DELETE: api/Controller/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
           if(context.Inmuebles.Include(x=> x.Propietario).FirstOrDefault(x=>x.IdInmueble == id && x.Propietario.email == User.Identity.Name) != null)
            {
                if (context.Contratos.Include(e=>e.Inmueble).FirstOrDefault(e => e.IdInmueble == id) == null)
                {
                    var entidad = context.Inmuebles.FirstOrDefault(x => x.IdInmueble == id);

                    context.Inmuebles.Remove(entidad);

                    context.SaveChanges();

                    return Ok("Borraste con exito");
                }
                else
                {
                    return BadRequest("ERROR TIENE CONTRATO");
                }
            }
            else
            {
                return BadRequest("no es tu propiedad");
            }

        }
    }
}
