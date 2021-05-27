using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BeneficiariesPageViewModel : ViewModels.BaseViewModel, IDisposable
    {
        public List<Models.Beneficiary> Beneficiaries { get; set; }
        public ICommand AddNewBeneficiaryCommand { get; set; }
        public ICommand OpenBeneficiaryDetailsCommand { get; set; }
        public BeneficiariesPageViewModel()
        {
            AddNewBeneficiaryCommand = new Command(AddNewBeneficiary);
            OpenBeneficiaryDetailsCommand = new Command<Models.Beneficiary>(beneficiary=> OpenBeneficiaryDetails(beneficiary));
            LoadBeneficiaries().ConfigureAwait(false);
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
            MessagingCenter.Subscribe<AddBeneficiaryPageViewModel>(this, "AddNewBeneficiary", (sender) => LoadBeneficiaries().ConfigureAwait(false));
        }
        private void OpenBeneficiaryDetails(Models.Beneficiary beneficiary)
        {
            OpenPage(new Views.BeneficiaryDetailsPage());
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<AddBeneficiaryPageViewModel>(this, "AddNewBeneficiary");
        }
    }
}
