using ImageGalleryChallenge.Models;
using ImageGalleryChallenge.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageGalleryChallenge.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private FavoritableImage _selectedImage;

        public List<FavoritableImage> Images { get; private set; }
        public Command LoadImagesCommand { get; }
        public Command AddItemCommand { get; }
        public Command<FavoritableImage> ImageTapped { get; }
        public int NumberOfColumns { get; set; } = 3;
        public int ColumnWidth { get; set; } = (int)((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 10) / 3;

        public ItemsViewModel()
        {
            Title = "Browse";
            AllImages.LoadImages(this);
            Images = AllImages.Images;
            LoadImagesCommand = new Command(async () => await ExecuteLoadImagesCommand());

            ImageTapped = new Command<FavoritableImage>(OnImageSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadImagesCommand()
        {
            IsBusy = true;

            try
            {
                //Images.Clear();
                Images = AllImages.Images;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedImage = null;
        }

        public FavoritableImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                SetProperty(ref _selectedImage, value);
                OnImageSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnImageSelected(FavoritableImage image)
        {
            if (image is null) return;

            int imageIndex = Images.IndexOf(image);
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ImageIndex)}={imageIndex}");
        }
    }
}