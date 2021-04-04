using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class HistoryPageViewModel : ViewModels.BaseViewModel
    {
        public ICommand OpenPopUpDetailCommand { get; set; }
        public HistoryPageViewModel()
        {
            OpenPopUpDetailCommand = new Command(OpenPopUpDetail);
        }
        private void OpenPopUpDetail() => OpenModal(new Views.TransactionDetailsView());
    }
}
