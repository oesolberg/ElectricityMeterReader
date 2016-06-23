using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElMeInterfaces
{

    public interface IImageData
    {
        Image<Bgr, byte> ImageCropped { get; set; }
        double ImageNumber { get; set; }

        string OriginalFilename { get; set; }

        DateTime CreatedDateTime { get; set; }

        Image<Bgr, byte> ImageWithDigitsOutlined { get; set; }
    }
}
