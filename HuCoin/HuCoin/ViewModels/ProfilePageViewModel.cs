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
        public BLL.Models.User User { get; set; }
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
                using var SourceStream = await photo.OpenReadAsync();
                User.Image = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(User.Image, 0, (int)SourceStream.Length);
                OnPropertyChanged(nameof(User));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }
        }
        private void UpdateProfile()
        {

        }
    }
}
