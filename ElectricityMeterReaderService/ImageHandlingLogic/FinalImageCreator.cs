using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class FinalImageCreator
    {
        private readonly Image<Bgr, byte> _numberPlateImage;
        private readonly DigitsAndDigitRectangles _foundDigitData;

        public FinalImageCreator(Image<Bgr, byte> numberPlateImage, DigitsAndDigitRectangles foundDigitData)
        {
            _numberPlateImage = numberPlateImage;
            _foundDigitData = foundDigitData;
        }

        internal Image<Bgr, byte> CreateImageWithDigitsOutlined()
        {
            foreach (var digitAndDigitRectangle in _foundDigitData.FoundDigitsData)
            {
                _numberPlateImage.Draw(digitAndDigitRectangle.FoundRectangle, new Bgr(Color.Red), 2);
            }
            return _numberPlateImage;
        }
    }
}