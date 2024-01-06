using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MarketingCORE.Models;
using Microsoft.AspNetCore.Authorization;
using CORE;
using Microsoft.EntityFrameworkCore;
using MarketingCORE.Migrations;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MarketingCORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly MarketingCOREContext _context;

        public HomeController(MarketingCOREContext context)
        {
            _context = context;
        }
		[Authorize(Roles = "ADMIN, USER")]
		public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> LOGIC(string objetivo, float presupuesto, int kpiID, int edad, string genero, float ingresos, string geo)
        {
            kpiID = Mod.ix;
            List<Campana> Lcamp = await _context.Campana.ToListAsync();
            List<KPI> Lkpi = await _context.KPI.ToListAsync();
            List<KPIXCampana> Lres = await _context.KPIXCampana.ToListAsync();
            List<Buyer> Lbuy = await _context.Buyer.ToListAsync();
            List<Logic> lista=new List<Logic>();
            var usuarioAct = HttpContext.User.Identity.Name;
            Usuario u=await _context.Usuario.Where(x=>x.Nombre.Equals(usuarioAct)).FirstOrDefaultAsync();
            List<Campana> us=Lcamp.Where(x=>x.UserID==u.Id).ToList();

            

            foreach (Campana c in us)
            {
                KPIXCampana kx=Lres.Where(x=>x.CampanaID==c.Id).FirstOrDefault();
                Logic l = new Logic();
                
                        if (kx == null)
                        {
                            l = new Logic { campana = c, buyer = Lbuy.Where(x => x.Id == c.BuyerID).FirstOrDefault(), kpi = null, kPIXCampana = null };
                        }
                        else
                        {
                            l = new Logic { campana = c, buyer = Lbuy.Where(x => x.Id == c.BuyerID).FirstOrDefault(), kpi = Lkpi.Where(x => x.Id == kx.KPIID).FirstOrDefault(), kPIXCampana = kx };

                        }
                    
                
                l.listkpi = new List<SelectListItem>();
                List<KPI> listakpi = await _context.KPI.ToListAsync();
                foreach (KPI b in listakpi)
                {
                    l.listkpi.Add(new SelectListItem { Value = b.Id.ToString(), Text = b.Nombre });
                }
                lista.Add(l);
            }

            if (!string.IsNullOrEmpty(objetivo))
            {
                lista = lista.Where(p => p.campana.Objetivo == objetivo).ToList();
            }

            if (!string.IsNullOrEmpty(presupuesto.ToString())&& presupuesto!=0)
            {
                lista = lista.Where(p => p.campana.Presupuesto== presupuesto).ToList();
            }
            //kpi y resultado
            if (!string.IsNullOrEmpty(kpiID.ToString()) && kpiID != 0)
            {
                lista = lista.Where(p => p.kpi.Id == kpiID).ToList();
                List<int> a=new List<int>();
                foreach (Logic i in lista) { a.Add(int.Parse(i.kPIXCampana.Resultado)); }
                if (lista.First().kpi.Id==3)
                {
                    int aux = a.Min();
                    lista = lista.Where(x => x.kPIXCampana.Resultado.Equals(aux.ToString())).ToList();
                }
                else
                {
                    int aux = a.Max();
                    lista = lista.Where(x=>x.kPIXCampana.Resultado.Equals(aux.ToString())).ToList();
                }
                
            }
            if (!string.IsNullOrEmpty(edad.ToString()) && edad != 0)
            {
                lista = lista.Where(p => p.buyer.Edad == edad).ToList();
            }

            if (!string.IsNullOrEmpty(genero))
            {
                lista = lista.Where(p => p.buyer.Genero.Contains(genero)).ToList();
            }

            if (!string.IsNullOrEmpty(ingresos.ToString()) && ingresos != 0)
            {
                lista = lista.Where(p => p.buyer.Ingresos == ingresos).ToList();
            }
            if (!string.IsNullOrEmpty(geo))
            {
                lista = lista.Where(p => p.buyer.Geografia.Contains(geo)).ToList();
            }

            

            ViewBag.objetivo = objetivo;
            ViewBag.presupuesto = presupuesto;
            ViewBag.kpiID = kpiID;
            ViewBag.edad = edad;
            ViewBag.genero = genero;
            ViewBag.ingresos = ingresos;
            ViewBag.geo = geo;

            return View(lista);
        }
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Reporte(string fi, string fv)
        {
            DateTime fiv=DateTime.Now; DateTime fvv = DateTime.Now;
            List<Campana> Lcamp = await _context.Campana.ToListAsync();
            List<KPI> Lkpi = await _context.KPI.ToListAsync();
            List<KPIXCampana> Lres = await _context.KPIXCampana.ToListAsync();
            var usuarioAct = HttpContext.User.Identity.Name;
            Usuario u = await _context.Usuario.Where(x => x.Nombre.Equals(usuarioAct)).FirstOrDefaultAsync();
            List<Campana> us = Lcamp.Where(x => x.UserID == u.Id).ToList();
            List<Reporte> kus=new List<Reporte>();
            foreach (KPIXCampana kx in Lres)
            {
                foreach (Campana ai in us)
                {
                    if (kx.CampanaID == ai.Id)
                    {
                        KPI kk = Lkpi.Where(x =>x.Id==kx.KPIID).First();
                        kus.Add(new Reporte {campana=ai,KPIXCampana=kx, kpi=kk}); 
                    }
                    
                }
            }
            if(!fi.IsNullOrEmpty() && !fv.IsNullOrEmpty())
            {
                string customFormat = "dd-MM-yyyy";
                DateTime fic = DateTime.ParseExact(fi, customFormat, null);
                DateTime fvc = DateTime.ParseExact(fv, customFormat, null);
                fiv = fic;
                    fvv= fvc;
                    kus = kus.Where(x => x.campana.fechaInicio >= fic && x.campana.fechaVencimiento<=fvc).ToList() ;
                
            }
            List <AnalisisReporte> analisisReportes = new List<AnalisisReporte>();
                
            foreach (KPI kpi in Lkpi) {
                analisisReportes.Add(new AnalisisReporte {
                    fechaInicio = fiv,
                    fechaVencimiento = fvv,
                    Resultado = 0,
                    Nombre = kpi.Nombre
                });
            }
            
            foreach (Reporte r in kus) {
                int re = int.Parse(r.KPIXCampana.Resultado);
                string na=r.kpi.Nombre;
                analisisReportes.Where(x => x.Nombre == na).FirstOrDefault().Resultado+=re;
                
            }

            ViewBag.fi = fi;
            ViewBag.fv = fv;

            return View(analisisReportes);
        }
        public IActionResult ViewError()
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