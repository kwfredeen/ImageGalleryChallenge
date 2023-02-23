using NUnit.Framework;
using Moq;
using SkiaSharp;
using static TestHelpers.RandomBitmap;
using static ImageGalleryChallenge.Helpers.BitmapHelpers;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace ImageGalleryTests
{
    [TestFixture]
    public class BitmapHelperTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region ShrinkImage
        /* Partitions:
         * Image relating to screen: smaller in both, smaller in width only, smaller in height only, larger in both
         * Image aspect: Square, Portrait, Landscape
         * Screen aspect: Square, Portrait, Landscape
         * Screen larger than maximum?: yes, no
         */

        [TestCase(640,480,1920,1080, Description = "480p on 1080p")]
        [TestCase(1920,1080,2560,1440, Description = "1082p on 1440p")]
        [TestCase(640,480,3840,2160, Description = "480p on 4K")]
        [TestCase(640,640,1920,1080, Description = "640 square on 1080p")]
        [TestCase(640,480,1920,1920, Description = "480p on 1920 square")]
        [TestCase(640,640,1920,1920, Description = "640 square on 1920 square")]
        [TestCase(1920,1080,1920,1080, Description = "1080p on 1080p")]
        [TestCase(1000,1000,1000,1000, Description = "1K square on 1K square")]
        public async Task DoesNotShrinkImageSmallerThanScreenBelowMax(int imageWidth, int imageHeight, int screenHeight, int screenWidth)
        {
            //create an image to test with
            SKBitmap testImage = CreateRandomBitmap(imageHeight, imageWidth);

            //try shrinking the image
            SKBitmap resultImage = await ShrinkImage(testImage, 6000, new(screenWidth, screenHeight, 1, DisplayOrientation.Portrait, DisplayRotation.Rotation0));

            //check that the image has not changed sizes. Also checks that aspect ratio has not changed as a side effect.
            Assert.Multiple(() =>
            {
                Assert.That(resultImage.Height, Is.EqualTo(imageHeight), "Image height changed when image did not need to shrink");
                Assert.That(resultImage.Width, Is.EqualTo(imageWidth), "Image width changed when image did not need to shrink");
            });
        }

        [TestCase(1920,1080, 1280,720, 1300, Description = "1080p on 720p")]
        [TestCase(2560,1440, 1920,1080, 3000, Description = "1440p on 1080p")]
        [TestCase(3480,2160, 2560,1440, 5000, Description = "4K on 1440p")]
        [TestCase(2000,2000, 1280,720, 1300, Description = "2k square on 720p")]
        [TestCase(1920,1080, 1000,1000, 3000, Description = "1080p on 1k square")]
        [TestCase(1000,1000, 700,700, 3000, Description = "1k square on 700 square")]
        public async Task ShrinksToScreenWhenLargerThanScreen(int imageWidth, int imageHeight, int screenHeight, int screenWidth, int maxDimension)
        {
            SKBitmap testImage = CreateRandomBitmap(imageHeight, imageWidth);

            //Shrink image
            SKBitmap resultImage = await ShrinkImage(testImage, maxDimension, new(screenWidth, screenHeight, 1, DisplayOrientation.Portrait, DisplayRotation.Rotation0));

            Assert.Multiple(() =>
            {
                //check that the aspect ratio is the same as what we started with
                double originalAspect = (double) imageWidth / imageHeight;
                double actualAspect = (double) resultImage.Width / resultImage.Height;
                Assert.That(actualAspect, Is.EqualTo(originalAspect).Within(0.002), "Aspect ratio has changed!");

                //check that maximum result dimension is equal to maximum screen dimension
                int screenMax = Math.Max(screenWidth, screenHeight);
                int actualMax = Math.Max(resultImage.Width, resultImage.Height);
                Assert.That(actualMax, Is.EqualTo(screenMax), "Result is larger than the screen!");
            });
        }

        [TestCase(2560,1440, 1920,1080, 1000, Description = "1440p on 1080p, max 1k")]
        [TestCase(3480,2160, 3480,2160, 3000, Description = "4K on 4K, max 3k")]
        [TestCase(1000,1000, 1500,1500, 750, Description ="1k square on 1.5k square, max 700")]
        public async Task ShrinksToMaxWhenImageAndScreenAreGreaterThanMax(int imageWidth, int imageHeight, int screenHeight, int screenWidth, int maxDimension)
        {
            SKBitmap testImage = CreateRandomBitmap(imageHeight, imageWidth);

            //Shrink image
            SKBitmap resultImage = await ShrinkImage(testImage, maxDimension, new(screenWidth, screenHeight, 1, DisplayOrientation.Portrait, DisplayRotation.Rotation0));

            Assert.Multiple(() =>
            {
                //check that the aspect ratio is the same as what we started with
                double originalAspect = (double)imageWidth / imageHeight;
                double actualAspect = (double)resultImage.Width / resultImage.Height;
                Assert.That(actualAspect, Is.EqualTo(originalAspect).Within(0.002), "Aspect ratio has changed!");

                //check that maximum result dimension is less than or equal to maximum dimension
                int actualMax = Math.Max(resultImage.Width, resultImage.Height);
                Assert.That(actualMax, Is.EqualTo(maxDimension), "Result is larger than the screen!");
            });
        }

        #endregion
    }
}
