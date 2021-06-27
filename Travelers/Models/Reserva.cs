using System;
using System.Collections.Generic;

#nullable disable

namespace Travelers.Models
{
    public partial class Reserva
    {
        public int IdReserva { get; set; }
        public int IdCliente { get; set; }
        public int IdViaje { get; set; }
        public int IdMedioPago { get; set; }
        public DateTime FechaReserva { get; set; }
        public decimal CostoTotal { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual MedioPago IdMedioPagoNavigation { get; set; }
        public virtual Viaje IdViajeNavigation { get; set; }
    }
}
