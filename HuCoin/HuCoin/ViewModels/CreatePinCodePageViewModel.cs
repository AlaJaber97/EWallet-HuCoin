using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class CreatePinCodePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand PinCodeCompletedCommand { get; set; }
        public CreatePinCodePageViewModel()
        {
            PinCodeCompletedCommand = new Command<string>(PinCodeCompleted);
        }
        private void PinCodeCompleted(string pincode)
        {
            //store pin code on mobile setting/preffrence 
            Xamarin.Essentials.SecureStorage.SetAsync(AppStatic.HuCoinPinCodeKey, pincode);
            //open main page as root page
            OpenPageAsMainPage(new Views.MainPage());
        }
    }
}
