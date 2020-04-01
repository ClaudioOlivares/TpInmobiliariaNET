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
    public class InquilinoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInquilino repositorioInquilino;
        public InquilinoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioInquilino = new RepositorioInquilino(configuration);
        }
        // GET: Inquilino
        public ActionResult Index()
        {
            var lista = repositorioInquilino.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilino/Details/5
        public ActionResult Details(int id)
        {
            Inquilino p = repositorioInquilino.ObtenerPorId(id);
            return View(p);
        }

        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino p)
        {
            try
            {
                // TODO: Add insert logic here
                repositorioInquilino.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            var persona = repositorioInquilino.ObtenerPorId(id);
            return View(persona);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino pi = null;
            try
            {
                // TODO: Add update logic here
                pi = repositorioInquilino.ObtenerPorId(id);
                pi.nombre = collection["nombre"];
                pi.apellido = collection["apellido"];
                pi.dni = collection["dni"];
                pi.trabajo = collection["trabajo"];
                pi.nombreGarante = collection["nombreGarante"];
                pi.apellidoGarante = collection["apellidoGarante"];
                pi.dniGarante = collection["dniGarante"];
                repositorioInquilino.Modificacion(pi);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                
                return View();
            }
        }

        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id)
        {
            var persona = repositorioInquilino.ObtenerPorId(id);
            return View(persona);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repositorioInquilino.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}