namespace TimeApp.Controls.Wizard.Steps;

public partial class SetupWizardStep2View : ContentView
{
    public SetupWizardStep2View()
	{
		InitializeComponent();
    }

    public event EventHandler StartProcessing;
    public event EventHandler EndProcessing;

    public void SetSelectedCompany(EnrolamientoViewModel viewModel)
    {
        viewModel.Worker.EmpresaId = ((CompanyItem)Companies.SelectedItem).Id;
    }

    public async Task ProcessDataAsync(EnrolamientoViewModel viewModel)
    {
        OnProcessing(true);
        if (viewModel.Worker is not null)
            await viewModel.LoadCompaniesAsync(viewModel.Worker.PersonaId);
        OnProcessing(false); 
    }

    void OnEmpresaRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton button = sender as RadioButton;
        if (e.Value)
            Companies.SelectedItem = button.BindingContext as CompanyItem;
        else
            Companies.SelectedItem = null;
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