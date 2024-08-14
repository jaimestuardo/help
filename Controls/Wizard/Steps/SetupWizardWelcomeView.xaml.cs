namespace TimeApp.Controls.Wizard.Steps;

public partial class SetupWizardWelcomeView : ContentView
{
	public SetupWizardWelcomeView()
	{
		InitializeComponent();
	}

    public event EventHandler StartProcessing;
    public event EventHandler EndProcessing;

    public Task ProcessDataAsync(EnrolamientoViewModel viewModel)
    {
        OnProcessing(true);
        OnProcessing(false);
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
}