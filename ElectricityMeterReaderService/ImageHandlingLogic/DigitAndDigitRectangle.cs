using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class DigitAndDigitRectangle
    {
        public Rectangle FoundRectangle { get; set; }
        public Image<Bgr, byte> ResultImage { get; set; }
        public int FoundInteger { get; set; }
        public string DigitImageName { get; set; }
    }
}