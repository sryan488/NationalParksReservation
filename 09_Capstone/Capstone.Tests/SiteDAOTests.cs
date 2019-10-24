using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
