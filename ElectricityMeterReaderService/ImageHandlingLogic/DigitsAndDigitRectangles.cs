using System;
using System.Collections.Generic;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    public class DigitsAndDigitRectangles
    {
        public List<DigitAndDigitRectangle> FoundDigitsData{ get; set; }

        public DigitsAndDigitRectangles()
        {
            FoundDigitsData=new List<DigitAndDigitRectangle>();
        }
        public double GetFoundNumber()
        {
            FoundDigitsData.Sort((firstDigitAndRectangle, secondDigitAndRectangle) =>secondDigitAndRectangle.FoundRectangle.X.CompareTo(firstDigitAndRectangle.FoundRectangle.X));
            var foundNumber = 0;
            for (int i = 0; i < FoundDigitsData.Count; i++)
            {
                foundNumber += (FoundDigitsData[i].FoundInteger) * (int)(Math.Pow(10, i));

            }
            return foundNumber;
        }

    }
}
