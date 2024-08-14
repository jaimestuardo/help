using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Models
{
    public class LocationItem
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public Position Ubicacion { get; set; }
        public int Radio { get; set; } // metros
    }

    public class Position
    {
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Altitud { get; set; }
    }
}
