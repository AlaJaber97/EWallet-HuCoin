using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BeneficiariesPageViewModel
    {
        public ICommand AddNewBeneficiaryCommand { get; set; }
        public ICommand OpenBeneficiaryDetails { get; set; }
        public BeneficiariesPageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            AddNewBeneficiaryCommand = new Command(OpenBeneficiary);
        }
        private void AddNewBeneficiary()
        {
        
        }
        private void OpenBeneficiary()
        {
        
        }
    }
}
