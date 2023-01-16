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
                var geometry = new MagickGeometry(newWidth, newHeight);

                image.Crop(geometry,Gravity.Center);
                //geometry.Greater = true; //Only Shrink Flag (">"): https://imagemagick.org/Usage/resize/#shrink
                geometry.IgnoreAspectRatio = true; //in case it needs to enlarge
                image.Resize(geometry);

                using (var memoryStream = new MemoryStream())
                {
                    image.Write(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}