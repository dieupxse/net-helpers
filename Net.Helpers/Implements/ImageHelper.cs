using Net.Helpers.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Linq;

namespace Net.Helpers.Implements
{
    public class ImageHelper : IImageHelper
    {
        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <returns>Image</returns>
        public Image<Rgba32> ResizeImage(Image<Rgba32> image, int width)
        {
            try
            {
                // lấy chiều rộng và chiều cao ban đầu của ảnh
                int originalW = image.Width;
                int originalH = image.Height;
                if (width > originalW) return image;
                // lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh (nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
                int resizedW = width;
                int resizedH = (originalH * resizedW) / originalW;
                image.Mutate(x => x.Resize(resizedW, resizedH));
                return image;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// SaveResizeImage
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <param name="path"></param>
        /// <returns>bool</returns>
        public bool SaveResizeImage(Image<Rgba32> img, int width, string path)
        {
            try
            {
                var imageResize = ResizeImage(img, width);
                imageResize.Save(path);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SaveImage(Image<Rgba32> img, string path)
        {
            try
            {
                img.Save(path);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Save Croped Image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool SaveCroppedImage(Image<Rgba32> image, int maxWidth, int maxHeight, string filePath, long compress = 100)
        {
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                Rectangle rec = new Rectangle(left, top, srcWidth, srcHeight);
                image.Mutate(x => x.Crop(rec).Resize(maxWidth, maxHeight));
                image.Save(filePath);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


        public Image<Rgba32> CropImage(Image<Rgba32> image, int maxWidth, int maxHeight)
        {
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                Rectangle rec = new Rectangle(left, top, srcWidth, srcHeight);
                image.Mutate(x => x.Crop(rec).Resize(maxWidth, maxHeight));
                return image;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }



        /// <summary>
        /// Convert base64 to Image
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public Image<Rgba32> Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);
            image.Mutate(x => x.Resize(image.Width, image.Height));
            return image;
        }

        public string ImageToBase64(Image<Rgba32> image, IImageFormat format)
        {
            string base64String;
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                ms.Position = 0;
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);
            }
            return base64String;
        }

        public byte[] ConvertStreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        //public static byte[] GetBitmapBytes(Bitmap source)
        //{
        //    //Settings to increase quality of the image
        //    ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[4];
        //    EncoderParameters parameters = new EncoderParameters(1);
        //    parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

        //    //Temporary stream to save the bitmap
        //    using (MemoryStream tmpStream = new MemoryStream())
        //    {
        //        source.Save(tmpStream, codec, parameters);

        //        //Get image bytes from temporary stream
        //        byte[] result = new byte[tmpStream.Length];
        //        tmpStream.Seek(0, SeekOrigin.Begin);
        //        tmpStream.Read(result, 0, (int)tmpStream.Length);

        //        return result;
        //    }
        //}

        public Image<Rgba32> CropImage(Stream content, int x, int y, int width, int height)
        {
            using (Image<Rgba32> image = Image.Load(content))
            {
                return CropImage(image, x, y, width, height);
            }
        }

        public Image<Rgba32> CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }
        public Image<Rgba32> CropImage(Image<Rgba32> image, int x, int y, int width, int height)
        {
            image.Mutate(e => e.Crop(new Rectangle(x, y, width, height)));
            return image;
        }
    }
}
