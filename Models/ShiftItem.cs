using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeApp.Enums;

namespace TimeApp.Models
{
    public class ShiftItem
    {
        public int Orden { get; set; }
        public DateTime Inicio { get; set; }    // Hora inicio
        public DateTime Fin { get; set; }       // Hora fin

        public override string ToString()
        {
            return $"{Inicio:HH:mm} - {Fin:HH:mm}";
        }
    }
}
