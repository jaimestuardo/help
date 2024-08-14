using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Helpers
{
    public class AttTypeColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OutTemplate { get; set; }
        public DataTemplate InTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return (item as HistoryItem).Direccion == Enums.TipoMovimientoEnum.Entrada ? InTemplate : OutTemplate;
        }
    }
}
