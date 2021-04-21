using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        public ICommand SingUpCommand { get; set; }
        public ICommand VerifyPhoneNumberCommand { get; set; }
        public BLL.Models.User User { get; set; }
        public string VerificationID { get; set; }
        public string VerificationCode { get; set; }
        public RegisterPageViewModel()
        {
            User = new BLL.Models.User();
            SingUpCommand = new Command(()=> SingUp().ConfigureAwait(false));
            VerifyPhoneNumberCommand = new Command(Verification);
        }
        private async Task SingUp()
        {
            try
            {
                OpenPage(new Views.VerificationPhoneCodePage());
                //send phone number to api
                if (!string.IsNullOrEmpty(User.PhoneNumber))
                {
                    var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(User.PhoneNumber);
                    VerificationID = verificationResult.VerificationId;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), "Okay");
            }
        }
        private void Verification()
        {
            try
            {
                //send verification id with fake verification code
                var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(VerificationID, VerificationCode);

                if (credential != null)
                {
                    OpenPage(new Views.MainPage());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(string.Empty, ex.ToString(), "Okay");
            }
        }
    }
}
