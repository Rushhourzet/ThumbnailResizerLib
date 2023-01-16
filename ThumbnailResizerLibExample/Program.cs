// See https://aka.ms/new-console-template for more information
using ThumbnailResizerLib;

var originalImg = File.ReadAllBytes("C:/diagram2.png");
var thumbnail = originalImg
    .CropAndResizeImage(128,128);
File.WriteAllBytes("C:/thumbnail.png", thumbnail);
