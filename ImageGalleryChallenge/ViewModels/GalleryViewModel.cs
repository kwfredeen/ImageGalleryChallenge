using ImageGalleryChallenge.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace ImageGalleryChallenge.ViewModels
{
    public class GalleryViewModel : BaseViewModel
    {
        public ObservableCollection<FavoritableImage> Images { get; set; } = new();
        public ObservableCollection<object> SelectedImages { get; set; }

        public GalleryViewModel()
        {
            Title = "Gallery";

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach(string resourceID in assembly.GetManifestResourceNames())
            {
                FavoritableImage favoritableImage = new(resourceID);
                favoritableImage.GenerateThumbnail();
                Images.Add(favoritableImage);
            }

            Console.WriteLine("check");
        }
    }
}
