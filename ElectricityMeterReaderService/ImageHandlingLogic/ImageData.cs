﻿using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class ImageData
    {
        public Image<Bgr,Byte> ImageWithDigitsOutlined { get; set; }
        public double ImageNumber { get; set; }

        public Image<Bgr,byte> ImageCropped { get; set; }
    }
}