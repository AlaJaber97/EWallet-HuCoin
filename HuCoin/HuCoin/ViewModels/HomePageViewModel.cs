using System;
using System.Collections.Generic;
using System.Text;
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
        public HomePageViewModel()
        {
            OpenCashOutServiceCommand = new Command(OpenCashOutService);
            OpenCashInServiceCommand = new Command(OpenCashInService);
            OpenTransferServiceCommand = new Command(OpenTransferService);
            OpenBeneficiariesCommand = new Command(OpenBeneficiaries);
        }
        private void OpenCashOutService() => OpenPage(new Views.CashOutServicePage());
        private void OpenCashInService() => OpenPage(new Views.CashInServicePage());
        private void OpenTransferService() => OpenPage(new Views.TransferServicePage());
        private void OpenBeneficiaries() => OpenPage(new Views.BeneficiariesPage());
    }
}