using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class CashOutServicePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand CashOutServiceCommand { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public CashOutServicePageViewModel()
        {
            CashOutServiceCommand = new Command(CashOutService);
        }
        private async Task LoadBalance()
        {
            Balance = await GetBalanceUser();
            OnPropertyChanged(nameof(Balance));
        }
        private void CashOutService()
        {
            OpenPage(new Views.VerficationPinPage());
        }
        private async Task CashOutBalance()
        {
            try
            {
                using var loadingview = new Components.LoadingView();
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
                var transaction = new BLL.Models.TransactionClient
                {
                    SenderPhoneNumber = AppStatic.User.PhoneNumber,
                    RecipientPhoneNumber = "0",
                    Amount= Amount,
                    Fees = 0
                };
                var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/blockchain/cashout", null);
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
