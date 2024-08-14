using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Models
{
#pragma warning disable IDE1006 // Estilos de nombres
    public class ErrorItem
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
        public Dictionary<string, string[]> errors { get; set; }
    }
#pragma warning restore IDE1006 // Estilos de nombres
}
