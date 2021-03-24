using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ICommand UpdateProfileCommand { get; private set; }
        public ProfilePageViewModel()
        {
            UpdateProfileCommand = new Command(OnUpdateProfile);
        }

        public void OnUpdateProfile()
        {

        }
    }
}
