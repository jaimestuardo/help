using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Services
{
    public class ShiftDataService
    {
        readonly RestUtilityService _api;

        public ShiftDataService(RestUtilityService api)
        {
            _api = api;
        }

        public async Task<IEnumerable<ShiftItem>> GetItems()
        {
            var workerId = await Helpers.LoginData.GetWorkerId();
            var result = (await _api.GetDataAsync<List<ShiftItem>>($"/api/TimeAttendance/Turno/GetListByWorkerId/{workerId}/1")).OrderBy(c => c.Orden);

            return result;

            /*
            await Task.Delay(1000); // Artifical delay to give the impression of work

            var result = new List<ShiftItem>
            {
                new ShiftItem
                {
                    Orden = 1,
                    Inicio = DateTime.Today.AddHours(8),
                    Fin = DateTime.Today.AddHours(19),
                    Tipo = Enums.TipoTurnoEnum.Fijo
                }
            }.OrderBy(t => t.Orden);
            */

            // return result;
        }
    }
}
