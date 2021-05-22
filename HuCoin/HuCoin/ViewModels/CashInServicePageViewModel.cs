using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class CashInServicePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand RechargeBalanceCommand { get; set; }
        public decimal CardNumber { get; set; }
        public decimal Balance { get; set; }

        public CashInServicePageViewModel()
        {
            RechargeBalanceCommand = new Command(RechargeBalanceService);
            MessagingCenter.Subscribe<VerficationPinPageViewModel, bool>(this, "VerficationPinCode", (sender, isSuccessed) =>
            {
                if (isSuccessed) RechargeBalance().ConfigureAwait(false);
            });
            LoadBalance().ConfigureAwait(false);
        }
        private async Task LoadBalance() 
        {
            Balance = await GetBalanceUser();
            OnPropertyChanged(nameof(Balance));
        }
        private void RechargeBalanceService()
        {
            OpenPage(new Views.VerficationPinPage());
        }
        private async Task RechargeBalance()
        {
            try
            {
                using var loadingview = new Components.LoadingView();
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
                var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/blockchain/recharge/balance?cardnumber={CardNumber}&ownerAddress={AppStatic.Wallet.Credential.PublicKey}", null);
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    await DisplayAlert(string.Empty, message, "Ok").ConfigureAwait(false);
                    await LoadBalance().ConfigureAwait(false);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("An error occurred", ex.ToString(), "Ok").ConfigureAwait(false);
            }
        }

    }
}
