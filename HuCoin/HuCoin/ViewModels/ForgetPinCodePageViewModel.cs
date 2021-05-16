using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class ForgetPinCodePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand SubmitPhoneNumebrCommand { get; set; }
        public ICommand VerifyPhoneNumberCommand { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationID { get; set; }
        public string VerificationCode { get; set; }
        public ForgetPinCodePageViewModel()
        {
            SubmitPhoneNumebrCommand = new Command(() => SubmitPhoneNumebr().ConfigureAwait(false));
            VerifyPhoneNumberCommand = new Command(() => Verification().ConfigureAwait(false));

        }
        private async Task SubmitPhoneNumebr()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PhoneNumber) || !PhoneNumber.StartsWith("+9627"))
                {
                    string ErrorMessage = "Please fill in the following fields:";
                    ErrorMessage += "\nmust phone number start with +9627 and not be empty field";
                    await DisplayAlert("Fields Required", ErrorMessage, "Okay").ConfigureAwait(false);
                    return;
                }
                using var loadingview = new Components.LoadingView();
                OpenPage(new Views.VerificationPhoneCodePage() { BindingContext = this });
                var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(PhoneNumber);
                VerificationID = verificationResult.VerificationId;
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), "Okay");
            }
        }
        private async Task Verification()
        {
            try
            {
                if (string.IsNullOrEmpty(VerificationCode)) return;

                using var loadingview = new Components.LoadingView();
                var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(VerificationID, VerificationCode);
                if (credential != null)
                {
                    OpenPage(new Views.CreatePinCodePage());
                }
                else
                {
                    await DisplayAlert("An error occurred", "Unfortunatley, we were unable to verify your phone number, please try agin\nIf the problem recures, please contact support", "Ok").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), "Okay").ConfigureAwait(false);
            }
        }

    }
}
