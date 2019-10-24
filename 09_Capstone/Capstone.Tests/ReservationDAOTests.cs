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
    public class ReservationDAOTests
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
        public void GetAllReservationsTest()
        {
            // Arrange
            IList<Reservation> reservations;
            IReservationDAO dao = new ReservationSqlDAO(connection);

            // Act
            reservations = dao.GetAllReservations();

            // Assert
            Assert.AreEqual(44, reservations.Count);
        }
        [TestMethod]
        public void GetAllReservationsInSiteTest()
        {
            // Arrange
            IList<Reservation> reservations;
            Site s = new Site()
            {
                SiteID = 20
            };
            IReservationDAO dao = new ReservationSqlDAO(connection);

            // Act
            reservations = dao.GetAllReservations(s);

            // Assert
            Assert.AreEqual(4, reservations.Count);
        }
        [TestMethod]
        public void GetReservationByIDTest()
        {
            // Arrange
            IReservationDAO dao = new ReservationSqlDAO(connection);

            // Act
            Reservation r = dao.GetReservationByID(4);

            // Assert
            Assert.AreEqual("Bauer Family Reservation", r.Name);
            Assert.IsNull(dao.GetReservationByID(50));
        }
        [TestMethod]
        public void CreateReservationTest()
        {
            // Arrange
            IReservationDAO dao = new ReservationSqlDAO(connection);

            // Act
            Reservation r = new Reservation()
            {
                SiteID = 2,
                Name = "Ryan Family Reservation",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CreateDate = DateTime.Now
            };
            Reservation r2 = new Reservation()
            {
                SiteID = 3,
                Name = "Bauer Family Reservation",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CreateDate = DateTime.Now
            };
            int thisOneIs45 = dao.CreateReservation(r);
            int thisOneIs46 = dao.CreateReservation(r2);

            // Assert
            Assert.AreEqual(45, thisOneIs45);
            Assert.AreEqual(46, thisOneIs46);
            Assert.AreEqual(46, dao.GetAllReservations().Count);
        }
    }
}
