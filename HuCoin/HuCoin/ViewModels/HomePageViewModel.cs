using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class HomePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand OpenCashOutServiceCommand { get; set; }
        public ICommand OpenCashInServiceCommand { get; set; }
        public ICommand OpenTransferServiceCommand { get; set; }
        public ICommand OpenBeneficiariesCommand { get; set; }
        public BLL.Models.User User { get; set; }
        public decimal Balance { get; set; }
        public HomePageViewModel()
        {
            OpenCashOutServiceCommand = new Command(OpenCashOutService);
            OpenCashInServiceCommand = new Command(OpenCashInService);
            OpenTransferServiceCommand = new Command(OpenTransferService);
            OpenBeneficiariesCommand = new Command(OpenBeneficiaries);
            LoadUserProfile().ConfigureAwait(false);
            MessagingCenter.Subscribe<ProfilePageViewModel>(this, "NotifyProfileInfromationUpdated", (sender) => LoadUserProfile().ConfigureAwait(false));
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
                AppStatic.Wallet = User.Wallet;
                OnPropertyChanged(nameof(User));

                Balance = await GetBalanceUser();
                OnPropertyChanged(nameof(Balance));
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
            }
        }
        private void OpenCashOutService() => OpenPage(new Views.CashOutServicePage());
        private void OpenCashInService() => OpenPage(new Views.CashInServicePage());
        private void OpenTransferService() => OpenPage(new Views.TransferServicePage());
        private void OpenBeneficiaries() => OpenPage(new Views.BeneficiariesPage());
    }
}