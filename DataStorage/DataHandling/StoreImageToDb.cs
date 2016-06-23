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

        private ElectricityMeterData TransformImageDataToElectricityMeterData(IImageData imageData, bool isValidNumber)
        {
            return new ElectricityMeterData()
            {
                CreatedDateTime = DateTime.Now,
                ElectricityValue = (int)imageData.ImageNumber,
                HasAcceptedElectricityValue = isValidNumber,
                ImageOfFrame = imageData.ImageCropped.ToJpegData(100),
                ImageOfFrameWithOutlines = imageData.ImageWithDigitsOutlined.ToJpegData(100)
            };
        }
    }
}