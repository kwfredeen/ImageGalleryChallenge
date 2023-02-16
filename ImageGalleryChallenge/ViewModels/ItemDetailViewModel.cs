using ImageGalleryChallenge.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageGalleryChallenge.ViewModels
{
    [QueryProperty(nameof(ImageIndex), nameof(ImageIndex))]
    public class ItemDetailViewModel : BaseViewModel
    {

        public ItemDetailViewModel()
        {
            Title = "Details";

            Images = AllImages.Images;
        }

        public List<FavoritableImage> Images { get; set; }
        public string ImageIndex { get; set; } = "0";
        public FavoritableImage CurrentImage { get; set; }
        public int Position { get; set; }

        public async void LoadImages(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public void OnAppearing()
        {
            //Position = int.Parse(ImageIndex);

            Position = int.Parse(ImageIndex);
            
            //CurrentImage = Images[int.Parse(ImageIndex)];
        }
    }
}
