using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int Visitor { get; set; }
        public string Description { get; set; }

        public Park() { }
        public Park(int parkID, string name, string location, DateTime establishDate, int area, int visitor, string description)
        {
            ParkID = parkID;
            Name = name;
            Location = location;
            EstablishDate = establishDate;
            Area = area;
            Visitor = visitor;
            Description = description;
        }
        //NOPE Maybe make a ToString() for all of these model classes?
    }
}
