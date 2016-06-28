using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace DataStorage.DataHandling
{
    public class GetAllImageNames
    {
        public List<string> Execute()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                cn.Open();
                //Person person = new Person { FirstName = "Foo", LastName = "Bar", Active = true, DateCreated = DateTime.Now };
                //int id = cn.Insert(person);
                var result = cn.Query<string>("Select [OriginalFilename] from ElectricityData").ToList();
                cn.Close();
                if (result.Any())
                    return result;
                return new List<string>();
            }
        }
    }
}