using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    // this makes this sitesqldao class inherit the interface we created
    public class SiteSqlDAO : ISiteDAO
    {
        // this is a string that will hold the connection we need to create to communicate with the datebase
        private string connectionString;

        public SiteSqlDAO(string dbConnectionString)
        {
            // assigns the data base connection string to connectionString
            connectionString = dbConnectionString;
        }

        public IList<Site> GetAllHandicapAccessableSites(bool getAccessable)
        {
            // creates a new list of sites to fill with info from the sql database table sites
            List<Site> sites = new List<Site>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // using the conection we created, opens it
                conn.Open();

                // this is a new sql command getting all hte handicap accessible sites (true/false is it accessible?)
                SqlCommand cmd = new SqlCommand("SELECT * FROM site WHERE accessible = 1 ORDER BY site_number ASC", conn);
                // reads the data and to r
                SqlDataReader r = cmd.ExecuteReader();

                // while the data is read, add the data to the sites list fed by r
                while (r.Read())
                {
                    // uses the RTS method so we don't have to type out the info everytime
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
            //DONE implement this // Tested it, it works :)
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
