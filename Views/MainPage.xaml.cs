using Microsoft.Maui.Graphics;
using SkiaSharp.Extended.UI.Controls;
using System.Runtime.Intrinsics.X86;

namespace TimeApp.Views;

public partial class MainPage : ContentPage
{
    readonly MainViewModel mainModel;
    private bool _isMarking = false;
    private readonly PhotoService _photoService;

    public MainPage(MainViewModel viewModel, PhotoService photoService)
	{
		InitializeComponent();

        BindingContext = mainModel = viewModel;
        _photoService = photoService;

        Shell.SetTabBarIsVisible(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await LocationDataService.GetCurrentLocationAsync();

        await UpdateData();

        Waiting.IsRunning = aiLayout.IsVisible = false;
    }

    private async void EntradaButton_Clicked(object sender, EventArgs e)
    {
        if (!_isMarking)
        {
            _isMarking = true;
            var item = await _photoService.TakePhotoAsync();
            if (item is not null)
            {
                var location = await LocationDataService.GetCurrentLocationAsync();
                if (location == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo obtener la ubicación del equipo, lo cual es necesario para poder marcar.\n\nConverse con el administrador de R.R.H.H.", "Aceptar");
                }
                else
                {
                    Waiting.IsRunning = aiLayout.IsVisible = true;
                    if (await mainModel.CheckInOutAsync("IN", DateTime.Now, string.Empty, location.Latitude, location.Longitude, location.Altitude ?? 0, await item.GetBytesAsync(), item.Extension))
                    {
                        await UpdateData();
                        await FeedbackCheckResult(true);
                    }
                    else
                    {
                        await FeedbackCheckResult(false);
                        // await Application.Current.MainPage.DisplayAlert("Error", "No se pudo realizar la marca de entrada.\n\nConverse con el administrador de R.R.H.H.", "Aceptar");
                    }
                }
            }
            _isMarking = false;
        }
    }

    private async void SalidaButton_Clicked(object sender, EventArgs e)
    {
        if (!_isMarking)
        {
            _isMarking = true;
            var item = await _photoService.TakePhotoAsync();
            if (item is not null)
            {
                var location = await LocationDataService.GetCurrentLocationAsync();
                if (location == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo obtener la ubicación del equipo, lo cual es necesario para poder marcar.\n\nConverse con el administrador de R.R.H.H.", "Aceptar");
                }
                else
                {
                    Waiting.IsRunning = aiLayout.IsVisible = true;
                    if (await mainModel.CheckInOutAsync("OUT", DateTime.Now, string.Empty, location.Latitude, location.Longitude, location.Altitude ?? 0, await item.GetBytesAsync(), item.Extension))
                    {
                        await UpdateData();
                        await FeedbackCheckResult(true);
                    }
                    else
                    {
                        await FeedbackCheckResult(false);
                        // await Application.Current.MainPage.DisplayAlert("Error", "No se pudo realizar la marca de salida.\n\nConverse con el administrador de R.R.H.H.", "Aceptar");
                    }
                }
            }
            _isMarking = false;
        }
    }

    private async Task UpdateData()
    {
        await mainModel.LoadDataAsync();

        if (mainModel is not null)
        {
            if (mainModel.LastAttendance is not null)
            {
                shiftView.StartTime = mainModel.LastAttendance.TurnoActual.Inicio;
                shiftView.EndTime = mainModel.LastAttendance.TurnoActual.Fin;
            }
        }

        Saludo.Text = $"¡ Hola, {await Helpers.LoginData.GetFirstName()} !";
        Empresa.Text = await Helpers.LoginData.GetCompany();
    }

    private async Task FeedbackCheckResult(bool success)
    {
        SKFileLottieImageSource source = new()
        {
            File = success ? "check_animated.json" : "failed_animated.json"
        };
        SKLottieView animated = new()
        {
            Source = source,
            RepeatCount = 0,
            HeightRequest = 200,
            WidthRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };

        Waiting.IsRunning = aiLayout.IsVisible = false;

        checkResult.Add(animated);

        await Task.Delay(6000);

        checkResult.Remove(animated);
    }
}
