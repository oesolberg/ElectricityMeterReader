using System;
using System.Linq;
using Dapper;
using ElMeInterfaces;
using DapperExtensions;

namespace DataStorage.DataHandling
{
    public class StoreImageToDb
    {
        public int Execute(IImageData imageData, bool isValidNumber)
        {
            var electricityMeterData = TransformImageDataToElectricityMeterData(imageData, isValidNumber);
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                cn.Open();
                //Person person = new Person { FirstName = "Foo", LastName = "Bar", Active = true, DateCreated = DateTime.Now };
                //int id = cn.Insert(person);
                var id = cn.Insert(electricityMeterData);
                cn.Close();
                return id;
            }
            
        }

        private ElectricityData TransformImageDataToElectricityMeterData(IImageData imageData, bool isValidNumber)
        {
            return new ElectricityData()
            {
                CreatedDateTime = DateTime.Now,
                FileCreatedDateTime = imageData.FileCreatedDateTime,
                ElectricityValue = (int)imageData.ImageNumber,
                HasAcceptedElectricityValue = isValidNumber,
                JpgImageOfFrame = imageData.ImageCropped.ToJpegData(100),
                JpgImageOfFrameWithOutlines = imageData.ImageWithDigitsOutlined.ToJpegData(100),
                OriginalFilename = imageData.OriginalFilename
            };
        }
    }
}