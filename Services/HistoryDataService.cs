namespace TimeApp.Services;

public class HistoryDataService
{
    readonly RestUtilityService _api;

    public HistoryDataService(RestUtilityService api)
    {
        _api = api;
    }

    public async Task<IEnumerable<HistoryItem>> GetItems()
	{
        var workerId = await Helpers.LoginData.GetWorkerId();
        var result = (await _api.GetDataAsync<List<HistoryItem>>($"/api/TimeAttendance/Marcacion/GetListByWorkerId/{workerId}")).OrderByDescending(c => c.FechaHora);

        return result.Take(10);
        /*
		await Task.Delay(1000); // Artifical delay to give the impression of work

		var result = new List<HistoryItem>();

		for (var i = 0; i < 40; i++)
		{
			result.Add(new HistoryItem
			{
				TransactionTime = DateTime.Today.AddDays(-i).AddHours(8),
				TransactionType = i % 2 == 0 ? Enums.TipoMovimientoEnum.Entrada : Enums.TipoMovimientoEnum.Salida
			});
		}
		*/
	}
}
