using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BeneficiariesPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand AddNewBeneficiaryCommand { get; set; }
        public ICommand OpenBeneficiaryDetailsCommand { get; set; }
        public BeneficiariesPageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            OpenBeneficiaryDetailsCommand = new Command(OpenBeneficiaryDetails);
        }
        private void AddNewBeneficiary()=> OpenPage(new Views.AddBeneficiaryPage());
        private void OpenBeneficiaryDetails()
        {
        
        }
    }
}
