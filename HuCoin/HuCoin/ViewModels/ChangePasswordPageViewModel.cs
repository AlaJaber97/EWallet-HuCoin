using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HuCoin.ViewModels
{
    public class ChangePasswordPageViewModel : ViewModels.BaseViewModel
    {
        public BLL.Models.ChangePassword ChangePassword { get; set; }
        public string ConfirmPassword { get; set; }
        public ChangePasswordPageViewModel()
        {
            SubmitNewPasswordCommand = new Command(() => SubmitNewPassword().ConfigureAwait(false));
        }
        private async Task SubmitNewPassword()
        {
            if(!string.IsNullOrWhiteSpace(ChangePassword.CurrentPassword) && string.IsNullOrWhiteSpace(ChangePassword.NewPassword)
               && ConfirmPassword.Equals(ChangePassword.NewPassword))
            {
                using var loadingview = new Components.LoadingView();
                using var httpClient = new HttpClient(); 
                httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
                ChangePassword.UserName = AppStatic.User?.UserName;
                var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/changepassword", ChangePassword);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Opration Successfulley", "The password has been changed successfully", "Ok").ConfigureAwait(false);
                    CloseCurrentPage();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
                }
            }
            else
            {
                await DisplayAlert("Required Field", "Please, don't leave empty fields and must new password field matching with confirm password field", "Ok").ConfigureAwait(false);
            }
        }
        private async Task SingIn()
        {
            if (!string.IsNullOrWhiteSpace(ChangePassword.CurrentPassword) &&
                !string.IsNullOrWhiteSpace(ChangePassword.NewPassword) &&
                ChangePassword.NewPassword == ConfirmPassword)
            {
                using var loadingview = new Components.LoadingView();
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/account/changepassword", ChangePassword);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Opration Successfulley", "The password has been reset successfully", "Ok").ConfigureAwait(false);
                    CloseCurrentPage();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
                }
            }
            else
            {
                await DisplayAlert("Required Field", "Please, don't leave empty fields", "Ok").ConfigureAwait(false);
            }
        }

    }
}
