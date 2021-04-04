﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class BaseViewModel
    {
        public ICommand BackPageCommand { get; set; }
        public BaseViewModel()
        {
            BackPageCommand = new Command(CloseCurrentPage);
        }

        public void OpenPageAsMainPage(Page page)
        {
            App.Current.MainPage = new NavigationPage(page);
        }

        public void OpenPage(Page page)
        {
            App.Current.MainPage.Navigation.PushAsync(page);
        }
        public void OpenModal(Page page)
        {
            App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public void CloseCurrentPage()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }
        public void CloseCurrentModal()
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
