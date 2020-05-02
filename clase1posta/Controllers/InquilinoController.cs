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
                TempData["mensaje"] = "Exito";
                TempData["mensaje2"] = "El Inquilino cargado fue dado de alta correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["mensaje"] = "Error";
                TempData["mensaje2"] = "El Inquilino cargado no fue dado de alta ";
                return RedirectToAction(nameof(Index));
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
                TempData["mensaje"] = "Exito";
                TempData["mensaje2"] = "El Inquilino fue Modificado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                TempData["mensaje"] = "Error";
                TempData["mensaje2"] = "El Inquilino no pudo ser modificado";
                return View();
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
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
                TempData["mensaje"] = "Exito";
                TempData["mensaje2"] = "El Inquilino fue Elimnado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["mensaje"] = "error";
                TempData["mensaje2"] = "El Inquilino no pudo ser Eliminado";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}