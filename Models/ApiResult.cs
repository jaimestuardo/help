using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Models
{
#pragma warning disable IDE1006 // Estilos de nombres
    public class ApiResult<T> : WebStatus
    {
        public bool success { get; set; }
        public string[] errorList { get; set; }
        public T data { get; set; }
    }
#pragma warning restore IDE1006 // Estilos de nombres
}
