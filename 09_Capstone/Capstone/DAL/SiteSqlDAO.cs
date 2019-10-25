using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;

        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetAllHandicapAccessableSites(bool getAccessable)
        {
            List<Site> sites = new List<Site>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM site WHERE accessible = 1 ORDER BY site_number ASC", conn);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    sites.Add(RowToSite(r));
                }

            }

            return sites;
        }

        public IList<Site> GetAllSites()
        {
            List<Site> sites = new List<Site>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM site ORDER BY site_number ASC", conn);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    sites.Add(RowToSite(r));
                }
            }
            return sites;
        }

        public IList<Site> GetAllSites(Campground campground)
        {
            List<Site> sites = new List<Site>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM site WHERE campground_id = @cid ORDER BY site_number ASC", conn);
                cmd.Parameters.AddWithValue("@cid", campground.CampgroundID);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    sites.Add(RowToSite(r));
                }
            }

            return sites;
        }

        public IList<Site> GetAvailableSites(DateTime arrivalDate, DateTime departureDate, Campground campground)
        {
            //TODO implement this //IDK if this will work at all but i hope it does :)
            List<Site> sites = new List<Site>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //get all sites in the specified campsite and then remove any sites that have conflicting dates
                SqlCommand cmd = new SqlCommand(@"SELECT  DISTINCT TOP 5 site.* FROM site WHERE site.campground_id = @cid AND site.site_id NOT IN (SELECT site_id FROM reservation WHERE (to_date > @fromdate AND from_date < @todate)) ORDER BY site_number", conn);
                cmd.Parameters.AddWithValue("@cid", campground.CampgroundID);
                cmd.Parameters.AddWithValue("@todate", departureDate);
                cmd.Parameters.AddWithValue("@fromdate", arrivalDate);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    sites.Add(RowToSite(r));
                }
            }

            return sites;
        }

        public Site GetSiteByID(int siteID)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM site WHERE site_id = @sid", conn);
                cmd.Parameters.AddWithValue("@sid", siteID);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    return RowToSite(r);
                }
            }
            return null;

        }
        private Site RowToSite(SqlDataReader r)
        {
            Site s;
            int siteID = Convert.ToInt32(r["site_id"]);
            int campgroundID = Convert.ToInt32(r["campground_id"]);
            int siteNumber = Convert.ToInt32(r["site_number"]);
            int maxOccupancy = Convert.ToInt32(r["max_occupancy"]);
            bool handicapAccessible = Convert.ToBoolean(r["accessible"]);
            int maxRVLength = Convert.ToInt32(r["max_rv_length"]);
            bool hasUtilities = Convert.ToBoolean(r["utilities"]);
            s = new Site(siteID, campgroundID, siteNumber, maxOccupancy, handicapAccessible, maxRVLength, hasUtilities);

            return s;
        }
    }
}
