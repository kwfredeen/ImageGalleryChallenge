using System;
using SkiaSharp;
using System.IO;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ImageGalleryChallenge.Models
{
    public class FavoritableImage : INotifyPropertyChanged
    {
        private string _imagePath;
        public ImageSource Thumbnail { get; set; }
        public ImageSource Image { get; set; }

        private bool _favorite;
        public bool Favorite 
        { 
            get => _favorite; 
            set 
            {
                _favorite = value;
                OnPropertyChanged();
            } 
        }

        private Color _starColor;
        public Color StarColor 
        { 
            get => _starColor; 
            set
            {
                _starColor = value;
                OnPropertyChanged();
            } 
        }

        public Command FavoriteTapped { get; }

        public FavoritableImage(string imagePath)
        {
            _imagePath = imagePath;

            FavoriteTapped = new(() => OnFavoriteTapped());
            StarColor = Color.LightGray;
            Favorite = false;
        }

        public async void GenerateThumbnail()
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using Stream stream = assembly.GetManifestResourceStream(_imagePath);

            SKBitmap sourceBitmap = SKBitmap.Decode(stream);

            //limit max resolution for "full size" images to fit screen, to improve performance
            int maxDimension = Math.Min(3000, (int)Math.Min(DeviceDisplay.MainDisplayInfo.Width, DeviceDisplay.MainDisplayInfo.Height));
            if(sourceBitmap.Height > maxDimension || sourceBitmap.Width > maxDimension)
            {
                //resize image to save loading times in detail view
                double resizeRatio = ((double) maxDimension / Math.Max(sourceBitmap.Height, sourceBitmap.Width));

                int newHeight = (int)(sourceBitmap.Height * resizeRatio);
                int newWidth = (int)(sourceBitmap.Width * resizeRatio);
                SKBitmap resizedBitmap = sourceBitmap.Resize(new SKSizeI(newWidth,newHeight), SKFilterQuality.High);

                //create an imagesource from the reduced image
                Image = ImageSource.FromStream(() => SKImage.FromBitmap(resizedBitmap).Encode(SKEncodedImageFormat.Png, 100).AsStream());
            } else
            {
                //create an imagesource for the full image
                Image = ImageSource.FromStream(() => SKImage.FromBitmap(sourceBitmap).Encode(SKEncodedImageFormat.Png, 100).AsStream());
            }

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

        private void OnFavoriteTapped()
        {
            Favorite = !Favorite;

            StarColor = Favorite ? Color.Yellow : Color.LightGray;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
