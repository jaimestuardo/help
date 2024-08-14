using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeApp.Controls;

namespace TimeApp.Helpers
{
    public class HeaderUtil
    {
        public async static Task AddUserInfoAsync()
        {
            await App.SetUserInformation();
            AppShell.Current.FlyoutHeader = new HeaderControl();
        }
    }
}
