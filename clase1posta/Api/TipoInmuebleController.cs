using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clase1posta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace clase1posta.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoInmuebleController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration config;

       

        public TipoInmuebleController(DataContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        // GET: api/TipoInmueble
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoInmueble>>> Get()
        {
            try
            {
                var j = context.TipoInmueble;
                return Ok(j);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        // GET: api/TipoInmueble/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TipoInmueble
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TipoInmueble/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
