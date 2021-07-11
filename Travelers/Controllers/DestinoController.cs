using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travelers.Clases;
using Travelers.Models;

namespace Travelers.Controllers
{
    public class DestinoController : Controller
    {
        private readonly TRAVELERSContext _context;

        public DestinoController(TRAVELERSContext context)
        {
            _context = context;
        }

        // GET: Destino
        public async Task<IActionResult> Index()
        {
            return View(await _context.Destinos.ToListAsync());
        }

        // GET: Destino/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destino = await _context.Destinos
                .FirstOrDefaultAsync(m => m.IdDestino == id);
            if (destino == null)
            {
                return NotFound();
            }

            return View(destino);
        }

        // GET: Destino/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DestinoCLS destino)
        {
            int cantVeces = 0;
            cantVeces = _context.Destinos.Where(d =>
            d.NombrePais.ToUpper().Trim() == destino.nombrePais.ToUpper().Trim()).Where(d =>
            d.NombreProvincia.ToUpper().Trim() == destino.nombreProvincia.ToUpper().Trim()).Count();
            if (!ModelState.IsValid || cantVeces >= 1)
            {
                if (cantVeces >= 1)
                {
                    destino.mensajeError = "El destino ya existe";
                }
                return View(destino);
            }
            else
            {
                Destino miDestino = new Destino();
                miDestino.NombrePais = destino.nombrePais;
                miDestino.NombreProvincia = destino.nombreProvincia;
                miDestino.Descripcion = destino.descripcion;
                _context.Destinos.Add(miDestino);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(DestinoCLS destino) 
        {
            int cantVeces = 0;
            cantVeces = _context.Destinos.Where(d =>
            d.NombrePais.ToUpper().Trim() == destino.nombrePais.ToUpper().Trim() &&
            d.NombreProvincia.ToUpper().Trim() == destino.nombreProvincia.ToUpper().Trim() && 
            d.IdDestino != destino.idDestino).Count();

            if (!ModelState.IsValid || cantVeces >= 1)
            {
                destino.mensajeError = "El destino ya existe";
                return View(destino);
            }
            else
            {
                Destino miDestino = _context.Destinos.Where(d => d.IdDestino == destino.idDestino).First();
                miDestino.NombrePais = destino.nombrePais;
                miDestino.NombreProvincia = destino.nombreProvincia;
                miDestino.Descripcion = destino.descripcion;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Destino/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DestinoCLS miDestino = new DestinoCLS();
            miDestino = (from destino in _context.Destinos
                         where destino.IdDestino == id
                         select new DestinoCLS
                         {
                             idDestino = destino.IdDestino,
                             nombrePais = destino.NombrePais,
                             nombreProvincia = destino.NombreProvincia,
                             descripcion = destino.Descripcion,
                         }).First();
            
            return View(miDestino);
        }

        // GET: Destino/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destino = await _context.Destinos
                .FirstOrDefaultAsync(m => m.IdDestino == id);
            if (destino == null)
            {
                return NotFound();
            }

            return View(destino);
        }

        // POST: Destino/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            _context.Destinos.Remove(destino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinoExists(int id)
        {
            return _context.Destinos.Any(e => e.IdDestino == id);
        }
    }
}
