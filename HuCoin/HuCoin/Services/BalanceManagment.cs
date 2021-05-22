using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HuCoin.Services
{
    public class BalanceManagment : NotifyPropertyChanged
    {
        private static BalanceManagment _Instance;
        public static BalanceManagment Instance => _Instance ??= new BalanceManagment();
        public decimal Balance { get; set; }
        private BalanceManagment() { }
        public async Task<decimal> ReLoadBalance()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
            var response = await httpClient.PostAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/ewallet/get/balance?ownerAddress={AppStatic.Wallet.Credential.PublicKey}", null);
            var result = await response.Content.ReadAsStringAsync();
            if (decimal.TryParse(result, out decimal amount))
                Balance = amount;
            else
                Balance = 0;
            return Balance;
        }
    }
}
