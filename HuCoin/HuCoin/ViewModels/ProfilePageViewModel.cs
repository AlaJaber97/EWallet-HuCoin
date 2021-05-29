using System;
using System.Collections.Generic;
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
    public class ProfilePageViewModel : BaseViewModel
    {
        public ICommand UpdateProfileCommand { get; private set; }
        public ICommand PickPhotoCommand { get; set; }
        public BLL.Models.User User { get; set; }
        public ProfilePageViewModel()
        {
            PickPhotoCommand = new Command(()=> PickPhotoAsync().ConfigureAwait(false));
            UpdateProfileCommand = new Command(()=> UpdateUserProfile().ConfigureAwait(false));
            LoadUserProfile().ConfigureAwait(false);
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
            catch(Exception ex)
            {
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }
        }
        private async Task LoadUserProfile()
        {
            using var loadingview = new Components.LoadingView();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
            var response = await httpClient.GetAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/profile");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                User = System.Text.Json.JsonSerializer.Deserialize<BLL.Models.User>(json);
                OnPropertyChanged(nameof(User));
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert(localizer.Instance["AnErrorOccurred"], error, localizer.Instance["Ok"]).ConfigureAwait(false);
            }
        }
        private bool IsValidUser(BLL.Models.User user)
        {
            string ErrorMessage = null;
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.SecondName) ||
                string.IsNullOrWhiteSpace(user.FamilyName))
                ErrorMessage = localizer.Instance["FillInError"];

            if (string.IsNullOrWhiteSpace(user.FirstName))
                ErrorMessage += "\n•" localizer.Instance["FirstName"];
            if (string.IsNullOrWhiteSpace(user.SecondName))
                ErrorMessage += "\n•"localizer.Instance["SecondName"];
            if (string.IsNullOrWhiteSpace(user.FamilyName))
                ErrorMessage += "\n•" localizer.Instance["FamilyName"];

            if (ErrorMessage != null)
            {
                DisplayAlert(localizer.Instance["RequiredField"], ErrorMessage, localizer.Instance["Ok"]).ConfigureAwait(false);
                return false;
            }
            return true;
        }
        private async Task UpdateUserProfile()
        {
            if (!IsValidUser(User)) return; 
            using var loadingview = new Components.LoadingView();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
            var response = await httpClient.PutAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/profile", User);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert(localizer.Instance["UpdateProfileAleart"], localizer.Instance["Ok"]).ConfigureAwait(false);
                MessagingCenter.Send(this, "NotifyProfileInfromationUpdated");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert(localizer.Instance["AnErrorOccurred"], error,  localizer.Instance["Ok"]).ConfigureAwait(false);
            }
        }
    }
}
