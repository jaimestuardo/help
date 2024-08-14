namespace TimeApp.ViewModels;

public partial class AsistenciaHistoricaViewModel : BaseViewModel
{
	readonly HistoryDataService dataService;

	[ObservableProperty]
	bool isRefreshing;

	[ObservableProperty]
	ObservableCollection<HistoryItem> items;

	public AsistenciaHistoricaViewModel(HistoryDataService service)
	{
		dataService = service;
	}

	[RelayCommand]
	private async Task OnRefreshing()
	{
        await LoadDataAsync();
    }

	/*
	[RelayCommand]
	public async Task LoadMore()
	{
		var items = await dataService.GetItems();

		foreach (var item in items)
		{
			Items.Add(item);
		}
	}
	*/

	public async Task LoadDataAsync()
	{
		if (!IsRefreshing)
		{
			try
			{
				IsRefreshing = true;
				Items = new ObservableCollection<HistoryItem>(await dataService.GetItems());
			}
			finally
			{
                IsRefreshing = false;
            }
        }
	}
}
