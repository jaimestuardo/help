using TimeApp.Enums;

namespace TimeApp.Models;

public class HistoryItem
{
	public DateTime FechaHora { get; set; }

	public TipoMovimientoEnum Direccion { get; set; }
}
