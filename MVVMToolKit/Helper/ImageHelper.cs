using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// The image helper class
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Creates the bitmap image using the specified path
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="decodePixelWidth">The decode pixel width</param>
        /// <returns>The bitmap</returns>
        public static BitmapImage? CreateBitmapImage(string path, int decodePixelWidth = 300)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnDemand;
            bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmap.DecodePixelWidth = decodePixelWidth;
            bitmap.UriSource = new Uri($"{path}");
            bitmap.EndInit();

            return bitmap;
        }

        /// <summary>
        /// Creates the bitmap image by recorce using the specified path
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="decodePixelWidth">The decode pixel width</param>
        /// <returns>The bitmap</returns>
        public static BitmapImage CreateBitmapImageByRecorce(string path, int decodePixelWidth = 300)
        {
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnDemand;
            bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmap.DecodePixelWidth = decodePixelWidth;
            bitmap.UriSource = new Uri($"{path}", UriKind.Relative);
            bitmap.EndInit();

            return bitmap;
        }

        /// <summary>
        /// Describes whether set image for image
        /// </summary>
        /// <param name="imageControl">The image control</param>
        /// <param name="path">The path</param>
        /// <param name="isLocalDownloadFile">The is local download file</param>
        /// <param name="decodePixelWidth">The decode pixel width</param>
        /// <returns>The bool</returns>
        public static bool SetImageForImage(Image imageControl, string path, bool isLocalDownloadFile = true, int decodePixelWidth = 300)
        {
            BitmapImage? bitmapImg;
            if (isLocalDownloadFile)
            {
                bitmapImg = CreateBitmapImage(path, decodePixelWidth);
            }
            else
            {
                bitmapImg = new BitmapImage();
                bitmapImg.BeginInit();
                bitmapImg.CacheOption = BitmapCacheOption.OnDemand;
                bitmapImg.CreateOptions = BitmapCreateOptions.DelayCreation;
                bitmapImg.DecodePixelWidth = decodePixelWidth;
                bitmapImg.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmapImg.EndInit();
            }

            if (bitmapImg == null)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return false;
                }

                imageControl.Source = new BitmapImage(new Uri(path));
                return true;
            }

            imageControl.Source = bitmapImg;
            return true;
        }
        /// <summary>
        /// Bases the 64 to bitmap image using the specified base 64 img
        /// </summary>
        /// <param name="base64Img">The base 64 img</param>
        /// <returns>The bitmap img</returns>
        public static BitmapImage? Base64ToBitmapImage(string? base64Img)
        {
            if (string.IsNullOrWhiteSpace(base64Img))
            {
                return null;
            }

            byte[] binaryData = Convert.FromBase64String(base64Img);

            BitmapImage bitmapImg = new();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = new MemoryStream(binaryData);
            bitmapImg.EndInit();

            return bitmapImg;
        }
        /// <summary>
        /// Gets the average color using the specified bitmap
        /// </summary>
        /// <param name="bitmap">The bitmap</param>
        /// <exception cref="InvalidOperationException">BitmapSource must have Bgr24, Bgr32, Bgra32 or Pbgra32 format</exception>
        /// <returns>The color</returns>
        public static Color GetAverageColor(BitmapSource bitmap)
        {
            PixelFormat format = bitmap.Format;

            if (format != PixelFormats.Bgr24 &&
                format != PixelFormats.Bgr32 &&
                format != PixelFormats.Bgra32 &&
                format != PixelFormats.Pbgra32)
            {
                throw new InvalidOperationException("BitmapSource must have Bgr24, Bgr32, Bgra32 or Pbgra32 format");
            }

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int numPixels = width * height;
            int bytesPerPixel = format.BitsPerPixel / 8;
            byte[] pixelBuffer = new byte[numPixels * bytesPerPixel];

            bitmap.CopyPixels(pixelBuffer, width * bytesPerPixel, 0);

            long blue = 0;
            long green = 0;
            long red = 0;

            for (int i = 0; i < pixelBuffer.Length; i += bytesPerPixel)
            {
                blue += pixelBuffer[i];
                green += pixelBuffer[i + 1];
                red += pixelBuffer[i + 2];
            }

            return Color.FromRgb((byte)(red / numPixels), (byte)(green / numPixels), (byte)(blue / numPixels));
        }
    }
}
