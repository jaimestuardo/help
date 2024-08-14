using Microsoft.Extensions.Configuration;
using TimeApp.Controls.Wizard.Steps;

namespace TimeApp.Controls.Wizard;

public partial class WizardStepsControl : ContentView
{
    private bool isProcessing;
    private int currentStep;
    private bool showFlyout;

    public WizardStepsControl()
	{
		InitializeComponent();
    }

    public bool GoBack()
    {
        if (currentStep == 0)
        {
            return false;
        }

        currentStep--;
        ShowHideSteps();
        return true;
    }


    public void SetCurrentStep(int step, bool initial)
    {
        currentStep = step;
        showFlyout = !initial;
        ShowHideSteps();
    }

    async void OnNextClicked(object sender, EventArgs args)
    {
        if (!isProcessing)
        {
            if (Step3.IsVisible)
            {
                if (await Step3.SetPassword((EnrolamientoViewModel)BindingContext))
                    await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
            }
            else
            {
                bool forward = currentStep == 0;
                switch (currentStep)
                {
                    case 1:
                        forward = await Step1.LoadWorkerAsync((EnrolamientoViewModel)BindingContext);
                        break;
                    case 2:
                        Step2.SetSelectedCompany((EnrolamientoViewModel)BindingContext);
                        forward = true;
                        break;
                }

                if (forward)
                {
                    currentStep++;
                    ShowHideSteps();
                }
            }
        }
    }

    void OnPreviousClicked(object sender, EventArgs args)
    {
        GoBack();
    }

    async void ShowHideSteps()
    {
        Welcome.IsVisible = currentStep == 0;
        Step1.IsVisible = currentStep == 1;
        Step2.IsVisible = currentStep == 2;
        Step3.IsVisible = currentStep == 3;

        Shell.Current.FlyoutBehavior = showFlyout && Welcome.IsVisible ? FlyoutBehavior.Flyout : FlyoutBehavior.Disabled;

        ButtonPrevious.IsVisible = !Welcome.IsVisible;
        ButtonNext.Text = Step3.IsVisible ? "Finalizar" : "Siguiente";
        GlyphNext.Glyph = Step3.IsVisible ? Helpers.FontAwesomeIcons.ThumbsUp : Helpers.FontAwesomeIcons.Next;

        switch(currentStep)
        {
            case 0:
                await Welcome.ProcessDataAsync((EnrolamientoViewModel)BindingContext);
                break;
            case 1:
                await SetupWizardStep1View.ProcessDataAsync((EnrolamientoViewModel)BindingContext);
                await Step1.Initialize();
                break;
            case 2:
                await Step2.ProcessDataAsync((EnrolamientoViewModel)BindingContext);
                break;
            case 3:
                await SetupWizardStep3View.ProcessDataAsync((EnrolamientoViewModel)BindingContext);
                Step3.Initialize();
                break;
        }
    }

    private void StartProcessing(object sender, EventArgs e)
    {
        isProcessing = true;
        Waiting.IsRunning = aiLayout.IsVisible = true;
    }

    private void EndProcessing(object sender, EventArgs e)
    {
        isProcessing = false;
        Waiting.IsRunning = aiLayout.IsVisible = false;
    }
}