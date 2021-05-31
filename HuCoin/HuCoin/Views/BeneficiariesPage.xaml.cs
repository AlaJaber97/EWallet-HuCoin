using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HuCoin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BeneficiariesPage : ContentPage
    {
        public BeneficiariesPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            var bindingContext = this.BindingContext as ViewModels.BaseViewModel;
            bindingContext.BackPageCommand.Execute(null);
            return false;
        }
    }
}