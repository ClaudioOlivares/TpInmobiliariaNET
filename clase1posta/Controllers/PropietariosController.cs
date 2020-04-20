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

    public class PropietariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositiorioPropietario repositorioPropietario;
        public PropietariosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioPropietario = new RepositiorioPropietario(configuration);
        }
        // GET: Propietarios
        public ActionResult Index()
        {
             var lista = repositorioPropietario.ObtenerTodos();
            return View(lista);

        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            Propietario p = repositorioPropietario.ObtenerPorId(id);
            return View(p);
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                // TODO: Add insert logic here
                repositorioPropietario.Alta(p);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
            var persona = repositorioPropietario.ObtenerPorId(id);
            return View(persona);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                // TODO: Add update logic here
                p = repositorioPropietario.ObtenerPorId(id);
                p.nombre = collection["nombre"];
                p.apellido = collection["apellido"];
                p.dni = collection["dni"];
                p.telefono = collection["telefono"];
                p.email = collection["email"];
                repositorioPropietario.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var persona = repositorioPropietario.ObtenerPorId(id);
            return View(persona);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                repositorioPropietario.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }
    }
}

