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
        public TransferServicePageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            TransferServiceCommand = new Command(TransferService);
            LoadBeneficiaries();
        }
        private void LoadBeneficiaries()
        {
            using var loadingview = new Components.LoadingView();

            using var db = new Data.DbCon();
            Beneficiaries = db.Beneficiaries.ToList();
            OnPropertyChanged(nameof(Beneficiaries));
        }
        private void AddNewBeneficiary()
        {
            OpenModal(new Views.AddBeneficiaryPage()); 
            MessagingCenter.Subscribe<AddBeneficiaryPageViewModel>(this, "AddNewBeneficiary", (sender) => LoadBeneficiaries());

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
                var message = await response.Content.ReadAsStringAsync();
                await DisplayAlert(string.Empty, message, "Ok").ConfigureAwait(false);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("An error occurred", error, "Ok").ConfigureAwait(false);
            }
        }
    }
}
