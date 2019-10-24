using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;

        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public int CreateReservation(Reservation newReservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date, create_date) VALUES (@sid, @n, @fd, @td, @cd); SELECT @@IDENTITY", conn);
                    cmd.Parameters.AddWithValue("@sid", newReservation.SiteID);
                    cmd.Parameters.AddWithValue("@n", newReservation.Name);
                    cmd.Parameters.AddWithValue("@fd", newReservation.FromDate);
                    cmd.Parameters.AddWithValue("@td", newReservation.ToDate);
                    cmd.Parameters.AddWithValue("@cd", newReservation.CreateDate);
                    //Execute the command
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public IList<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM reservation ORDER BY site_id ASC, name ASC", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(RowToReservation(reader));
                }
            }
            return reservations;
        }

        public IList<Reservation> GetAllReservations(Site campsite)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM reservation WHERE site_id = @sid ORDER BY site_id ASC, name ASC", conn);
                cmd.Parameters.AddWithValue("@sid", campsite.SiteID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(RowToReservation(reader));
                }
            }

            return reservations;
        }
        public Reservation GetReservationByID(int reservationID)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM reservation WHERE reservation_id = @rid", conn);
                cmd.Parameters.AddWithValue("@rid", reservationID);

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    return RowToReservation(r);
                }
            }
            return null;

        }
        private Reservation RowToReservation(SqlDataReader reader)
        {
            Reservation r;
            int reservationID = Convert.ToInt32(reader["reservation_id"]);
            int siteID = Convert.ToInt32(reader["site_id"]);
            string name = Convert.ToString(reader["name"]);
            DateTime fromDate = Convert.ToDateTime(reader["from_date"]);
            DateTime toDate = Convert.ToDateTime(reader["to_date"]);
            DateTime createDate = Convert.ToDateTime(reader["create_date"]);
            r = new Reservation(reservationID, siteID, name, fromDate, toDate,createDate);

            return r;
        }
    }
}
