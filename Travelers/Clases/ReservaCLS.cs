using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travelers.Clases
{
    public class ReservaCLS
    {
        public int idCliente { get; set; }
        public int idViaje { get; set; }
        public int idMedioPago { get; set; }
        public DateTime FechaReserva { get; set; }
        public decimal CostoTotal { get; set; }
    }
}
