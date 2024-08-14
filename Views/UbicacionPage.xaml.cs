using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace TimeApp.Views;

public partial class UbicacionPage : ContentPage
{
    readonly UbicacionViewModel locationModel;

    public UbicacionPage(UbicacionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = locationModel = viewModel;

#if WINDOWS
		// Note that the map control is not supported on Windows.
		// For more details, see https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/map?view=net-maui-7.0
		// For a possible workaround, see https://github.com/CommunityToolkit/Maui/issues/605
		Content = new Label() { Text = "Windows does not have a map control. 😢" };
#endif
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Waiting.IsRunning = aiLayout.IsVisible = true;

        await locationModel.LoadDataAsync();

        if (locationModel.Locations.Count > 0)
        {
            foreach(var location in locationModel.Locations)
            {
                Ubicacion.Pins.Add(new Pin
                {
                    Address = location.Direccion,
                    Label = location.Nombre,
                    Location = new Location(location.Ubicacion.Latitud, location.Ubicacion.Longitud, location.Ubicacion.Altitud)
                });
                var area = new Circle
                {
                    StrokeColor = Color.FromArgb("#1BA1E2"),
                    StrokeWidth = 2,
                    FillColor = Color.FromArgb("#881BA1E2"),
                    Center = new Location(location.Ubicacion.Latitud, location.Ubicacion.Longitud, location.Ubicacion.Altitud),
                    Radius = new Distance(location.Radio)
                };
                Ubicacion.MapElements.Add(area);
            }
            var pos = new Location(locationModel.Locations[0].Ubicacion.Latitud, locationModel.Locations[0].Ubicacion.Longitud, locationModel.Locations[0].Ubicacion.Altitud);
            var span = new MapSpan(pos, 0.01, 0.01);
            Ubicacion.MoveToRegion(span);
        }

        Waiting.IsRunning = aiLayout.IsVisible = false;
    }
}
