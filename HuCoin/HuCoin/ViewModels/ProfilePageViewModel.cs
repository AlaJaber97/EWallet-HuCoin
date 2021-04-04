using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HuCoin.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ICommand UpdateProfileCommand { get; private set; }
        public ICommand PickPhotoCommand { get; set; }
        public ProfilePageViewModel()
        {
            PickPhotoCommand = new Command(()=> PickPhotoAsync().ConfigureAwait(false));
            UpdateProfileCommand = new Command(UpdateProfile);
        }
        private async Task PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                await LoadPhotoAsync(photo);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }
        }
        private async Task LoadPhotoAsync(FileResult photo)
        {

        }
        private void UpdateProfile()
        {

        }
    }
}
