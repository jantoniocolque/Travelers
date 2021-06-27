﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travelers.Models;

namespace Travelers.Controllers
{
    public class ReservaController : Controller
    {
        private readonly TRAVELERSContext _context;

        public ReservaController(TRAVELERSContext context)
        {
            _context = context;
        }

        // GET: Reserva
        public async Task<IActionResult> Index()
        {
            var tRAVELERSContext = _context.Reservas.Include(r => r.IdClienteNavigation).Include(r => r.IdMedioPagoNavigation).Include(r => r.IdViajeNavigation);
            return View(await tRAVELERSContext.ToListAsync());
        }

        // GET: Reserva/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdClienteNavigation)
                .Include(r => r.IdMedioPagoNavigation)
                .Include(r => r.IdViajeNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reserva/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Apellido");
            ViewData["IdMedioPago"] = new SelectList(_context.MedioPagos, "IdMedioPago", "Tipo");
            ViewData["IdViaje"] = new SelectList(_context.Viajes, "IdViaje", "Aerolinas");
            return View();
        }

        // POST: Reserva/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReserva,IdCliente,IdViaje,IdMedioPago,FechaReserva,CostoTotal")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Apellido", reserva.IdCliente);
            ViewData["IdMedioPago"] = new SelectList(_context.MedioPagos, "IdMedioPago", "Tipo", reserva.IdMedioPago);
            ViewData["IdViaje"] = new SelectList(_context.Viajes, "IdViaje", "Aerolinas", reserva.IdViaje);
            return View(reserva);
        }

        // GET: Reserva/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Apellido", reserva.IdCliente);
            ViewData["IdMedioPago"] = new SelectList(_context.MedioPagos, "IdMedioPago", "Tipo", reserva.IdMedioPago);
            ViewData["IdViaje"] = new SelectList(_context.Viajes, "IdViaje", "Aerolinas", reserva.IdViaje);
            return View(reserva);
        }

        // POST: Reserva/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReserva,IdCliente,IdViaje,IdMedioPago,FechaReserva,CostoTotal")] Reserva reserva)
        {
            if (id != reserva.IdReserva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.IdReserva))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Apellido", reserva.IdCliente);
            ViewData["IdMedioPago"] = new SelectList(_context.MedioPagos, "IdMedioPago", "Tipo", reserva.IdMedioPago);
            ViewData["IdViaje"] = new SelectList(_context.Viajes, "IdViaje", "Aerolinas", reserva.IdViaje);
            return View(reserva);
        }

        // GET: Reserva/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdClienteNavigation)
                .Include(r => r.IdMedioPagoNavigation)
                .Include(r => r.IdViajeNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.IdReserva == id);
        }
    }
}
