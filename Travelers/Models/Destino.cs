using System;
using System.Collections.Generic;

#nullable disable

namespace Travelers.Models
{
    public partial class Destino
    {
        public Destino()
        {
            Viajes = new HashSet<Viaje>();
        }

        public int IdDestino { get; set; }
        public string NombrePais { get; set; }
        public string NombreProvincia { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Viaje> Viajes { get; set; }
    }
}
