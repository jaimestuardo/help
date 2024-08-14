using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using UraniumUI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using TimeApp.Helpers;

namespace TimeApp;

public static partial class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("TimeApp.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
        builder.Configuration.AddConfiguration(config);

        builder
			.UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
            });

        builder.Services
            .AddSingleton<ShellViewModel>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<MainPage>()
            .AddTransient<HistoryDataService>()
            .AddTransient<ShiftDataService>()
            .AddTransient<AttendanceDataService>()
            .AddTransient<LocationDataService>()
            .AddTransient<EnrollmentDataService>()
            .AddTransient<PhotoService>()
            .AddTransient(factory =>
            {
                var rest = new RestUtilityService(factory.GetService<IConfiguration>());
                rest.Exception += Api_Exception;
                return rest;
            })
            .AddSingleton<AsistenciaHistoricaViewModel>()
            .AddSingleton<AsistenciaHistoricaPage>()
            .AddSingleton<TurnoViewModel>()
            .AddSingleton<TurnoPage>()
            .AddSingleton<UbicacionViewModel>()
            .AddSingleton<UbicacionPage>()
            .AddSingleton<EnrolamientoViewModel>()
            .AddSingleton<EnrolamientoPage>()
            .AddSingleton<LoadingPage>()
            .AddSingleton<LoginPage>();

        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("es-CL");

        var app = builder.Build();

        ServiceHelper.Initialize(app.Services);

        return app;
	}

    static async private void Api_Exception(object sender, ThreadExceptionEventArgs e)
    {
        string message = e.Exception.Message;
        Regex regex = ExtractTitle();
        var match = regex.Match(message);
        await Application.Current.MainPage.DisplayAlert(match.Success ? match.Value : "Error", match.Success ? message.Replace("[" + match.Value + "]", string.Empty).Trim() : message, "Aceptar");
    }

    [GeneratedRegex("(?<=^\\[).+?(?=\\])")]
    private static partial Regex ExtractTitle();
}