using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace Net.Helpers.Interfaces
{
    public interface IImageHelper
    {
        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <returns>Image</returns>
        Image<Rgba32> ResizeImage(Image<Rgba32> image, int width);

        /// <summary>
        /// SaveResizeImage
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <param name="path"></param>
        /// <returns>bool</returns>
        bool SaveResizeImage(Image<Rgba32> img, int width, string path);
        bool SaveImage(Image<Rgba32> img, string path);
        /// <summary>
        /// Save Croped Image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool SaveCroppedImage(Image<Rgba32> image, int maxWidth, int maxHeight, string filePath, long compress = 100);



        /// <summary>
        /// Convert base64 to Image
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        Image<Rgba32> Base64ToImage(string base64String);

        string ImageToBase64(Image<Rgba32> image, IImageFormat format);

        byte[] ConvertStreamToByteArray(Stream input);
        Image<Rgba32> CropImage(Image<Rgba32> image, int maxWidth, int maxHeight);
        Image<Rgba32> CropImage(Stream content, int x, int y, int width, int height);

        Image<Rgba32> CropImage(byte[] content, int x, int y, int width, int height);
        Image<Rgba32> CropImage(Image<Rgba32> image, int x, int y, int width, int height);
    }
}
