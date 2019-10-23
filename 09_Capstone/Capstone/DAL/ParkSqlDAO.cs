using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private string connectionString;

        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> GetAllParks()
        {
            List<Park> parks = new List<Park>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM park ORDER BY name ASC", conn);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    parks.Add(RowToPark(r));   
                }
            }
            return parks;
        }

        public Park GetParkByID(int parkID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE park_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", parkID);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    return RowToPark(r);
                }
            }
            return null;
        }
        private Park RowToPark(SqlDataReader r)
        {
            Park p;
            int parkID = Convert.ToInt32(r["park_id"]);
            string name = Convert.ToString(r["name"]);
            string location = Convert.ToString(r["location"]);
            DateTime establishDate = Convert.ToDateTime(r["establish_date"]);
            int area = Convert.ToInt32(r["area"]);
            int visitor = Convert.ToInt32(r["visitors"]);
            string description = Convert.ToString(r["description"]);
            p = new Park(parkID, name, location, establishDate, area, visitor, description);

            return p;
        }
    }
}
