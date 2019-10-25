using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    // these are the C# "models" that represent the tables in the sql database
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int SiteID { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }
        public Reservation() { }
        public Reservation(int reservationID, int siteID, string name, DateTime fromDate, DateTime toDate, DateTime createDate)
        {
            ReservationID = reservationID;
            SiteID = siteID;
            Name = name;
            FromDate = fromDate;
            ToDate = toDate;
            CreateDate = createDate;
        }
        //NOPE Maybe make a ToString() for all of these model classes?
    }
}
