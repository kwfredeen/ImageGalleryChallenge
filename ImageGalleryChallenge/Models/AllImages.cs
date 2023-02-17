using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ImageGalleryChallenge.Models
{
    public class AllImages
    {
        public static List<FavoritableImage> Images = new();

        public static async void LoadImages(object o)
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
                Assembly assembly = o.GetType().GetTypeInfo().Assembly;
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
