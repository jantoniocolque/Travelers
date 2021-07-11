using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travelers.Clases
{
    public class DestinoCLS
    {
        public int idDestino { get; set; }
        [Display(Name = "Nombre Pais")]
        public string nombrePais { get; set; }
        [Display(Name = "Nombre Provincia")]
        public string nombreProvincia { get; set; }
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
        public string mensajeError { get; set; }
    }
}
