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
        public string VerificationID { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public ICommand SubmitPhoneNumberCommand { get; set; }
        public ICommand VerifyPhoneNumberCommand { get; set; }
        public ForgetPinCodePageViewModel()
        {
            SubmitPhoneNumberCommand = new Command(() => SubmitPhoneNumber().ConfigureAwait(false));
            VerifyPhoneNumberCommand = new Command(() => Verification().ConfigureAwait(false));
        }

        private async Task SubmitPhoneNumber()
        {
            OpenPage(new Views.VerificationPhoneCodePage() { BindingContext = this });
            var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(PhoneNumber);
            VerificationID = verificationResult.VerificationId;
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
                    await DisplayAlert(string.Empty, "Unfortunately, we were unable to verify your phone number, please try again\nIf the problem recurs, please contact support", "Okay").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), "Okay").ConfigureAwait(false);
            }
        }

    }
}
