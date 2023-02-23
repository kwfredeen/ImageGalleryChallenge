using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageGalleryChallenge.Helpers
{
    public static class BitmapHelpers
    {
        /// <summary>
        /// Resizes an image so that the largest dimension of the image matches the largest
        /// dimension of the screen, or a specified maximum if both screen dimensions are larger.
        /// </summary>
        /// <param name="bitmap">Bitmap to shrink</param>
        /// <param name="maxSize">Maximum size any dimension of the image can be</param>
        /// <param name="displayInfo">Display info passed from caller. Enables unit testing.</param>
        /// <returns>Image resized to fit screen, maintining the aspect ratio. If image is smaller or equal, this returns the original image</returns>
        public static async Task<SKBitmap> ShrinkImage(SKBitmap bitmap, int maxSize, DisplayInfo displayInfo)
        {
            int maxFinalDimension = (int) Math.Min(maxSize, Math.Max(displayInfo.Width, displayInfo.Height));

            //don't scale if image fits on screen
            if (bitmap.Height <= maxFinalDimension && bitmap.Width <= maxFinalDimension) return bitmap;

            double resizeRatio = ((double)maxFinalDimension / Math.Max(bitmap.Height, bitmap.Width));

            int newHeight = (int)(bitmap.Height * resizeRatio);
            int newWidth = (int)(bitmap.Width * resizeRatio);

            return bitmap.Resize(new SKSizeI(newWidth, newHeight), SKFilterQuality.High);

            throw new NotImplementedException();
        }
    }
}
