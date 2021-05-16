using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class VerficationPinPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand PinCodeCompletedCommand { get; set; }
        public ICommand OpenForgetPinCodePageCommand { get; set; }
        public VerficationPinPageViewModel()
        {
            PinCodeCompletedCommand = new Command<string>((e)=> PinCodeCompleted(e).ConfigureAwait(false));
            OpenForgetPinCodePageCommand = new Command(OpenForgetPinCodePage);
        }
        private void OpenForgetPinCodePage() => OpenPage(new Views.ForgetPinCodePage());
        private async Task PinCodeCompleted(string pincode)
        {
            //store pin code on mobile setting/preffrence 
            var original_pincode = await Xamarin.Essentials.SecureStorage.GetAsync(AppStatic.HuCoinPinCodeKey);
            //back to opration page
            if (original_pincode != pincode)
                CloseCurrentPage();
            else
                await DisplayAlert("Worng Pin Code", "The code you entered appears to be incorrect, please try again", "Okay");
        }
    }
}
