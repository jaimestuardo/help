namespace TimeApp.Views;

public partial class AsistenciaHistoricaPage : ContentPage
{
    readonly AsistenciaHistoricaViewModel historyModel;

	public AsistenciaHistoricaPage(AsistenciaHistoricaViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = historyModel = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

        Waiting.IsRunning = aiLayout.IsVisible = true;

        await historyModel.LoadDataAsync();

        Waiting.IsRunning = aiLayout.IsVisible = false;
    }
}
