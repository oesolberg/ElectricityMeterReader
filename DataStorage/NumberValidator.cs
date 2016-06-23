
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
            var readElectricityNumber = imageData.ImageNumber;

            if (lastValueInDb < 1)
            {
                //Only compare with config value
                var configValue = ConfigReader.GetMinimumValidElectricityNumber();
                if (configValue > readElectricityNumber) return false;
            }
          
            //Compare with db if we have a value
            if (lastValueInDb>1 && ((readElectricityNumber<lastValueInDb) || ( readElectricityNumber > (lastValueInDb + 100)))) return false;
            

            return true;
        }

        private double GetLastValidElectricityValue()
        {
            var dbOperation = new GetPreviousValidNumber();
            return dbOperation.Execute();
            
        }
    }
}