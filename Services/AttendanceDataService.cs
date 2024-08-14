using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeApp.Enums;

namespace TimeApp.Services
{
    public class AttendanceDataService(RestUtilityService api)
    {
        public async Task<IEnumerable<AttendanceItem>> GetItems()
        {
            var workerId = await Helpers.LoginData.GetWorkerId();
            var today = DateTime.Today;
            var result = (await api.GetDataAsync<List<AttendanceItem>>($"/api/TimeAttendance/Asistencia/GetListByWorkerId/{workerId}?fechaInicio={today:yyyy-MM-dd}&fechaTermino={today:yyyy-MM-dd}")).OrderByDescending(c => c.Orden);

            return result;

            /*
            var result = new List<AttendanceItem>
            {
                new AttendanceItem
                {
                    Orden = 1,
                    TurnoActual = new ShiftItem
                    {
                        Inicio = DateTime.Today.AddHours(8),
                        Fin = DateTime.Today.AddHours(19),
                        Tipo = Enums.TipoTurnoEnum.Fijo
                    },
                    HoraLlegada = DateTime.Today.AddHours(7).AddMinutes(50)
                }
            }.OrderBy(a => a.Orden);

            return result;
            */
        }

        public async Task<bool> CheckInOut(string type, DateTime fechaHora, string equipo, double geoLatitud, double geoLongitud, double geoAltitud, byte[] picture, string extension)
        {
            var workerId = await Helpers.LoginData.GetWorkerId();
            var result = await api.PostDataAsync<bool>
            (
                $"/api/TimeAttendance/Marcacion/AddByWorkerId/{workerId}", 
                new 
                {
                    equipo = new
                    {
                        serial = equipo
                    },
                    verificacion = (int)TipoVerificacionEnum.Automatico,
                    direccion = type == "IN" ? (int)TipoMovimientoEnum.Entrada : (int)TipoMovimientoEnum.Salida,
                    fechaHora,
                    fuente = TipoFuenteEnum.MobileApp,
                    ubicacion = new
                    {
                        Latitud = geoLatitud,
                        Longitud = geoLongitud,
                        Altitud = geoAltitud
                    },
                    foto = new
                    {
                        Base64 = Convert.ToBase64String(picture),
                        Extension = extension
                    }
                }
            );

            return result;
        }
    }
}
