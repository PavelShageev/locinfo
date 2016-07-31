using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace LocInfo.Models
{
    public class Place
    {
        public double Distance { get; set; }
        public string Name { get; set; }

        public static List<Place> Get(string longitude, string latitude, string filter, int count = 5)
        {
            var result = new List<Place>();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                var sql = "DECLARE @g geography; SET @g = geography::STPointFromText('POINT(" + longitude + " " + latitude + ")', 4283); " +
                    "SELECT TOP "+count+" name, ROUND(loc.STDistance(@g), 0) AS Distance FROM community " +
                    "WHERE name IS NOT NULL AND loc IS NOT NULL AND featuretyp = @filter ORDER BY loc.STDistance(@g)";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("@filter", System.Data.SqlDbType.NVarChar)).Value = filter;
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var place = new Place();
                            
                            place.Name = reader.GetString(reader.GetOrdinal("name"));
                            place.Distance = reader.GetDouble(reader.GetOrdinal("Distance"));
                            result.Add(place);
                        }
                    }
                }
            }

            return result;
        }
    }
}