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
    public class InmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioTipoInmueble repoTipoInmueble;
        private readonly RepositiorioPropietario repositiorioPropietario;
        public InmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioInmueble = new RepositorioInmueble(configuration);
            repositiorioPropietario = new RepositiorioPropietario(configuration);
            repoTipoInmueble = new RepositorioTipoInmueble(configuration);
        }
        // GET: Inmueble
        public ActionResult Index()
        {
            var lista = repositorioInmueble.ObtenerTodos();
            return View(lista);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repositiorioPropietario.ObtenerTodos();
            ViewBag.Tipos = repoTipoInmueble.ObtenerTodos();
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                // TODO: Add insert logic here
                repositorioInmueble.Alta(i);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repositiorioPropietario.ObtenerTodos();
            ViewBag.Tipos = repoTipoInmueble.ObtenerTodos();
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i) ;
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble entidad)
        {
            try
            {
                // TODO: Add update logic here
                repositorioInmueble.Modificacion(entidad);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        [Authorize (Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble entidad)
        {
            try
            {
                // TODO: Add delete logic here
                repositorioInmueble.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}