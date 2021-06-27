using System;
using System.Collections.Generic;

#nullable disable

namespace Travelers.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int IdCliente { get; set; }
        public int Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
