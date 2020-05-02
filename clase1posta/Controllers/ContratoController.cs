using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clase1posta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace clase1posta.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPago repoPagos;
        private readonly RepositorioContrato repoContrato;
        private readonly RepositorioInquilino repoInquilino;
        private readonly RepositorioInmueble repoInmueble;
        





        public ContratoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repoContrato = new RepositorioContrato(configuration);
            repoInquilino = new RepositorioInquilino(configuration);
            repoInmueble = new RepositorioInmueble(configuration);
            repoPagos = new RepositorioPago(configuration);
        }
            // GET: Contrato
        public ActionResult Index()
        {
            var  c = repoContrato.ObtenerTodos();
            
            return View(c);
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Contrato/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
           
               var a = repoContrato.TraerFechaCercana(c.FechaInicio, c.FechaFinal);

                // TODO: Add insert logic here
                var todosLosContratos = repoContrato.ObtenerTodos();
                foreach (var item in todosLosContratos)
                {
                    if((c.FechaInicio >= item.FechaInicio &&  c.FechaInicio <= item.FechaFinal)||(c.FechaFinal <= item.FechaFinal && c.FechaFinal >= item.FechaInicio ))
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }
                if (a != null)
                {
                    if (c.FechaFinal >= a.FechaInicio && c.FechaFinal <= a.FechaFinal)
                    {
                        //meter advertencia aca
                        return RedirectToAction(nameof(Index));
                    }
                }
                try
                {
                    var contratocreado = repoContrato.Alta(c);
                    
                    Pago p = new Pago();
                    for (int i = 1; i <= c.Duracion; i++)
                    {
                        p.IdContrato = contratocreado;
                        p.Cuota = i;
                        p.Estado = false;
                        
                        repoPagos.Alta(p);
                        
                    }
                    
                    
                }
                catch(Exception ex)
                {
                    //mostrar errores
                    return View();
                }

                
                return RedirectToAction(nameof(Index));
           
        }



       
        // GET: Contrato/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            var c  = repoContrato.ObtenerPorId(id);
            
            return View(c);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
                // TODO: Add update logic here
                repoContrato.Modificacion(c);
                TempData["mensaje"] = "contrato modificado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contrato/Delete/5
        public ActionResult Delete(int id)
        {
            var c = repoContrato.ObtenerPorId(id);
            return View(c);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repoContrato.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}