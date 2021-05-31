using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using localizer = HuCoin.Utils.LocalizationResourceManager;
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
                if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length != 13  /*!PhoneNumber.StartsWith("+9627")*/)
                {
                    string ErrorMessage = localizer.Instance["FillInError"];
                    ErrorMessage += "\n"+localizer.Instance["MustNumber"];
                    await DisplayAlert(localizer.Instance["RequiredField"], ErrorMessage, localizer.Instance["Ok"]).ConfigureAwait(false);
                    return;
                }
                using var loadingview = new Components.LoadingView();
                OpenPage(new Views.VerificationPhoneCodePage() { BindingContext = this });
                var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(PhoneNumber);
                VerificationID = verificationResult.VerificationId;
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), localizer.Instance["Ok"]);
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
                    await DisplayAlert(localizer.Instance["AnErrorOccurred"], localizer.Instance["VerificationError"], localizer.Instance["Ok"]).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), localizer.Instance["Ok"]).ConfigureAwait(false);
            }
        }

    }
}
