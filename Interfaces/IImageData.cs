using Emgu.CV;
using Emgu.CV.Structure;

namespace Interfaces
{
    public interface IImageData
    {
        Image<Bgr, byte> ImageCropped { get; set; }
        double ImageNumber { get; set; }
        Image<Bgr, byte> ImageWithDigitsOutlined { get; set; }
    }
}