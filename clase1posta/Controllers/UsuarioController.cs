using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clase1posta.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace clase1posta.Controllers
{
    public class UsuarioController : Controller
    {
       
            private readonly IConfiguration configuration;
            private readonly RepositorioUsuario repoUsuario;
            private readonly HomeController hc;
            public UsuarioController(IConfiguration configuration)
            {
                this.configuration = configuration;
                repoUsuario = new RepositorioUsuario(configuration);
            }
            // GET: Usuario
            public ActionResult Index()
            {
                return View();
            }

            // GET: Usuario/Details/5
            public ActionResult Details(int id)
            {
                return View();
            }

            // GET: Usuario/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Usuario/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Usuario u)
            {
               
                    // TODO: Add insert logic here
                    u.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                           password: u.Clave,
                           salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                           prf: KeyDerivationPrf.HMACSHA1,
                           iterationCount: 1000,
                           numBytesRequested: 256 / 8));
               try
               {
                     repoUsuario.Alta(u);

                    return RedirectToAction("Index", hc);

                }
                catch(Exception ex)
                {
                    return View();
                }
            }

            // GET: Usuario/Edit/5
            public ActionResult Edit(int id)
            {
                return View();
            }

            // POST: Usuario/Edit/5
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

            // GET: Usuario/Delete/5
            public ActionResult Delete(int id)
            {
                return View();
            }

            // POST: Usuario/Delete/5
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
