using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.ViewModels
{
    public partial class EnrolamientoViewModel(EnrollmentDataService service) : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<CompanyItem> companies;

        [ObservableProperty]
        WorkerItem worker;

        [ObservableProperty]
        bool toCreate;

        public async Task LoadWorkerBySsn(string ssn)
        {
            Worker = await service.GetWorkerBySsn(ssn);
        }

        public async Task LoadCompaniesAsync(int personaId)
        {
            Companies = new ObservableCollection<CompanyItem>(await service.GetCompaniesByPersonId(personaId));
        }

        public async Task<bool> SetPassword(string email, string password)
        {
            return await service.SetPasswordByPersonId(Worker.PersonaId, email, password, Worker.EmpresaId, ToCreate);
        }

        public async Task<bool> SetPicture(byte[] picture, string extension)
        {
            return await service.SetPictureByPersonId(Worker.PersonaId, picture, extension);
        }
    }
}
