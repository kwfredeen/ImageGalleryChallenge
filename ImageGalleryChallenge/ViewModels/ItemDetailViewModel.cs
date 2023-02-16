using ImageGalleryChallenge.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void OnAppearing()
        {
            Position = int.Parse(ImageIndex);
        }
    }
}
