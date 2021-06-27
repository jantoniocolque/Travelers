using System;
using System.Collections.Generic;

#nullable disable

namespace Travelers.Models
{
    public partial class Viaje
    {
        public Viaje()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int IdViaje { get; set; }
        public int CapacidadMax { get; set; }
        public decimal Precio { get; set; }
        public string Aerolinas { get; set; }
        public int IdDestino { get; set; }

        public virtual Destino IdDestinoNavigation { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
