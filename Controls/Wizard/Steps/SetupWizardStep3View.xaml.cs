using CommunityToolkit.Maui.Behaviors;
using TimeApp.Helpers;

namespace TimeApp.Controls.Wizard.Steps;

public partial class SetupWizardStep3View : ContentView
{
    private string _pictureExtension = string.Empty;
    private byte[] _pictureContent = null;
    private readonly PhotoService _photoService;

	public SetupWizardStep3View()
	{
		InitializeComponent();

        _photoService = ServiceHelper.GetService<PhotoService>();
    }

    public void Initialize()
    {
        Email.Focus();
        Email.Text = Password.Text = ConfirmPassword.Text = string.Empty;
    }

    public event EventHandler StartProcessing;
    public event EventHandler EndProcessing;

    public static Task ProcessDataAsync(EnrolamientoViewModel _)
    {
        return Task.CompletedTask;
    }

    public async Task<bool> SetPassword(EnrolamientoViewModel viewModel)
    {
        if (myPicture.ImageSource == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Debes tomarte una selfie para poder completar el registro.", "Aceptar");
            return false;
        }

        foreach (var behavior in Email.Behaviors)
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

        foreach (var behavior in Password.Behaviors)
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

        string email = Email.Text.Trim();
        string password = Password.Text.Trim();
        string confirmPassword = ConfirmPassword.Text.Trim();

        if (password != confirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "La contraseña con su confirmación no coinciden.", "Aceptar");
            return false;
        }

        try
        {
            OnProcessing(true);
            var result = await viewModel.SetPassword(email, password);
            if (result)
            {
                if (_pictureContent != null)
                    result = await viewModel.SetPicture(_pictureContent, _pictureExtension);

                Preferences.Default.Set("enrolled", true);
                await Application.Current.MainPage.DisplayAlert("Éxito", "El enrolamiento fue exitoso. Antes de poder ingresar al sistema, tienes que revisar tu correo electrónico y hacer clic en el link que se te envió para confirmarlo.\n\nSi no te llega, por favor, revisa tu carpeta de SPAM.", "Aceptar");
            }
            else
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo completar el enrolamiento. Comunícate con tu administrador.", "Aceptar");
            return result;
        }
        finally
        {
            OnProcessing(false);
        }
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

    private async void PictureButton_Clicked(object sender, EventArgs e)
    {
        var item = await _photoService.TakePhotoAsync();
        if (item is not null)
        {
            _pictureExtension = item.Extension;
            _pictureContent = await item.GetBytesAsync();
            myPicture.ImageSource = ImageSource.FromStream(() => item.Stream);
        }
    }
}