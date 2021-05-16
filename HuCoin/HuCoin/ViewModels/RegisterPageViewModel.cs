using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        public ICommand SingUpCommand { get; set; }
        public ICommand VerifyPhoneNumberCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public BLL.Models.User User { get; set; }
        public string VerificationID { get; set; }
        public string VerificationCode { get; set; }
        public RegisterPageViewModel()
        {
            User = new BLL.Models.User();
            SingUpCommand = new Command(()=> SingUp().ConfigureAwait(false));
            VerifyPhoneNumberCommand = new Command(()=> Verification().ConfigureAwait(false));
            PickPhotoCommand = new Command(() => PickPhotoAsync().ConfigureAwait(false));
        }

        private async Task SingUp()
        {
            try
            {
                if (!IsValidUser(User)) return; 
                using var loadingview = new Components.LoadingView();
                OpenPage(new Views.VerificationPhoneCodePage() { BindingContext = this });
                var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(User.PhoneNumber);
                VerificationID = verificationResult.VerificationId;
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), "Okay");
            }
        }
        private async Task PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                using var SourceStream = await photo.OpenReadAsync();
                User.Image = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(User.Image, 0, (int)SourceStream.Length);
                OnPropertyChanged(nameof(User));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }
        }
        private bool IsValidUser(BLL.Models.User user)
        {
            string ErrorMessage = null;
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.SecondName) ||
                string.IsNullOrWhiteSpace(user.FamilyName) ||
                string.IsNullOrWhiteSpace(user.PhoneNumber) || !user.PhoneNumber.StartsWith("+9627") ||
                user.UniversityID.ToString().Length != 7 ||
                string.IsNullOrWhiteSpace(user.Password))
                ErrorMessage= "Please fill in the following fields:";

            if (string.IsNullOrWhiteSpace(user.FirstName))
                ErrorMessage += "\n• First Name";
            if (string.IsNullOrWhiteSpace(user.SecondName))
                ErrorMessage += "\n• Second Name";
            if (string.IsNullOrWhiteSpace(user.FamilyName))
                ErrorMessage += "\n• Family Name";
            if (string.IsNullOrWhiteSpace(user.PhoneNumber) || !user.PhoneNumber.StartsWith("+9627"))
                ErrorMessage += "\n• Phone Number must start with +9627 and not be empty field";
            if (user.UniversityID.ToString().Length != 7)
                ErrorMessage += "\n• University ID";
            if (string.IsNullOrWhiteSpace(user.Password))
                ErrorMessage += "\n• Password";

            if(ErrorMessage != null)
            {
                DisplayAlert("Fields Required", ErrorMessage, "Okay").ConfigureAwait(false);
                return false;
            }
            return true;
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
                    User.UserName = User.PhoneNumber;
                    User.PhoneNumberConfirmed = true; 
                    using var httpClient = new HttpClient();
                    var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/register", User);
                    if (response.IsSuccessStatusCode)
                    {
                        AppStatic.Token = await response.Content.ReadAsStringAsync();
                        OpenPage(new Views.CreatePinCodePage());
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), "Okay").ConfigureAwait(false);
            }
        }
    }
}
