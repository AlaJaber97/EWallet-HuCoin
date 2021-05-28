using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;
using localizer = HuCoin.Utils.LocalizationResourceManager;
namespace HuCoin.ViewModels
{
    public class HistoryPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand RefreshTransactionsCommand { get; set; }
        public ICommand OpenPopUpDetailCommand { get; set; }
        public bool IsRefreshing { get; set; }
        public List<BLL.Models.TransactionClient> ListOfTransaction { get; set; }
        public HistoryPageViewModel()
        {
            OpenPopUpDetailCommand = new Command<BLL.Models.TransactionClient>(OpenPopUpDetail);
            RefreshTransactionsCommand = new Command(() =>
            {
                using var loadingview = new Components.LoadingView();
                LoadTransactions().ConfigureAwait(false);
            });
            LoadTransactions().ConfigureAwait(false);
        }
        private async Task LoadTransactions()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
                var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/ewallet/get/transactions?PhoneNumber={HttpUtility.UrlEncode(AppStatic.User.PhoneNumber)}",null);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ListOfTransaction = System.Text.Json.JsonSerializer.Deserialize<List<BLL.Models.TransactionClient>>(json);
                    OnPropertyChanged(nameof(ListOfTransaction));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert(localizer.Instance["AnErrorOccurred"], error,localizer.Instance["Ok"]).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(localizer.Instance["AnErrorOccurred"], ex.ToString(), localizer.Instance["Ok"]).ConfigureAwait(false);
            }
            IsRefreshing = false;
            OnPropertyChanged(nameof(IsRefreshing));
        }
        private void OpenPopUpDetail(BLL.Models.TransactionClient transaction) 
                        => OpenModal(new Views.TransactionDetailsView(transaction));
    }
}