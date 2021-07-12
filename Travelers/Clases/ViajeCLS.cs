using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travelers.Clases
{
    public class ViajeCLS
    {
        public int idViaje { get; set; }
        [Display(Name="Capacidad Max")]
        public int capacidadMax { get; set; }
        [Display(Name = "Precio")]
        public decimal precio { get; set; }
        [Display(Name = "Aerolineas")]
        public string aerolinas { get; set; }
        [Display(Name = "Nombre Pais")]
        public string nombrePais { get; set; }
        [Display(Name = "Nombre Provincia")]
        public string nombreProvincia { get; set; }
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
        public string mensajeError { get; set; }
    }
}
