using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Campground> GetAllCampgrounds()
        {
            List<Campground> campgrounds = new List<Campground>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM campground ORDER BY name ASC", conn);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    campgrounds.Add(RowToCampground(r));
                }
            }

            return campgrounds;
        }

        public IList<Campground> GetAllCampgrounds(Park park)
        {
            List<Campground> campgrounds = new List<Campground>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @pid ORDER BY name ASC", conn);
                cmd.Parameters.AddWithValue("@pid", park.ParkID);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    campgrounds.Add(RowToCampground(r));
                }
            }

            return campgrounds;
        }

        public IList<Campground> GetAllOpenCampgrounds(int time)
        {
            List<Campground> campgrounds = new List<Campground>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE open_from_mm <= @date AND open_to_mm >= @date ORDER BY name ASC", conn);
                cmd.Parameters.AddWithValue("@date", time);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    campgrounds.Add(RowToCampground(r));
                }
            }

            return campgrounds;
        }

        public Campground GetCampgroundByID(int campgroundID)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE campground_id = @cid", conn);
                cmd.Parameters.AddWithValue("@cid", campgroundID);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                   return RowToCampground(r);
                }
            }
            return null;

        }
        private Campground RowToCampground(SqlDataReader r)
        {
            Campground c;
            int campgroundID = Convert.ToInt32(r["campground_id"]);
            int parkID = Convert.ToInt32(r["park_id"]);
            string name = Convert.ToString(r["name"]);
            int openFrom = Convert.ToInt32(r["open_from_mm"]);
            int openTo = Convert.ToInt32(r["open_to_mm"]);
            decimal dailyFee = Convert.ToDecimal(r["daily_fee"]);
            c = new Campground(campgroundID, parkID, name, openFrom, openTo, dailyFee);

            return c;
        }
    }
}
