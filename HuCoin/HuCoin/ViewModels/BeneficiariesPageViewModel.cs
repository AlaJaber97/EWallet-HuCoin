using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BeneficiariesPageViewModel : ViewModels.BaseViewModel
    {
        public List<Models.Beneficiary> Beneficiaries { get; set; }
        public ICommand AddNewBeneficiaryCommand { get; set; }
        public ICommand OpenBeneficiaryDetailsCommand { get; set; }
        public BeneficiariesPageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            OpenBeneficiaryDetailsCommand = new Command<Models.Beneficiary>(beneficiary=> OpenBeneficiaryDetails(beneficiary));
            LoadBeneficiaries().ConfigureAwait(false);
            MessagingCenter.Subscribe<AddBeneficiaryPageViewModel>(this, "AddNewBeneficiary", (sender) =>
            {
                CloseCurrentPage();
                LoadBeneficiaries().ConfigureAwait(false);
            });
        }
        private async Task LoadBeneficiaries()
        {
            using var loadingview = new Components.LoadingView();

            using var db = new Data.DbCon();
            Beneficiaries = await db.Beneficiaries.ToListAsync();
            OnPropertyChanged(nameof(Beneficiaries));
        }
        private void AddNewBeneficiary()
        {
            OpenPage(new Views.AddBeneficiaryPage()); 
        }
        public override void CloseCurrentPage()
        {
            MessagingCenter.Unsubscribe<AddBeneficiaryPageViewModel>(this, "AddNewBeneficiary");
            base.CloseCurrentPage();
        }
    }
}
