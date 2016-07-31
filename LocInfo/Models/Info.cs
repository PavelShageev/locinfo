using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LocInfo.Models
{
    public class Info
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Suburb { get; set; }
        public string CatcmmentSchool { get; set; }
        public string LGA { get; set; }
        public List<BusStop> BusStops { get; set; }
        public List<Place> Museums { get; set; }
        public List<Place> PostOffices { get; set; }
        public List<Place> Theatres { get; set; }
        public List<Place> Galleries { get; set; }
        public List<Place> Churches { get; set; }

        public static Info Get(string longitude, string latitude)
        {
            var i = new Info();

            i.Longitude = longitude.Length > 20 ? longitude.Substring(0, 20) : longitude;
            i.Latitude = latitude.Length > 20 ? latitude.Substring(0,20) : latitude;

            i.BusStops = new List<BusStop>();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                var sql = "DECLARE @g geography; SET @g = geography::STPointFromText('POINT("+i.Longitude+" "+i.Latitude+")', 4283); "+
                    "SELECT TOP 5 stop_code, stop_name, stop_url, ROUND(loc.STDistance(@g), 0) AS Distance FROM stops " +
                    "WHERE stop_name IS NOT NULL AND loc IS NOT NULL ORDER BY loc.STDistance(@g)";
                var cmd = new SqlCommand(sql, cn);
                cn.Open();
                using(var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var bs = new BusStop();
                            
                            bs.Code = reader.GetString(reader.GetOrdinal("stop_code"));
                            bs.Name = reader.GetString(reader.GetOrdinal("stop_name"));
                            bs.Url = reader.GetString(reader.GetOrdinal("stop_url"));
                            bs.Distance = reader.GetDouble(reader.GetOrdinal("Distance"));
                            i.BusStops.Add(bs);
                        }
                    }
                }
            }

            i.PostOffices = Place.Get(i.Longitude, i.Latitude, "Post Office");
            i.Galleries = Place.Get(i.Longitude, i.Latitude, "Art Gallery");
            i.Museums = Place.Get(i.Longitude, i.Latitude, "Museum");
            i.Theatres = Place.Get(i.Longitude, i.Latitude, "Theatre");
            i.Churches = Place.Get(i.Longitude, i.Latitude, "Place of Worship");

            return i;
        } 
    }
}