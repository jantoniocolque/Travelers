using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travelers.Clases;
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
            List<ViajeCLS> listaViajes = new List<ViajeCLS>();
            listaViajes = (from viaje in _context.Viajes
                           join destino in _context.Destinos
                           on viaje.IdDestino equals destino.IdDestino
                           select new ViajeCLS
                           {
                               idViaje = viaje.IdViaje,
                               capacidadMax = viaje.CapacidadMax,
                               precio = viaje.Precio,
                               aerolinas = viaje.Aerolinas,
                               nombrePais = destino.NombrePais,
                               nombreProvincia = destino.NombreProvincia,
                               descripcion = destino.Descripcion,
                           }).ToList();
            //.Include(v => v.IdDestinoNavigation);
            ViewBag.listaDestinos = listaDestinos();
            return View(listaViajes);
        }
        public List<SelectListItem> listaDestinos()
        {
            List<SelectListItem> listaDestinos = new List<SelectListItem>();
            listaDestinos = (from destino in _context.Destinos
                             group destino by new { destino.NombrePais } into g
                             select new SelectListItem
                             {
                                 Text = g.Key.NombrePais,
                                 Value = g.Key.NombrePais,
                             }).ToList();
            listaDestinos.Insert(0, new SelectListItem { Text = "--Seleccionar--", Value = "" });
            return  listaDestinos;
        }
        public IActionResult Filter(ViajeCLS viajeBuscado) {
            List<ViajeCLS> viajesPorNombre = new List<ViajeCLS>();
            if (viajeBuscado.nombrePais != null || viajeBuscado.nombrePais!="") {
                viajesPorNombre = (from viaje in _context.Viajes
                                   join destino in _context.Destinos
                                   on viaje.IdDestino equals destino.IdDestino
                                   where destino.NombrePais.Contains(viajeBuscado.nombrePais)
                                   select new ViajeCLS
                                   {
                                       idViaje = viaje.IdViaje,
                                       capacidadMax = viaje.CapacidadMax,
                                       precio = viaje.Precio,
                                       aerolinas = viaje.Aerolinas,
                                       nombrePais = destino.NombrePais,
                                       nombreProvincia = destino.NombreProvincia,
                                       descripcion = destino.Descripcion,
                                   }).ToList();
            }
            ViewBag.listaDestinos = listaDestinos();
            return View("Index",viajesPorNombre);
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
            ViewData["IdDestino"] = new SelectList(_context.Destinos, "IdDestino", "NombrePais");
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
            ViewData["IdDestino"] = new SelectList(_context.Destinos, "IdDestino", "NombrePais", viaje.IdDestino);
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
