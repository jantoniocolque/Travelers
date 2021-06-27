using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travelers.Models;

namespace Travelers.Controllers
{
    public class ViajeController : Controller
    {
        private readonly TRAVELERSContext _context;

        public ViajeController(TRAVELERSContext context)
        {
            _context = context;
        }

        // GET: Viaje
        public async Task<IActionResult> Index()
        {
            var tRAVELERSContext = _context.Viajes.Include(v => v.IdDestinoNavigation);
            return View(await tRAVELERSContext.ToListAsync());
        }

        // GET: Viaje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viaje = await _context.Viajes
                .Include(v => v.IdDestinoNavigation)
                .FirstOrDefaultAsync(m => m.IdViaje == id);
            if (viaje == null)
            {
                return NotFound();
            }

            return View(viaje);
        }

        // GET: Viaje/Create
        public IActionResult Create()
        {
            ViewData["IdDestino"] = new SelectList(_context.Destinos, "IdDestino", "Descripcion");
            return View();
        }

        // POST: Viaje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdViaje,CapacidadMax,Precio,Aerolinas,IdDestino")] Viaje viaje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDestino"] = new SelectList(_context.Destinos, "IdDestino", "Descripcion", viaje.IdDestino);
            return View(viaje);
        }

        // GET: Viaje/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viaje = await _context.Viajes.FindAsync(id);
            if (viaje == null)
            {
                return NotFound();
            }
            ViewData["IdDestino"] = new SelectList(_context.Destinos, "IdDestino", "Descripcion", viaje.IdDestino);
            return View(viaje);
        }

        // POST: Viaje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdViaje,CapacidadMax,Precio,Aerolinas,IdDestino")] Viaje viaje)
        {
            if (id != viaje.IdViaje)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViajeExists(viaje.IdViaje))
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
            ViewData["IdDestino"] = new SelectList(_context.Destinos, "IdDestino", "Descripcion", viaje.IdDestino);
            return View(viaje);
        }

        // GET: Viaje/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viaje = await _context.Viajes
                .Include(v => v.IdDestinoNavigation)
                .FirstOrDefaultAsync(m => m.IdViaje == id);
            if (viaje == null)
            {
                return NotFound();
            }

            return View(viaje);
        }

        // POST: Viaje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var viaje = await _context.Viajes.FindAsync(id);
            _context.Viajes.Remove(viaje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViajeExists(int id)
        {
            return _context.Viajes.Any(e => e.IdViaje == id);
        }
    }
}
