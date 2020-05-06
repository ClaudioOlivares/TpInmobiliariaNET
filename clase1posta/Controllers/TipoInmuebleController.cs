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
    public class TipoInmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioTipoInmueble repositorioTipo;
        public TipoInmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioTipo = new RepositorioTipoInmueble(configuration);
        }
        // GET: TipoInmueble
        public ActionResult Index()
        {
            var p = repositorioTipo.ObtenerTodos();
            return View(p);
        }

        // GET: TipoInmueble/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoInmueble/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoInmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoInmueble c)
        {
            try
            {
                // TODO: Add insert logic here
                repositorioTipo.Alta(c);
                return RedirectToAction("Index","Usuario");
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoInmueble/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TipoInmueble/Edit/5
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

        // GET: TipoInmueble/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TipoInmueble/Delete/5
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