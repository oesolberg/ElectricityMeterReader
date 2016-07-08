using System;
using System.Linq;
using Dapper;
using ElMeInterfaces;
using DapperExtensions;

namespace DataStorage.DataHandling
{
    public class StoreImageToDb
    {
        private string _connectionString;

        public StoreImageToDb()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public int Execute(IImageData imageData, bool isValidNumber)
        {
            var electricityMeterData = TransformImageDataToElectricityMeterData(imageData, isValidNumber);

            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();
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
                ElectricityValue = (int) imageData.ImageNumber,
                HasAcceptedElectricityValue = isValidNumber,
                JpgImageOfFrame = imageData.ImageCropped.ToJpegData(100),
                JpgImageOfFrameWithOutlines = imageData.ImageWithDigitsOutlined.ToJpegData(100),
                OriginalFilename = imageData.OriginalFilename
            };
        }

        public int BlackListFile(string originalFileName, string filepath)
        {
            var fileBlackListData = TransformDataToBlackListData(originalFileName, filepath);

            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();
                var id = cn.Insert(fileBlackListData);
                cn.Close();
                return id;
            }
        }

        private FileBlackList TransformDataToBlackListData(string originalFileName, string filepath)
        {
            return new FileBlackList() {FullpathToFile = filepath, OriginalFilename = originalFileName};
        }
    }

    internal class FileBlackList
    {
        public int Id { get; set; }
        public string OriginalFilename { get; set; }
        public string FullpathToFile { get; set; }
    }
}
