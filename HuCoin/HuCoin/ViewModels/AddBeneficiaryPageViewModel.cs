using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace HuCoin.ViewModels
{
    public class AddBeneficiaryPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand SaveBeneficiaryCommand { get; set; }
        public Models.Beneficiary Beneficiary { get; set; }
        public AddBeneficiaryPageViewModel()
        {
            Beneficiary = new Models.Beneficiary();
            SaveBeneficiaryCommand = new Command(()=> SaveBeneficiary().ConfigureAwait(false));
        }
        private async Task SaveBeneficiary()
        {
            try
            {
                using var loadingview = new Components.LoadingView();
                using var db = new Data.DbCon();
                db.Beneficiaries.Add(Beneficiary);
                if (await db.SaveChangesAsync() > 0)
                {   
                    CloseCurrentModal();
                    MessagingCenter.Send(this, "AddNewBeneficiary");
                }
                else
                {
                    await DisplayAlert("Add new beneficiary", "can not add new beneficiary", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Add new beneficiary", ex.ToString(), "Ok");
            }
        }
    }
}
