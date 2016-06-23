using System.Linq;
using Dapper;
using DataStorage.DataHandling;

namespace DataStorage
{
    public class GetPreviousValidNumber
    {
        public double Execute()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                cn.Open();
                //Person person = new Person { FirstName = "Foo", LastName = "Bar", Active = true, DateCreated = DateTime.Now };
                //int id = cn.Insert(person);
                var result = cn.Query<ElectricityMeterData>("Select top 1 * from ElectricityData where HasAcceptedElectricityValue=1 order by CreatedDateTime desc").FirstOrDefault();
                cn.Close();
                if (result != null) return result.ElectricityValue;
            }
            return -1;
        }
    }
}