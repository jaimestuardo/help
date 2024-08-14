using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Services
{
    public class LocationDataService
    {
        readonly RestUtilityService _api;

        public LocationDataService(RestUtilityService api)
        {
            _api = api;
        }

        public async Task<IEnumerable<LocationItem>> GetItems()
        {
            var workerId = await Helpers.LoginData.GetWorkerId();
            var result = (await _api.GetDataAsync<List<LocationItem>>($"/api/TimeAttendance/Ubicacion/GetListByWorkerId/{workerId}"));

            return result;
            /*
            await Task.Delay(1000); // Artifical delay to give the impression of work

            var result = new List<LocationItem>()
            {
                new LocationItem
                {
                    Name = "Casa",
                    Address = "Av. Pedro Fontova 6426 casa 136, Huechuraba",
                    Location = new Location(-33.363164, -70.671837),
                    Radius = 100
                }
            };

            return result;
            */
        }

        public static async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                GeolocationRequest request = new (GeolocationAccuracy.High, TimeSpan.FromSeconds(10));

                Location location = await Geolocation.Default.GetLocationAsync(request);// ?? await Geolocation.Default.GetLastKnownLocationAsync();
                return location;
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch
            {
                return null;
            }
        }
    }
}
