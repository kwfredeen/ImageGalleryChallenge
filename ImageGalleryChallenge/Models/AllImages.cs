using ImageGalleryChallenge.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ImageGalleryChallenge.Models
{
    public class AllImages
    {
        public static List<FavoritableImage> Images = new();

        /// <summary>
        /// Loads images from embedded resources into Images, and generates renderable sources for each
        /// </summary>
        /// <param name="vm">ViewModel this method is called from</param>
        public static async void LoadImages(BaseViewModel vm)
        {
            Images.Clear();

            List<FavoritableImage> images = new();
            if (images.Any())
            {
                foreach (var image in images)
                {
                    Images.Add(image);
                }
            }
            else
            {
                //populate with stock images
                Assembly assembly = vm.GetType().GetTypeInfo().Assembly;
                foreach (string resourceID in assembly.GetManifestResourceNames())
                {
                    if (!resourceID.Contains(".Images.")) continue;
                    FavoritableImage favoritableImage = new(resourceID);
                    favoritableImage.GenerateThumbnail();
                    Images.Add(favoritableImage);
                }
            }
        }
    }
}
