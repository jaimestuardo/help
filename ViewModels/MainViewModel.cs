using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace TimeApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    readonly AttendanceDataService dataService;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    DateTime currentDate;

    [ObservableProperty]
    ObservableCollection<AttendanceItem> attendances;

    public AttendanceItem FirstAttendance { 
        get
        {
            if (Attendances == null || Attendances.Count == 0)
            {
                return null;
            }

            return Attendances.LastOrDefault();
        }
    }

    public AttendanceItem LastAttendance
    {
        get
        {
            if (Attendances == null || Attendances.Count == 0)
            {
                return null;
            }

            return Attendances.FirstOrDefault();
        }
    }

    public MainViewModel(AttendanceDataService service)
    {
        dataService = service;

        CurrentDate = DateTime.Today;

        var timer = new System.Timers.Timer(60000);
        timer.Elapsed += Timer_Elapsed;
        timer.Start();
    }

    [RelayCommand]
    private async Task OnRefreshing()
    {
        IsRefreshing = true;

        try
        {
            await LoadDataAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    public async Task LoadDataAsync()
    {
        Attendances = new ObservableCollection<AttendanceItem>(await dataService.GetItems());
    }

    public async Task<bool> CheckInOutAsync(string type, DateTime fechaHora, string equipo, double geoLatitud, double geoLongitud, double geoAltitud, byte[] pictureContent, string pictureExtension)
    {
        return await dataService.CheckInOut(type, fechaHora, equipo, geoLatitud, geoLongitud, geoAltitud, pictureContent, pictureExtension);
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        CurrentDate = DateTime.Today;
        OnPropertyChanged(nameof(CurrentDate));
    }
}
