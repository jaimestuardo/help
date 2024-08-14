using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Services
{
    public class EnrollmentDataService(RestUtilityService rest)
    {

        /*
        async private void Api_Exception(object sender, ThreadExceptionEventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Error", e.Exception.Message, "Aceptar");
        }
        */

        public async Task<WorkerItem> GetWorkerBySsn(string ssn)
        {
            var worker = await rest.GetDataAsync<WorkerItem>($"/api/TimeAttendance/Trabajador/GetBySsn/0/{ssn}");
            bool permitido = worker is not null 
                && worker.PermiteRemoto 
                && (worker.FechaInicioContrato is null || worker.FechaInicioContrato <= DateTime.Today)
                && (worker.FechaTerminoContrato is null || worker.FechaTerminoContrato >= DateTime.Today);
            return permitido ? worker : null;
        }

        public async Task<IEnumerable<CompanyItem>> GetCompaniesByPersonId(int personaId)
        {
            var result = (await rest.GetDataAsync<List<CompanyItem>>($"/api/Personal/Persona/GetCompaniesByPersonId/{personaId}")).OrderBy(c => c.Nombre);

            if (result.Any())
                result.First().Seleccionado = true;

            return result;
        }

        public async Task<bool> SetPasswordByPersonId(int personId, string email, string password, int empresaId, bool toCreate)
        {
            string apiName = toCreate ? "CreateUserByPersonId" : "UpdateUserByPersonId";
            return await rest.PostDataAsync<bool>($"/api/TimeAttendance/Trabajador/{apiName}/{personId}", new { email, plainTextPassword = password, empresaId });
        }

        public async Task<bool> SetPictureByPersonId(int personId, byte[] picture, string extension)
        {
            return await rest.PostDataAsync<bool>($"/api/Personal/Persona/UploadPictureById/{personId}", new { base64 = Convert.ToBase64String(picture), extension });
        }
    }
}
