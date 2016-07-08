using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace DataStorage.DataHandling
{
    public class GetAllImageNames
    {
        private readonly string _connectionString;

        public GetAllImageNames()
        {
            
                _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public List<string> Execute()
        {
            
            var fileList = GetFilnamesFromFilesProcessedOk();
            fileList.AddRange(GetFilnamesFromFileBlackList());
            return fileList;


        }

        private List<string> GetFilnamesFromFileBlackList()
        {
            List<string> fileList;
            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();
                fileList = cn.Query<string>("Select [OriginalFilename] from FileBlackList").ToList();
                cn.Close();

            }
            return fileList;
        }

        private List<string> GetFilnamesFromFilesProcessedOk()
        {
            List<string> fileList;
            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                cn.Open();
                
                fileList = cn.Query<string>("Select [OriginalFilename] from ElectricityData").ToList();
                cn.Close();
                
            }
            return fileList;
        }
    }
}