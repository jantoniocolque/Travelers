using System;
using System.Collections.Generic;

#nullable disable

namespace Travelers.Models
{
    public partial class MedioPago
    {
        public MedioPago()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int IdMedioPago { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
