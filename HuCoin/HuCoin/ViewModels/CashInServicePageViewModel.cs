using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class CashInServicePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand RechargeBalanceCommand { get; set; }
        public CashInServicePageViewModel()
        {
            RechargeBalanceCommand = new Command(RechargeBalance);
        }
        private void RechargeBalance() 
        {

        }
    }
}
