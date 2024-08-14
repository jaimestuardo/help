using CommunityToolkit.Maui.Behaviors;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using TimeApp.Helpers;

namespace TimeApp.Controls.Wizard.Steps;

public partial class SetupWizardStep1View : ContentView
{
    public SetupWizardStep1View()
    {
        InitializeComponent();
    }

    public event EventHandler StartProcessing;
    public event EventHandler EndProcessing;

    public async Task Initialize()
    {
        string currentSsn = await Helpers.LoginData.GetSSN();
        Ssn.Text = string.IsNullOrEmpty(currentSsn) ? string.Empty : currentSsn;
        Ssn.Focus();
        Ssn.IsReadOnly = Ssn.Text != string.Empty;
    }

    public async Task<bool> LoadWorkerAsync(EnrolamientoViewModel viewModel)
    {
        foreach (var behavior in Ssn.Behaviors)
        {
            if (behavior is MultiValidationBehavior multi)
            {
                if (multi.IsNotValid)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", (string)multi.Errors.First(), "Aceptar");
                    return false;
                }
            }
        }

        string ssn = Ssn.Text.Trim();
        OnProcessing(true);
        await viewModel.LoadWorkerBySsn(ssn);
        OnProcessing(false);
        if (viewModel.Worker is null)
            await Application.Current.MainPage.DisplayAlert("Alerta", "El RUT no se encuentra en el sistema. Asegúrate que tengas autorización para trabajar de manera remota y con contrato vigente.\n\nConversa con el administrador de R.R.H.H.", "Aceptar");
        else if (!Ssn.IsReadOnly && viewModel.Worker.UsuarioId != 0)   // Ya está enrolado
        {
            await Application.Current.MainPage.DisplayAlert("Alerta", "El RUT ya se encuentra enrolado para marca remota. Serás redirigido a la página de autenticación.", "Aceptar");
            Preferences.Default.Set("enrolled", true);
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
        }

        return viewModel.Worker is not null;
    }

    public static Task ProcessDataAsync(EnrolamientoViewModel viewModel)
    {
        return Task.CompletedTask;
    }

    protected void OnProcessing(bool processing)
    {
        if (processing)
        {
            StartProcessing?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            EndProcessing?.Invoke(this, EventArgs.Empty);
        }
    }

    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text ?? string.Empty;
        Regex reg = NotSsnCharacters();
        ((Entry)sender).Text = reg.Replace(text, string.Empty);
    }

    [GeneratedRegex("[^1-9kK]")]
    private static partial Regex NotSsnCharacters();
}
