using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class TransferServicePageViewModel : ViewModels.BaseViewModel
    {
        public List<HuCoin.Models.Beneficiary> Beneficiaries { get; set; }
        public HuCoin.Models.Beneficiary Beneficiary { get; set; }
        public decimal Amount { get; set; }
        public ICommand AddNewBeneficiaryCommand { get; set; }
        public ICommand TransferServiceCommand { get; set; }
        public decimal Balance { get; set; }

        public TransferServicePageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            TransferServiceCommand = new Command(TransferService);
            LoadBeneficiaries().ConfigureAwait(false);
        }
        private async Task LoadBeneficiaries()
        {
            using var loadingview = new Components.LoadingView();
            using var db = new Data.DbCon();
            Beneficiaries = await db.Beneficiaries.ToListAsync();
            OnPropertyChanged(nameof(Beneficiaries));


            Balance = await GetBalanceUser();
            OnPropertyChanged(nameof(Balance));
        }
        private void AddNewBeneficiary()
        {
            OpenModal(new Views.AddBeneficiaryPage()
            {
                BackgroundColor = Color.FromHex("#AAC4C4C4"),
                Padding = 10
            }); 
            MessagingCenter.Subscribe<AddBeneficiaryPageViewModel>(this, "AddNewBeneficiary", (sender) => LoadBeneficiaries().ConfigureAwait(false));

        }
        private void TransferService()
        {
            OpenPage(new Views.VerficationPinPage());

            MessagingCenter.Subscribe<VerficationPinPageViewModel, bool>(this, "VerficationPinCode", (sender,isSuccessed) => {
                if (isSuccessed) 
                    SendTransaction().ConfigureAwait(false);
            });
        }
        private async Task SendTransaction()
        {
            using var loadingview = new Components.LoadingView();
            var transaction = new BLL.Models.Transaction
            {
                Sender = AppStatic.Wallet.Credential.PublicKey,
                RecipientPhoneNumber = Beneficiary.PhoneNumber,
            Amount = Amount,
        };
            transaction.Signature = BLL.Utils.RSA.SignatureGenerate(AppStatic.Wallet.Credential.PrivateKey, transaction.ToString());


            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AppStatic.GetAuthenticationHeader();
            var response = await httpClient.PostAsJsonAsync($"{BLL.Settings.Connections.GetServerAddress()}/api/ewallet/send/transaction", transaction);
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
            }
        }
    }
}
