using Microsoft.Extensions.Configuration;

namespace TimeApp.Views;

[QueryProperty("Initial", "initial")]
public partial class EnrolamientoPage : ContentPage
{
    private bool isInitial;

    public int Initial
    {
        set
        {
            isInitial = value == 1;
            //Shell.Current.FlyoutBehavior = isInitial ? FlyoutBehavior.Disabled : FlyoutBehavior.Flyout;
            Wizard.SetCurrentStep(0, isInitial);
            ((EnrolamientoViewModel)BindingContext).ToCreate = isInitial;
        }
        get
        {
            return isInitial ? 1 : 0;
        }
    }

    public EnrolamientoPage(EnrolamientoViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Wizard.SetCurrentStep(0, false);
        base.OnNavigatedTo(args);
    }
}