using TimeApp.Controls.Wizard.Steps;

namespace TimeApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        //Routing.RegisterRoute(nameof(EnrolamientoPage), typeof(EnrolamientoPage));
        //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        //Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
        //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }

    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        App.ClearUserInformation();

        await Shell.Current.GoToAsync($"///{nameof(LoadingPage)}");

        Shell.Current.FlyoutIsPresented = false;
    }
}