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
using localizer = HuCoin.Utils.LocalizationResourceManager;

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
                await DisplayAlert(string.Empty, ex.ToString(), localizer.Instance["Ok"]);
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
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PhoneNumber) || !user.PhoneNumber.StartsWith("+9627") ||
                user.UniversityID.ToString().Length != 7 ||
                string.IsNullOrWhiteSpace(user.Password))
                ErrorMessage= localizer.Instance["FillInError"];

            if (string.IsNullOrWhiteSpace(user.FirstName))
                ErrorMessage += "\n•" localizer.Instance["FirstName"];
            if (string.IsNullOrWhiteSpace(user.SecondName))
                ErrorMessage += "\n•" localizer.Instance["SecondName"];
            if (string.IsNullOrWhiteSpace(user.FamilyName))
                ErrorMessage += "\n•" localizer.Instance["FamilyName"];
            if (string.IsNullOrWhiteSpace(user.Email))
                ErrorMessage += "\n•"localizer.Instance["Email"];
            if (string.IsNullOrWhiteSpace(user.PhoneNumber) || !user.PhoneNumber.StartsWith("+9627"))
                ErrorMessage += "\n•" localizer.Instance["MobileNumber"];
            if (user.UniversityID.ToString().Length != 7)
                ErrorMessage += "\n•"localizer.Instance["UniversityId"];
            if (string.IsNullOrWhiteSpace(user.Password))
                ErrorMessage += "\n•" localizer.Instance["Password"];

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
                        await DisplayAlert(localizer.Instance["RequiredField"], error, localizer.Instance["Ok"]).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Empty, ex.ToString(), localizer.Instance["Ok"]).ConfigureAwait(false);
            }
        }
    }
}
