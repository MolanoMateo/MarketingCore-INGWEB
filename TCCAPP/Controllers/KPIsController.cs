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
    [Authorize(Roles = "ADMIN")]
    public class KPIsController : Controller
    {
        private readonly MarketingCOREContext _context;

        public KPIsController(MarketingCOREContext context)
        {
            _context = context;
        }

        // GET: KPIs
        public async Task<IActionResult> Index()
        {
              return View(await _context.KPI.ToListAsync());
        }

        // GET: KPIs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KPI == null)
            {
                return NotFound();
            }

            var kPI = await _context.KPI
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kPI == null)
            {
                return NotFound();
            }

            return View(kPI);
        }

        // GET: KPIs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KPIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion")] KPI kPI)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kPI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kPI);
        }

        // GET: KPIs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KPI == null)
            {
                return NotFound();
            }

            var kPI = await _context.KPI.FindAsync(id);
            if (kPI == null)
            {
                return NotFound();
            }
            return View(kPI);
        }

        // POST: KPIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] KPI kPI)
        {
            if (id != kPI.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kPI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KPIExists(kPI.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kPI);
        }

        // GET: KPIs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KPI == null)
            {
                return NotFound();
            }

            var kPI = await _context.KPI
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kPI == null)
            {
                return NotFound();
            }

            return View(kPI);
        }

        // POST: KPIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KPI == null)
            {
                return Problem("Entity set 'MarketingCOREContext.KPI'  is null.");
            }
            var kPI = await _context.KPI.FindAsync(id);
            if (kPI != null)
            {
                _context.KPI.Remove(kPI);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KPIExists(int id)
        {
          return _context.KPI.Any(e => e.Id == id);
        }
    }
}
