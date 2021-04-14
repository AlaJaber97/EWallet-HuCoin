using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class CashOutServicePageViewModel : ViewModels.BaseViewModel
    {
        public ICommand CashOutServiceCommand { get; set; }
        public CashOutServicePageViewModel()
        {
            CashOutServiceCommand = new Command(CashOutService);
        }
        private void CashOutService()
        {
            OpenPage(new Views.VerficationPinPage());
            //call-api

            //return to home page
        }
    }
}
