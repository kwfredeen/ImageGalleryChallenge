using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using System.IO;
using Xamarin.Forms;
using System.Reflection;

namespace ImageGalleryChallenge.Models
{
    public class FavoritableImage
    {
        private string _imagePath;
        public ImageSource Thumbnail { get; set; }
        public ImageSource Image { get; set; }
        public bool Favorite { get; set; }

        public FavoritableImage(string imagePath)
        {
            _imagePath = imagePath;
        }

        public async void GenerateThumbnail()
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using Stream stream = assembly.GetManifestResourceStream(_imagePath);

            SKBitmap sourceBitmap = SKBitmap.Decode(stream);

            //create an imagesource for the full image
            Image = ImageSource.FromStream(() => SKImage.FromBitmap(sourceBitmap).Encode(SKEncodedImageFormat.Png, 100).AsStream());

            int cropSideLength = Math.Min(sourceBitmap.Width, sourceBitmap.Height);

            //crop to the largest centered square you can fit in the image
            SKRectI cropWindow = new((sourceBitmap.Width - cropSideLength) / 2, (sourceBitmap.Height - cropSideLength) / 2,
                (sourceBitmap.Width + cropSideLength) / 2, (sourceBitmap.Height + cropSideLength) / 2);

            //create a Pixmap object to perform cropping on
            SKPixmap sourcePixmap = new(sourceBitmap.Info, sourceBitmap.GetPixels());

            SKPixmap croppedPixmap = sourcePixmap.ExtractSubset(cropWindow);

            //write cropped pixels to new image, close to original size
            SKBitmap croppedBitmap = new(croppedPixmap.Info, croppedPixmap.RowBytes);
            croppedBitmap.SetPixels(croppedPixmap.GetPixels());
            
            //resize to small size for thumbnail
            SKBitmap thumbnail = new(200, 200);
            croppedBitmap.ScalePixels(thumbnail, SKFilterQuality.Medium);

            //create an imagesource
            Thumbnail = ImageSource.FromStream(() => SKImage.FromBitmap(thumbnail).Encode(SKEncodedImageFormat.Png, 100).AsStream());

            stream.Dispose();
        }
    }
}
