using System;
using ElMeInterfaces;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class ImageData : IImageData
    {
        

        public DateTime FileCreatedDateTime { get; set; }
        public Image<Bgr,Byte> ImageWithDigitsOutlined { get; set; }
        public double ImageNumber { get; set; }
        public string OriginalFilename { get; set; }

        public Image<Bgr,byte> ImageCropped { get; set; }
    }
}