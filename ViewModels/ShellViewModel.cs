﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.ViewModels
{
    public partial class ShellViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                if (_userEmail != value)
                {
                    _userEmail = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        private string _userFullname;
        public string UserFullname
        {
            get { return _userFullname; }
            set
            {
                if (_userFullname != value)
                {
                    _userFullname = value;
                    OnPropertyChanged(nameof(UserFullname));
                }
            }
        }

        private async Task GetUserInformation()
        {
            UserEmail = await Helpers.LoginData.GetEmail();
            UserFullname = await Helpers.LoginData.GetFullName();
        }

        public async Task LoadData()
        {
            await GetUserInformation();
        }
    }
}
