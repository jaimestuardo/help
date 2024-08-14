using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Models
{
    public class WorkerItem
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public int EmpresaId { get; set; }
        public int UsuarioId { get; set; }  // Si tiene un valor <> 0 es porque ya está enrolado
        public bool PermiteRemoto { get; set; }
        public DateTime? FechaInicioContrato { get; set; }
        public DateTime? FechaTerminoContrato { get; set; }
    }
}
