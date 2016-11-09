
using System;
using ElMeInterfaces;
using OpenTK.Graphics.OpenGL;

namespace DataStorage
{
    public class NumberValidator
    {
        public bool IsElectricityNumberValid(IImageData imageData)
        {
            //Get previous values and check
            var lastValueInDb = GetLastValidElectricityValue();
            var dateTimeForLastValueInDb = GetDateTimeForLastValidElectricityValue();
            if (dateTimeForLastValueInDb == DateTime.MinValue)
            {
                dateTimeForLastValueInDb = imageData.FileCreatedDateTime.Date.AddDays(-1);
            }
            var readElectricityNumber = imageData.ImageNumber;

            if (lastValueInDb < 1)
            {
                //Only compare with config value
                var configValue = ConfigReader.GetMinimumValidElectricityNumber();
                if (configValue > readElectricityNumber) return false;
            }
          //Find the number of days between the two values
            var numberOfDaysSinceLastValidNumber = (imageData.FileCreatedDateTime - dateTimeForLastValueInDb).TotalDays;
            //Compare with db if we have a value
            if (lastValueInDb > 1 &&
                ((readElectricityNumber < lastValueInDb) || (readElectricityNumber > (lastValueInDb + (200*numberOfDaysSinceLastValidNumber)))))
            {
                return false;
            }

            return true;
        }

        private DateTime GetDateTimeForLastValidElectricityValue()
        {
            var dbOperation = new GetDateTimeForPreviousValidNumber();
            return dbOperation.Execute();

        }

        private double GetLastValidElectricityValue()
        {
            var dbOperation = new GetPreviousValidNumber();
            return dbOperation.Execute();
            
        }
    }
}