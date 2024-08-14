using CommunityToolkit.Maui.Core.Extensions;

namespace TimeApp.ViewModels;

public partial class TurnoViewModel : BaseViewModel
{
    readonly ShiftDataService dataService;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    ObservableCollection<ShiftItem> shifts;

    public TurnoViewModel(ShiftDataService service)
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
                Shifts = new ObservableCollection<ShiftItem>(await dataService.GetItems());
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
