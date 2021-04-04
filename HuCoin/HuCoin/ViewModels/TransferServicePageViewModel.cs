using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class TransferServicePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand AddNewBeneficiaryCommand { get; set; }
        public ICommand TransferServiceCommand { get; set; }
        public TransferServicePageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            TransferServiceCommand = new Command(TransferService);
        }
        private void AddNewBeneficiary()
        {
        
        }
        private void TransferService()
        {

        }
    }
}
