using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundID { get; set; }
        public int ParkID { get; set; }
        public string Name { get; set; }
        public int OpenFrom { get; set; }
        public int OpenTo { get; set; }
        public decimal DailyFee { get; set; }

        public Campground() { }
        public Campground(int campgroundID, int parkID, string name, int openFrom, int openTo, decimal dailyFee)
        {
            CampgroundID = campgroundID;
            ParkID = parkID;
            Name = name;
            OpenFrom = openFrom;
            OpenTo = openTo;
            DailyFee = dailyFee;
        }
        //TODO Maybe make a ToString() for all of these model classes?
    }
}
