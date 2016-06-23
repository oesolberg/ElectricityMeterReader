using System.Configuration;

namespace DataStorage
{
    public static class ConfigReader
    {
        public static double GetMinimumValidElectricityNumber()
        {
            double minValue = 120000;
            var minimumValueAsString = ConfigurationManager.AppSettings["minimumElectricityValue"];
            if (!string.IsNullOrWhiteSpace(minimumValueAsString))
            {
                double outValue = 0;
                if (double.TryParse(minimumValueAsString, out outValue))
                {
                    minValue = outValue;
                }
            }

            return minValue;
        }
    }
}