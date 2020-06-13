using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using clase1posta.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace clase1posta.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly RepositorioUsuario repoUsuario;
        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repoUsuario = new RepositorioUsuario(configuration);
        }
       

        public IActionResult Restringido()
        {
            return View();
        }
        [AllowAnonymous]

        public IActionResult Index()
        {
        
            return View();

        }
        [AllowAnonymous]
        public async Task<IActionResult> Login( LoginView l)
        {
            Usuario u = repoUsuario.ObtenerPorEmail(l.Email);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: l.Clave,
                   salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                   prf: KeyDerivationPrf.HMACSHA1,
                   iterationCount: 1000,
                   numBytesRequested: 256 / 8));
            if(u == null || u.Clave != hashed)
            {

                ModelState.AddModelError("","MAL");
                return View("Index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, u.Email ),
                new Claim("FullName", u.Nombre + " " + u.Apellido),
                new Claim(ClaimTypes.Role, u.Rol),                   
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)

                );


            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
