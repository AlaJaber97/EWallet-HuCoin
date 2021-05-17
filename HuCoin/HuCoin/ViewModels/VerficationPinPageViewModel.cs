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
            PinCodeCompletedCommand = new Command<string>((pincode) => PinCodeCompleted(pincode).ConfigureAwait(false));
            OpenForgetPinCodePageCommand = new Command(OpenForgetPinCodePage);
        }
        private void OpenForgetPinCodePage() => OpenPage(new Views.ForgetPinCodePage());
        private async Task PinCodeCompleted(string pincode)
        {
            var original_pincode = await Xamarin.Essentials.SecureStorage.GetAsync(AppStatic.HuCoinPinCodeKey);
            if (original_pincode == pincode)
            {
                MessagingCenter.Send(this, "VerficationPinCode", true);
                CloseCurrentPage();
            }
            else
                await DisplayAlert("Worng Pin Code", "The code you entered appears to be incorrect, please try again", "Okay");
        }
    }
}
