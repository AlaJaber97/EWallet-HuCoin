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
        public decimal Balance { get; set; }
        public int CardNumber { get; set; }
        public CashInServicePageViewModel()
        {
            RechargeBalanceCommand = new Command(RechargeBalanceService);
            Balance= Services.BalanceManagment.Instance.Balance;
            MessagingCenter.Subscribe<VerficationPinPageViewModel, bool>(this, "VerficationPinCode", (sender, isSuccessed) =>
            {
                if (isSuccessed) RechargeBalance().ConfigureAwait(false);
            });

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
                var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/ewallet/recharge/balance?number_card={CardNumber}&ownerAddress={AppStatic.Wallet.Credential.PublicKey}", null);
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    await DisplayAlert(string.Empty, message, "Ok").ConfigureAwait(false);
                    await Services.BalanceManagment.Instance.ReLoadBalance();
                    Balance = Services.BalanceManagment.Instance.Balance;
                    OnPropertyChanged(nameof(Balance));
                    MessagingCenter.Send(this, "UpdateBalance");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("An error occurred", ex.ToString(), "Ok").ConfigureAwait(false);
            }
        }
    }
}
