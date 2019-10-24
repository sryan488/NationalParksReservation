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
    public class CampgroundDAOTests
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
        public void GetAllCampgroundsTest()
        {
            // Arrange
            IList<Campground> cgrounds;
            ICampgroundDAO dao = new CampgroundSqlDAO(connection);

            // Act
            cgrounds = dao.GetAllCampgrounds();

            // Assert
            Assert.AreEqual(7, cgrounds.Count);
        }
        [TestMethod]
        public void GetAllCampgroundsInParkTest()
        {
            // Arrange
            IList<Campground> cgrounds;
            Park p = new Park()
            {
                ParkID = 1
            };
            ICampgroundDAO dao = new CampgroundSqlDAO(connection);

            // Act
            cgrounds = dao.GetAllCampgrounds(p);

            // Assert
            Assert.AreEqual(3, cgrounds.Count);
        }
        [TestMethod]
        public void GetAllOpenCampgroundsTest()
        {
            // Arrange
            IList<Campground> cgrounds;
            
            ICampgroundDAO dao = new CampgroundSqlDAO(connection);

            // Act
            cgrounds = dao.GetAllOpenCampgrounds(3);

            // Assert
            Assert.AreEqual(4, cgrounds.Count);
        }
        [TestMethod]
        public void GetCampgroundByIDTest()
        {
            // Arrange
            ICampgroundDAO dao = new CampgroundSqlDAO(connection);

            // Act
            Campground c = dao.GetCampgroundByID(4);

            // Assert
            Assert.AreEqual("Devil's Garden", c.Name);
            Assert.IsNull(dao.GetCampgroundByID(9));
        }
    }
}