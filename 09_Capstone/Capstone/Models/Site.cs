using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool HandicapAccess { get; set; }
        public int MaxRVLength { get; set; }
        public bool HasUtilities { get; set; }

        public Site() { }
        public Site(int siteID, int campgroundID, int siteNumber, int maxOccupancy, bool handicapAccess, int maxRVLength, bool hasUtilities)
        {
            SiteID = siteID;
            CampgroundID = campgroundID;
            SiteNumber = siteNumber;
            MaxOccupancy = maxOccupancy;
            HandicapAccess = handicapAccess;
            MaxRVLength = maxRVLength;
            HasUtilities = hasUtilities;
        }
        //NOPE Maybe make a ToString() for all of these model classes?
    }
}
