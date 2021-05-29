using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuCoin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionDetailsView : ContentPage
    {
        public ICommand BackModalCommand { get; set; }
        public BLL.Models.TransactionClient Transaction { get; set; }
        public TransactionDetailsView(BLL.Models.TransactionClient transaction)
        {
            InitializeComponent();
            Transaction = transaction;
            BackModalCommand = new Command(CloseCurrentModal);
            this.BindingContext = this;
        }
        public void CloseCurrentModal()=>
            Application.Current.MainPage.Navigation.PopModalAsync();
    }
}