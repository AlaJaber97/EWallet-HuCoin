using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class VerficationPinPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand OpenForgetPinCodePageCommand { get; set; }
        public VerficationPinPageViewModel()
        {
            OpenForgetPinCodePageCommand = new Command(OpenForgetPinCodePage);
        }
        private void OpenForgetPinCodePage() => OpenPage(new Views.ForgetPinCodePage());
    }
}
