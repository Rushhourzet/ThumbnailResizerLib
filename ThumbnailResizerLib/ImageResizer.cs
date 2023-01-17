//  Copyright 2023 Sascha Hampl

//  Licensed under the ImageMagick License (the "License");
//  you may not use
//  this file except in compliance with the License.  You may obtain a copy
//  of the License at

//    https://imagemagick.org/script/license.php

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
//  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
//  License for the specific language governing permissions and limitations
//  under the License.

using ImageMagick;
using System.Runtime.CompilerServices;

namespace ThumbnailResizerLib
{
    public static class ImageResizer
    {
        /// <summary>
        /// Crops the image centered first and then resizes it
        /// </summary>
        /// <param name="originalImage">original image as byte[]</param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns>cropped and resized image as byte[]</returns>
        /// <exception cref="MagickException"></exception>
        public static byte[] CropAndResizeImage(this byte[] originalImage, int newWidth, int newHeight)
        {
            using (var image = new MagickImage(originalImage))
            {
                image.CropToAspectRatioCentered(newWidth, newHeight);
                image.ResizeImage(newWidth, newHeight);

                using (var memoryStream = new MemoryStream())
                {
                    image.Write(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private static MagickImage ResizeImage(this MagickImage image, int newWidth, int newHeight)
        {
            //geometry.Greater = true; //Only Shrink Flag (">"): https://imagemagick.org/Usage/resize/#shrink
            var geometry = new MagickGeometry(newWidth, newHeight);
            geometry.IgnoreAspectRatio = true; //in case it needs to enlarge
            image.Resize(geometry);
            return image;
        }

        private static MagickImage CropToAspectRatioCentered(this MagickImage image, int width, int height)
        {
            var currentAspectRatio = (double)image.Width / image.Height;
            var desiredAspectRatio = (double)width / height;

            if (currentAspectRatio > desiredAspectRatio)
            {
                // Crop the width to match the desired aspect ratio
                var newWidth = (int)(image.Height * desiredAspectRatio);
                var x = (image.Width - newWidth) / 2;
                image.Crop(new MagickGeometry(x, 0, newWidth, image.Height));
            }
            else if (currentAspectRatio < desiredAspectRatio)
            {
                // Crop the height to match the desired aspect ratio
                var newHeight = (int)(image.Width / desiredAspectRatio);
                var y = (image.Height - newHeight) / 2;
                image.Crop(new MagickGeometry(0, y, image.Width, newHeight));
            }
            return image;
        }
    }
}