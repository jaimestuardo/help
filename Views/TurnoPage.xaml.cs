namespace TimeApp.Views;

public partial class TurnoPage : ContentPage
{
    readonly TurnoViewModel shiftModel;

    public TurnoPage(TurnoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = shiftModel = viewModel;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Waiting.IsRunning = aiLayout.IsVisible = true;

        await shiftModel.LoadDataAsync();

        Waiting.IsRunning = aiLayout.IsVisible = false;
    }
}
