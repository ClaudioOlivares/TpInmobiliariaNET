using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clase1posta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace clase1posta.Controllers
{
    [Authorize]
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
           
               var a = repoContrato.TraerFechaCercana(c.FechaInicio, c.FechaFinal,c.IdInmueble);

                // TODO: Add insert logic here
                var todosLosContratos = repoContrato.ObtenerTodosPorId(c.IdInmueble);
                   if (todosLosContratos != null)
                    {
                        foreach (var item in todosLosContratos)
                        {
                            if ((c.FechaInicio >= item.FechaInicio && c.FechaInicio <= item.FechaFinal) || (c.FechaFinal <= item.FechaFinal && c.FechaFinal >= item.FechaInicio))
                            {
                                TempData["mensaje"] = "Error";
                                TempData["mensaje2"] = "Inmueble Ocupado para la fecha ingresada";
                                return RedirectToAction(nameof(Index));

                            }
                        }
                    }
                if (a != null)
                {
                    if (c.FechaFinal >= a.FechaInicio && c.FechaFinal <= a.FechaFinal)
                    {
                        //meter advertencia aca
                        TempData["mensaje"] = "Error";
                        TempData["mensaje2"] = "Inmueble Ocupado para la fecha ingresada";
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

            TempData["mensaje"] = "Exito";
            TempData["mensaje2"] = "El Contrato cargado fue dado de alta correctamente";
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
                TempData["mensaje"] = "Exito";
                TempData["mensaje2"] = "contrato modificado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Cancelar( int id)
        {
            decimal impuesto = 0;
            var i = 0;
            var pagos = repoPagos.ObtenerTodosPagosDe(id);
            foreach (var item in pagos)
            {
                if(item.Estado == true)
                {
                    i++;
                }
                else
                {
                    repoPagos.Baja(item.IdPago);
                }
            }
            
            var contrato = repoContrato.ObtenerPorId(id);

            if( i >= contrato.Duracion /2)
            {
                impuesto = contrato.Inmueble.Precio * 2;
            }
            else
            {
                impuesto = contrato.Inmueble.Precio;
            }
            contrato.FechaFinal = DateTime.Now;
            repoContrato.Modificacion(contrato);
            TempData["mensaje"] = "Adv";
            TempData["mensaje2"] = "Contrato Cancelado, Impuesto a Pagar: " + impuesto;
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "Administrador")]
        // GET: Contrato/Delete/5
        public ActionResult Delete(int id)
        {
            var c = repoContrato.ObtenerPorId(id);
            return View(c);
        }
        [Authorize(Policy = "Administrador")]
        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repoContrato.Baja(id);
                TempData["mensaje"] = "Exito";
                TempData["mensaje2"] = "contrato Eliminado correctamente";
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                TempData["mensaje"] = "Error";
                TempData["mensaje2"] = "contrato no pudo Eliminado correctamente";
                return View();
            }
        }
    }
}