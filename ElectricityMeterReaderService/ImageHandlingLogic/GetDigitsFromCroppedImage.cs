using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{

    public class GetDigitsFromCroppedImage
    {

        private List<DigitTemplateInfo> _digitTemplateInfoList;

        public GetDigitsFromCroppedImage()
        {
            //Create digittemplates
            CreateDigitTemplateInfoList();

        }

        private void CreateDigitTemplateInfoList()
        {
            var templatePath = ImageHandlingLogicHelper.GetTemplatePath();
            _digitTemplateInfoList = new List<DigitTemplateInfo>(10);
            for (int i = 0; i < 10; i++)
            {
                _digitTemplateInfoList.Add(CreateDigitTemplateInfoList(i, templatePath));
            }
        }

        private DigitTemplateInfo CreateDigitTemplateInfoList(int i, string templatePath)
        {
            var templateFileName = GetTemplateFileName(i);
            
            return new DigitTemplateInfo() { DigitNumber = i, TemplateFilePath = Path.Combine(templatePath, templateFileName) };
        }

        

        private string GetTemplateFileName(int i)
        {
            return ConfigurationManager.AppSettings[i.ToString()];
        }

        public DigitsAndDigitRectangles Process(Image<Bgr, byte> numberPlateImage)
        {
            //Lighten numberPlageImage
            //var lightenedNumberPlateImage = LightenNumberPlateImage(numberPlateImage);

            var digitData = new DigitsAndDigitRectangles();
            foreach (var digitTemplateInfo in _digitTemplateInfoList)
            {
                var foundDigitData= FindDigit(numberPlateImage, digitTemplateInfo);
                if (foundDigitData.Any())
                {
                    digitData.FoundDigitsData.AddRange(foundDigitData);
                }
            }
            if (digitData.FoundDigitsData.Any())
            {
                return digitData;
            }
            return null;
        }

        private Image<Bgr,byte> LightenNumberPlateImage(Image<Bgr, byte> numberPlateImage)
        {
            var resultImage = numberPlateImage.ThresholdAdaptive(new Bgr(Color.White), AdaptiveThresholdType.MeanC, ThresholdType.Binary, 31, new Bgr(Color.Black));
  
                                  CvInvoke.Imshow("test",resultImage);
            CvInvoke.WaitKey(0);
            return resultImage;
        }

        private IEnumerable<DigitAndDigitRectangle> FindDigit(Image<Bgr, byte> source, DigitTemplateInfo digitTemplateInfo)
        {
            
            Image<Bgr, byte> template = new Image<Bgr, byte>(digitTemplateInfo.TemplateFilePath);


            var foundRectanglesList = new List<Rectangle>();
            var foundResultsList = new List<DigitAndDigitRectangle>();

            DigitAndDigitRectangle findResult = new DigitAndDigitRectangle() { ResultImage = source };
            do
            {
                findResult = SearchImageForDigit(findResult.ResultImage, template, digitTemplateInfo.DigitNumber);
                if (findResult != null)
                {
                    foundRectanglesList.Add(findResult.FoundRectangle);
                    foundResultsList.Add(findResult);
                }
            } while (findResult != null);

            return foundResultsList;

        }

        private DigitAndDigitRectangle SearchImageForDigit(Image<Bgr, byte> imageToSearch, Image<Bgr, byte> searchTemplate, int numberToSearchFor = 0)
        {
            using (Image<Gray, float> result = imageToSearch.MatchTemplate(searchTemplate, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;

                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                if (maxValues[0] > 0.92)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], searchTemplate.Size);

                    
                    for (int i = searchTemplate.Size.Width; i > 1; i--)
                    {
                        match = new Rectangle(maxLocations[0].X + 5, maxLocations[0].Y, searchTemplate.Size.Width - (10 + i), searchTemplate.Size.Height);
                        imageToSearch.Draw(match, new Bgr(Color.Gray), 3);
                    }
                    
                    return new DigitAndDigitRectangle() { FoundRectangle = match, ResultImage = imageToSearch, FoundInteger = numberToSearchFor };
                }

            }
            return null;
        }


    }
}