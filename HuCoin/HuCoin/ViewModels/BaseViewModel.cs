using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BaseViewModel: INotifyPropertyChanged
    {
        public ICommand BackPageCommand { get; set; }
        public ICommand BackModalCommand { get; set; }
        public BaseViewModel()
        {
            BackPageCommand = new Command(CloseCurrentPage);
            BackModalCommand = new Command(CloseCurrentModal);
        }
        
        public void OpenPageAsMainPage(Page page)
        {
            App.Current.MainPage = new NavigationPage(page);
        }

        public void OpenPage(Page page)
        {
            App.Current.MainPage.Navigation.PushAsync(page);
        }
        public void OpenModal(Page page)
        {
            App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public void CloseCurrentPage()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
        public void CloseCurrentModal()
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        public async Task DisplayAlert(string title, string message, string cancelButton)
        {
            if(Xamarin.Essentials.MainThread.IsMainThread)
                await App.Current.MainPage.DisplayAlert(title, message, cancelButton);
            else 
                await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => App.Current.MainPage.DisplayAlert(title, message, cancelButton));

        }
        public async Task<bool> DisplayAlert(string title, string message, string okButton, string cancelButton)
        {
            bool isAproved;
            if (Xamarin.Essentials.MainThread.IsMainThread)
                isAproved = await App.Current.MainPage.DisplayAlert(title, message, okButton, cancelButton);
            else
                isAproved = await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => App.Current.MainPage.DisplayAlert(title, message, okButton, cancelButton));
            return isAproved;
        }

        public async Task<decimal> GetBalanceUser()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/ewallet/get/balance",AppStatic.Wallet.Credential.PublicKey);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                if (decimal.TryParse(result, out decimal amount))
                    return amount;
            }
            else
            {
                await DisplayAlert(string.Empty, response.ToString(), "Okat");
            }
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propretyname)
        {
            if(PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propretyname));
        }
    }
}
