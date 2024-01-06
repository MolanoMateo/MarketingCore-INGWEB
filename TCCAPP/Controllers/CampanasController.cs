using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CORE;
using MarketingCORE.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MarketingCORE.Controllers
{
    [Authorize(Roles = "USER")]
    public class CampanasController : Controller
    {
        private readonly MarketingCOREContext _context;

        public CampanasController(MarketingCOREContext context)
        {
            _context = context;
        }

        // GET: Campanas

        public async Task<IActionResult> Index(string objetivo, string canal, string buyer)
        {
            List<Campana> productList = await _context.Campana.ToListAsync();
            List<Buyer> buyersList = await _context.Buyer.ToListAsync();
            List<campanaView> filteredList = new List<campanaView>();
            foreach (Campana c in productList)
            {
                Buyer b = buyersList.Where(p => p.Id==c.BuyerID).ToList().First();
                filteredList.Add(new campanaView { buyer=b, campana=c});
            }

            if (!string.IsNullOrEmpty(objetivo))
            {
                filteredList = filteredList.Where(p => p.campana.Objetivo.Equals(objetivo)).ToList();
            }

            if (!string.IsNullOrEmpty(canal))
            {
                filteredList = filteredList.Where(p => p.campana.Canal.Equals(canal)).ToList();
            }

            if (!string.IsNullOrEmpty(buyer))
            {
                filteredList = filteredList.Where(p => p.buyer.Nombre.Equals(buyer)).ToList();
            }

            ViewBag.objetivo = objetivo;
            ViewBag.canal = canal;
            ViewBag.buyer = buyer;

            return View(filteredList);
        }


        // GET: Campanas/Create
        public async Task<IActionResult> Create()
        {
            campanaView modelo = new campanaView();
            List<Buyer> buyersList = await _context.Buyer.ToListAsync();
            
            modelo.opNomBuy = new List<SelectListItem>();
            foreach (Buyer b in buyersList)
            {
                modelo.opNomBuy.Add(new SelectListItem { Value = b.Id.ToString(), Text = b.Nombre });
            }
            
   
            return View(modelo);
        }

        // POST: Campanas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Objetivo,Canal,Presupuesto,Frecuencia,BuyerID,UserID,fechaInicio, fechaVencimiento")] Campana campana)
        {
            if (ModelState.IsValid)
            {
                var usuarioAct = HttpContext.User.Identity.Name;
                List<Usuario> us = await _context.Usuario.ToListAsync();
                Usuario usl = us.Where(p => p.Nombre.Equals(usuarioAct)).ToList().First();
                campana.UserID =usl.Id;
                _context.Add(campana);
                await _context.SaveChangesAsync();
                string url = Url.Action("Create", "KPIXCampanas", campana);
                return Redirect(url);

            }
            return View(campana);
        }
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(id);
        }
        // GET: Campanas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Campana == null)
            {
                return NotFound();
            }

            var campana = await _context.Campana
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campana == null)
            {
                return NotFound();
            }

            return View(campana);
        }

        // POST: Campanas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campana == null)
            {
                return Problem("Entity set 'MarketingCOREContext.Campana'  is null.");
            }
            var campana = await _context.Campana.FindAsync(id);
            if (campana != null)
            {
                List<KPIXCampana> valor=await _context.KPIXCampana.ToListAsync();
                List<KPIXCampana> limpiar= valor.Where(x => x.CampanaID == campana.Id).ToList();
                _context.Campana.Remove(campana);
                foreach(KPIXCampana a in limpiar)
                {
                    _context.KPIXCampana.Remove(a);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampanaExists(int id)
        {
          return _context.Campana.Any(e => e.Id == id);
        }
    }
}
