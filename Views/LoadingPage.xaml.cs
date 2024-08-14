using TimeApp.Helpers;

namespace TimeApp.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            //Preferences.Default.Set("enrolled", false);
            if (await IsAuthenticated())
            {
                await HeaderUtil.AddUserInfoAsync();
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            }
            else if (IsEnrolled())
            {
                await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
            }
            else
                await Shell.Current.GoToAsync($"///{nameof(EnrolamientoPage)}?initial=1");
            base.OnNavigatedTo(args);
        }

        private static async Task<bool> IsAuthenticated()
        {
            return await Helpers.LoginData.IsAuthenticated();
        }

        private static bool IsEnrolled()
        {
            bool enrolled = Preferences.Default.Get("enrolled", false);
            return enrolled;
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.Quit();
            return true;
        }
    }
}
