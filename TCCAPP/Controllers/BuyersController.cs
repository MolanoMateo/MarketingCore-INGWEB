using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CORE;
using MarketingCORE.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MarketingCORE.Controllers
{
    [Authorize(Roles = "USER")]
    public class BuyersController : Controller
    {
        private readonly MarketingCOREContext _context;

        public BuyersController(MarketingCOREContext context)
        {
            _context = context;
        }

        // GET: Buyers
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Buyer.ToListAsync());
        //}

        public async Task<IActionResult> Index(int edad, string genero, float ingresos, string geo)
        {
            List<Buyer> productList = await _context.Buyer.ToListAsync();

            List<Buyer> filteredList = productList;

            if (!string.IsNullOrEmpty(edad.ToString()) && edad != 0)
            {
                filteredList = filteredList.Where(p => p.Edad == edad).ToList();
            }

            if (!string.IsNullOrEmpty(genero))
            {
                filteredList = filteredList.Where(p => p.Genero.Contains(genero)).ToList();
            }

            if (!string.IsNullOrEmpty(ingresos.ToString()) && ingresos != 0)
            {
                filteredList = filteredList.Where(p => p.Ingresos == ingresos).ToList();
            }
            if (!string.IsNullOrEmpty(geo))
            {
                filteredList = filteredList.Where(p => p.Geografia.Contains(geo)).ToList();
            }

            ViewBag.edad = edad;
            ViewBag.genero = genero;
            ViewBag.ingresos = ingresos;
            ViewBag.geo = geo;

            return View(filteredList);
        }


        // GET: Buyers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buyers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Edad,Genero,Ingresos,Intereses,Frustraciones,Geografia")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buyer);
        }


        private bool BuyerExists(int id)
        {
            return _context.Buyer.Any(e => e.Id == id);
        }
    }
}
