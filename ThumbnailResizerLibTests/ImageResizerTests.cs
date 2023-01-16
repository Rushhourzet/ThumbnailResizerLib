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

using Xunit;
using ImageMagick;
using ThumbnailResizerLib;

namespace ThumbnailResizerLibTests
{   

    public class ImageTests
    {
        private static readonly string _workingDirectory = Environment.CurrentDirectory;
        private static readonly string _projectDirectory = Directory.GetParent(_workingDirectory).Parent.Parent.FullName;
        private static readonly string _imageName = "test.jpg";
        private static readonly string _imagePath = Path.Combine(_projectDirectory, _imageName);

        [Theory]
        [InlineData(128,64)]
        [InlineData(64,128)]
        [InlineData(128,128)]
        public void CropAndResizeImage_ExampleImage_ReturnsCroppedAndResizedImage(int width, int height)
        {
            // Arrange
            var image = File.ReadAllBytes(_imagePath);

            // Act
            var croppedAndResizedImage = image.CropAndResizeImage(width, height);

            // Assert
            using (var imageStream = new MemoryStream(croppedAndResizedImage))
            {
                var imageInfo = new MagickImageInfo(imageStream);
                Assert.Equal(width, imageInfo.Width);
                Assert.Equal(height, imageInfo.Height);
            }
        }

        [Theory]
        [InlineData(0, 128)]
        public void CropAndResizeImage_ZeroWidth_ReturnsOriginalWidthAndKeepsAspectRatio(int width, int height)
        {
            // Arrange
            var image = File.ReadAllBytes(_imagePath);
            var originalImageInfo = new MagickImageInfo(image);

            // Act
            var croppedAndResizedImage = image.CropAndResizeImage(width, height);

            // Assert
            using (var imageStream = new MemoryStream(croppedAndResizedImage))
            {
                var imageInfo = new MagickImageInfo(imageStream);
                Assert.Equal(originalImageInfo.Width, imageInfo.Width);
                Assert.Equal(height, imageInfo.Height);
            }
        }

        [Theory]
        [InlineData(256, 0)]
        public void CropAndResizeImage_ZeroHeight_ReturnsOriginalHeightAndKeepsAspectRatio(int width, int height)
        {
            // Arrange
            var image = File.ReadAllBytes(_imagePath);
            var originalImageInfo = new MagickImageInfo(image);

            // Act
            var croppedAndResizedImage = image.CropAndResizeImage(width, height);

            // Assert
            using (var imageStream = new MemoryStream(croppedAndResizedImage))
            {
                var imageInfo = new MagickImageInfo(imageStream);
                Assert.Equal(width, imageInfo.Width);
                Assert.Equal(originalImageInfo.Height, imageInfo.Height);
            }
        }

        [Theory]
        [InlineData(3000, 3000)]
        public void CropAndResizeImage_MakeImageBigger_StretchImage(int width, int height)
        {
            // Arrange
            var image = File.ReadAllBytes(_imagePath);

            // Act
            var croppedAndResizedImage = image.CropAndResizeImage(width, height);

            // Assert
            using (var imageStream = new MemoryStream(croppedAndResizedImage))
            {
                var imageInfo = new MagickImageInfo(imageStream);
                Assert.Equal(width, imageInfo.Width);
                Assert.Equal(height, imageInfo.Height);
            }
        }

        [Theory]
        [InlineData(64, -128)]
        [InlineData(-64, 64)]
        public void CropAndResizeImage_NegativeParameter_ThrowsArgumentException(int width, int height)
        {
            // Arrange
            var image = File.ReadAllBytes(_imagePath);
            var originalImageInfo = new MagickImageInfo(image);

            // Action
            var action = () => image.CropAndResizeImage(width, height);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}