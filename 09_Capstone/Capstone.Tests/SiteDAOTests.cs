using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class SiteDAOTests
    {

        private TransactionScope transaction;
        const string connection = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";

        [TestInitialize]
        public void TestInitialize()
        {
            this.transaction = new TransactionScope();
            string script;
            using (StreamReader sr = new StreamReader("SetUp.sql"))
            {
                script = sr.ReadToEnd();
            }
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(script, conn);

                SqlDataReader rdr = cmd.ExecuteReader();
            }
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void GetAllSitesTest()
        {
            // Arrange
            IList<Site> sites;
            ISiteDAO dao = new SiteSqlDAO(connection);

            // Act
            sites = dao.GetAllSites();

            // Assert
            Assert.AreEqual(51, sites.Count);
        }
        [TestMethod]
        public void GetAllSitesInCampgroundTest()
        {
            // Arrange
            IList<Site> sites;
            Campground c = new Campground()
            {
                CampgroundID = 7
            };
            ISiteDAO dao = new SiteSqlDAO(connection);

            // Act
            sites = dao.GetAllSites(c);

            // Assert
            Assert.AreEqual(5, sites.Count);
        }
        [TestMethod]
        public void GetSiteByIDTest()
        {
            // Arrange
            ISiteDAO dao = new SiteSqlDAO(connection);

            // Act
            Site s = dao.GetSiteByID(49);

            // Assert
            Assert.AreEqual(3, s.SiteNumber);
            Assert.IsNull(dao.GetSiteByID(302));
        }
        [TestMethod]
        public void GetAllHandicapAccessibleSitesTest()
        {
            ISiteDAO dao = new SiteSqlDAO(connection);

            IList<Site> sites = dao.GetAllHandicapAccessableSites(true);

            Assert.AreEqual(18, sites.Count);
            foreach(Site s in sites)
            {
                Assert.IsTrue(s.HandicapAccess);
            }
        }
        [TestMethod]
        public void GetAllAvailableCampSitesTest()
        {
            //campground ID = 7 has 5 campsites and there are no reservations in them
            //the site IDs for campground 7 are 47 to 51
            //Arrange
            ISiteDAO dao = new SiteSqlDAO(connection);
            Campground camp = new Campground()
            {
                CampgroundID = 7
            };
            DateTime timeToCheck1 = Convert.ToDateTime("10/05/2020");
            DateTime timeToCheck2 = Convert.ToDateTime("10/10/2020");
            DateTime timeToCheckAgainst1a = Convert.ToDateTime("10/01/2020");
            DateTime timeToCheckAgainst1b = Convert.ToDateTime("10/05/2020");
            DateTime timeToCheckAgainst2a = Convert.ToDateTime("10/01/2020");
            DateTime timeToCheckAgainst2b = Convert.ToDateTime("10/06/2020");
            DateTime timeToCheckAgainst3a = Convert.ToDateTime("10/06/2020");
            DateTime timeToCheckAgainst3b = Convert.ToDateTime("10/09/2020");
            DateTime timeToCheckAgainst4a = Convert.ToDateTime("10/09/2020");
            DateTime timeToCheckAgainst4b = Convert.ToDateTime("10/11/2020");
            DateTime timeToCheckAgainst5a = Convert.ToDateTime("10/11/2020");
            DateTime timeToCheckAgainst5b = Convert.ToDateTime("10/20/2020");
            DateTime timeToCheckAgainst6a = Convert.ToDateTime("10/01/2020");
            DateTime timeToCheckAgainst6b = Convert.ToDateTime("10/20/2020");


            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string SQLcmd = @"INSERT reservation
(site_id, name, from_date, to_date)
VALUES
(47, 'Thomas Family', @fd, @td)";
                SqlCommand cmd = new SqlCommand(SQLcmd, conn);
                cmd.Parameters.AddWithValue("@fd", timeToCheckAgainst1a);
                cmd.Parameters.AddWithValue("@td", timeToCheckAgainst1b);
                cmd.ExecuteNonQuery();
                SQLcmd = @"INSERT reservation
(site_id, name, from_date, to_date)
VALUES
(47, 'Thomas Family', @fd, @td)";
                cmd = new SqlCommand(SQLcmd, conn);
                cmd.Parameters.AddWithValue("@fd", timeToCheckAgainst2a);
                cmd.Parameters.AddWithValue("@td", timeToCheckAgainst2b);
                cmd.ExecuteNonQuery();
                SQLcmd = @"INSERT reservation
(site_id, name, from_date, to_date)
VALUES
(48, 'Thomas Family', @fd, @td)";
                cmd = new SqlCommand(SQLcmd, conn);
                cmd.Parameters.AddWithValue("@fd", timeToCheckAgainst3a);
                cmd.Parameters.AddWithValue("@td", timeToCheckAgainst3b);
                cmd.ExecuteNonQuery();
                SQLcmd = @"INSERT reservation
(site_id, name, from_date, to_date)
VALUES
(49, 'Thomas Family', @fd, @td)";
                cmd = new SqlCommand(SQLcmd, conn);
                cmd.Parameters.AddWithValue("@fd", timeToCheckAgainst4a);
                cmd.Parameters.AddWithValue("@td", timeToCheckAgainst4b);
                cmd.ExecuteNonQuery();
                SQLcmd = @"INSERT reservation
(site_id, name, from_date, to_date)
VALUES
(50, 'Thomas Family', @fd, @td)";
                cmd = new SqlCommand(SQLcmd, conn);
                cmd.Parameters.AddWithValue("@fd", timeToCheckAgainst5a);
                cmd.Parameters.AddWithValue("@td", timeToCheckAgainst5b);
                cmd.ExecuteNonQuery();
                SQLcmd = @"INSERT reservation
(site_id, name, from_date, to_date)
VALUES
(47, 'Thomas Family', @fd, @td)";
                cmd = new SqlCommand(SQLcmd, conn);
                cmd.Parameters.AddWithValue("@fd", timeToCheckAgainst6a);
                cmd.Parameters.AddWithValue("@td", timeToCheckAgainst6b);
                cmd.ExecuteNonQuery();
            }
            //Act
            IList<Site> sites = dao.GetAvailableSites(timeToCheck1, timeToCheck2, camp);

            //Assert
            Assert.AreEqual(2, sites.Count);//it should return only site with ID 50 and 51, because 50 doesn't have conflicting reervation dates, and 51 has no reservation dates
            Assert.AreEqual(50, sites[0].SiteID);//the first in the list should be ID 50
            Assert.AreEqual(51, sites[1].SiteID);//the second in the list should be ID 51
        }
    }
}
