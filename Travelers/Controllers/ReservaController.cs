using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travelers.Models;

namespace Travelers.Controllers
{
    public class ReservaController : Controller
    {
        private readonly TRAVELERSContext _context;
        private static List<Reserva> lista = new List<Reserva>();

        public ReservaController(TRAVELERSContext context)
        {
            _context = context;
        }

        // GET: Reserva
        public async Task<IActionResult> Index()
        {
            var tRAVELERSContext = _context.Reservas.Include(r => r.IdClienteNavigation).Include(r => r.IdMedioPagoNavigation).Include(r => r.IdViajeNavigation);
            lista = await tRAVELERSContext.ToListAsync();
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

        public FileResult exportar(string[] nombrePropiedades)
        {
            byte[] buffer = exportarPDFDatos(nombrePropiedades, lista);
            return File(buffer, "application/pdf");
        }
        public byte[] exportarPDFDatos<T>(string[] nombrePropiedades, List<T> lista)
        {
            Dictionary<string, string> diccionario = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>().ToDictionary(p => p.Name, p => p.DisplayName);
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                using (var pdfDoc = new PdfDocument(writer))
                {
                    Document doc = new Document(pdfDoc);
                    Paragraph c1 = new Paragraph("Reporte en PDF");
                    c1.SetFontSize(20);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    doc.Add(c1);
                    Table table = new Table(nombrePropiedades.Length);
                    Cell celda;
                    for (int i = 0; i < nombrePropiedades.Length; i++)
                    {
                        celda = new Cell();
                        celda.Add(new Paragraph(diccionario[nombrePropiedades[i]]));
                        table.AddHeaderCell(celda);
                    }
                        foreach (string propiedad in nombrePropiedades)
                        {
                            celda = new Cell();
                            celda.Add(new Paragraph(lista[0].GetType().GetProperty(propiedad).GetValue(lista[0]).ToString()));
                            table.AddCell(celda);
                        }
                    doc.Add(table);
                    doc.Close();
                    writer.Close();

                }
                return ms.ToArray();
            }
        }
    }
}
