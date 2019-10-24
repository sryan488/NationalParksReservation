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
    public class ParkDAOTests
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
        public void GetAllParksTest()
        {
            // Arrange
            IList<Park> parks;
            IParkDAO dao = new ParkSqlDAO(connection);

            // Act
            parks = dao.GetAllParks();

            // Assert
            Assert.AreEqual(3, parks.Count);
        }
        [TestMethod]
        public void GetParkByIDTest()
        {
            // Arrange
            IParkDAO dao = new ParkSqlDAO(connection);

            // Act
            Park p = dao.GetParkByID(1);

            // Assert
            Assert.AreEqual("Acadia", p.Name);
        }
    }
}
