using Microsoft.AspNetCore.Mvc;
using MarketingCORE.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CORE;
using MarketingCORE.Migrations;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MarketingCORE.Controllers
{
    
    public class AccesoController : Controller
    {
        private readonly MarketingCOREContext _context;
        public AccesoController(MarketingCOREContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Registrar()
        {

            return RedirectToAction("Create", "Usuarios");
        }
        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {

            //DA_Logic _da_usuario = new DA_Logic();
            //var usuario = _da_usuario.ValidarUsuario(_usuario.Correo, _usuario.Clave);
            var listado = await _context.Usuario.ToListAsync();
            var usuario = listado.Where(item => item.Correo == _usuario.Correo && item.Clave == _usuario.Clave).FirstOrDefault();

            if (usuario != null)
            {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,usuario.Nombre),
                    new Claim(ClaimTypes.Email,usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Roles)
                };
                var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.User = new ClaimsPrincipal(claimsIdentity);
                var roles = HttpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            
        }
        
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Acceso");
        }
    }
}
