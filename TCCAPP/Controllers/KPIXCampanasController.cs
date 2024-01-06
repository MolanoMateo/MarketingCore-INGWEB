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
    public class KPIXCampanasController : Controller
    {
        private readonly MarketingCOREContext _context;

        public KPIXCampanasController(MarketingCOREContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "w")]
        // GET: KPIXCampanas
        public async Task<IActionResult> Index()
        {
              return View(await _context.KPIXCampana.ToListAsync());
        }
        [Authorize(Roles = "USER")]
        // GET: KPIXCampanas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KPIXCampana == null || _context.Campana == null)
            {
                return NotFound();
            }

            var kPIXCampana = await _context.KPIXCampana
                .FirstOrDefaultAsync(m => m.CampanaID == id);
            if (kPIXCampana == null)
            {
                return NotFound();
            }
            ResultadosCamp nc= new ResultadosCamp();
            nc.kPIXCampana=kPIXCampana;
            var camp = await _context.Campana.Where(p => p.Id==id).FirstOrDefaultAsync();
            nc.campana=camp;
            var kpi = await _context.KPI.Where(p => p.Id == kPIXCampana.KPIID).FirstOrDefaultAsync();
            nc.kpi = kpi;
            return View(nc);
        }
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Create(Campana campana)
        {
            ResultadosCamp modelo = new ResultadosCamp();
            modelo.listkpi = new List<SelectListItem>();
            List<KPI> lista = await _context.KPI.ToListAsync();
            foreach (KPI b in lista)
            {
                modelo.listkpi.Add(new SelectListItem { Value = b.Id.ToString(), Text = b.Nombre });
            }
            modelo.campana = campana;
            return View(modelo);
        }
        [Authorize(Roles = "USER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KPIID,CampanaID,Resultado")] KPIXCampana kPIXCampana)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kPIXCampana);
                await _context.SaveChangesAsync();
                Campana camp = await _context.Campana.Where(p => p.Id == kPIXCampana.CampanaID).FirstOrDefaultAsync();
                string url = Url.Action("Create", "KPIXCampanas", camp);
                return Redirect(url);
            }
            return View(kPIXCampana);
        }


        private bool KPIXCampanaExists(int id)
        {
          return _context.KPIXCampana.Any(e => e.Id == id);
        }
    }
}
