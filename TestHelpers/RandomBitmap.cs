using SkiaSharp;

namespace TestHelpers
{
    public static class RandomBitmap
    {
        private static Random random = new();

        public static SKBitmap CreateRandomBitmap(int height, int width)
        {
            SKBitmap bitmap = new(width, height);
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, new((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                }
            }

            return bitmap;
        }
    }
}