using System.Configuration;
using System.IO;

namespace ElectricityMeterReaderService.ImageHandlingLogic
{
    internal static class ImageHandlingLogicHelper
    {
        public static string GetTemplatePath()
        {
            var applicatonTemplatePath = ConfigurationManager.AppSettings["applicationTemplatePath"];
            var applicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(applicationPath, applicatonTemplatePath);
        }
    }
}