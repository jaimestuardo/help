namespace TimeApp.ViewModels;

public partial class UbicacionViewModel : BaseViewModel
{
    readonly LocationDataService dataService;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    ObservableCollection<LocationItem> locations;

    public UbicacionViewModel(LocationDataService service)
    {
        dataService = service;
    }

    [RelayCommand]
    private async Task OnRefreshing()
    {
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        if (!IsRefreshing)
        {
            try
            {
                IsRefreshing = true;
                Locations = new ObservableCollection<LocationItem>(await dataService.GetItems());
            }
            finally
            { 
                IsRefreshing = false; 
            }
        }
    }
}
