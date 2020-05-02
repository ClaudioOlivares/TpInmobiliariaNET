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
    public class PagoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPago repoPagos;
        private readonly RepositorioContrato repoContratos;
        private readonly RepositorioInmueble repoInmueble;
        private readonly Contrato Contratos;
        public PagoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repoPagos = new RepositorioPago(configuration);
            repoContratos = new RepositorioContrato(configuration);
            repoInmueble = new RepositorioInmueble(configuration);
            Contratos = new Contrato();
        }
            // GET: Pago
            public ActionResult Index()
        { 
            return View();
        }

        // GET: Pago/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult RealizarPago(int id)
        {
            try
            {
                var p = repoPagos.ObtenerPorId(id);
                p.FechaPago = DateTime.Now;
                p.Estado = true;
                repoPagos.Pagar(p);
                TempData["mensaje"] = "Exito";
                TempData["mensaje2"] = "Se ha registrado el Pago correctamente";
                return RedirectToAction("Index","Contrato");
            }
            catch(Exception ex)
            {

                return View("Index", Contratos);

            }

        }


        public ActionResult MostrarPagos(int id)
        {
            ViewBag.Contrato = repoInmueble.ObtenerPorId(repoContratos.ObtenerPorId(id).IdInmueble).Precio;
            var p = repoPagos.ObtenerTodosPagosDe(id);
            return View(p);
        }


        public ActionResult CancelarContrato()
        {

            return View();
        }






        // GET: Pago/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}