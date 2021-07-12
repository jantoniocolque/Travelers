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
            listaDestinos.Insert(0, new SelectListItem { Text = "--Seleccionar pais--", Value = "" });
            return  listaDestinos;
        }
        public List<SelectListItem> listaProvincias()
        {
            List<SelectListItem> listaProvincias = new List<SelectListItem>();
            listaProvincias = (from destino in _context.Destinos
                             group destino by new { destino.NombreProvincia,destino.NombrePais } into g
                             select new SelectListItem
                             {
                                 Text = g.Key.NombreProvincia,
                                 Value = g.Key.NombrePais,
                             }).ToList();
            listaProvincias.Insert(0, new SelectListItem { Text = "--Seleccionar provincia--", Value = "" });
            return listaProvincias;
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
            ViewBag.listaProvincias = listaProvincias();
            ViewBag.listaDestinos = listaDestinos();
            return View();
        }

        // POST: Viaje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViajeCLS viaje)
        {
            int cantVeces = 0;
            cantVeces = _context.Destinos.Where(d =>
            d.NombrePais.ToUpper().Trim() == viaje.nombrePais.ToUpper().Trim()).Where(d =>
            d.NombreProvincia.ToUpper().Trim() == viaje.nombreProvincia.ToUpper().Trim()).Count();

            if (!ModelState.IsValid || cantVeces >= 1)
            {
                if (cantVeces >= 1)
                {
                    viaje.mensajeError = "El destino ya existe";
                }
                ViewBag.listaProvincias = listaProvincias();
                ViewBag.listaDestinos = listaDestinos();
                return View(viaje);
            }
            else
            {
                if(viaje.descripcion != null)
                {
                    Destino miDestino = new Destino();
                    miDestino.NombrePais = viaje.nombrePais;
                    miDestino.NombreProvincia = viaje.nombreProvincia;
                    miDestino.Descripcion = viaje.descripcion;
                    _context.Destinos.Add(miDestino);
                    _context.SaveChanges();
                }
                

                var nuevoDestino = _context.Destinos.Where(d =>
                                    d.NombrePais.ToUpper().Trim() == viaje.nombrePais.ToUpper().Trim()).Where(d =>
                                    d.NombreProvincia.ToUpper().Trim() == viaje.nombreProvincia.ToUpper().Trim()).First();

                Viaje miViaje = new Viaje();
                miViaje.CapacidadMax = viaje.capacidadMax;
                miViaje.Precio = viaje.precio;
                miViaje.Aerolinas = viaje.aerolinas;
                miViaje.IdDestino = nuevoDestino.IdDestino;
                _context.Add(miViaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
