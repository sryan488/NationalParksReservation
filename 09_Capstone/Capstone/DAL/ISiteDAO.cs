using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    // these are the interfaces that serve as a middle man to communicate with the sql database
    // Database Access Layers
    public interface ISiteDAO
    {
        /// <summary>
        /// A method for getting all of the campsites
        /// </summary>
        /// <returns>A list of all of the campsites</returns>
        IList<Site> GetAllSites();
        /// <summary>
        /// A method for getting all the campsites at a specific campground
        /// </summary>
        /// <param name="campground">The campground to get campsites from</param>
        /// <returns>A list of all campsites at the specified campground</returns>
        IList<Site> GetAllSites(Campground campground);
        /// <summary>
        /// A method for getting a list of all handicap accessable campsites if true or a list of all campsites if false
        /// </summary>
        /// <param name="getAccessable">true if only handicap accessable, false if all campsites</param>
        /// <returns>A list of campsites</returns>
        IList<Site> GetAllHandicapAccessableSites(bool getAccessable);
        /// <summary>
        /// A method for getting a campsite with the specified ID
        /// </summary>
        /// <param name="siteID">The ID of the campsite to get</param>
        /// <returns>The campsite with the specified ID or null if the ID is invalid</returns>
        Site GetSiteByID(int siteID);
        /// <summary>
        /// A method for getting a list of sites that are available between the dates at the campground given
        /// </summary>
        /// <param name="arrivalDate">date to arrive</param>
        /// <param name="departureDate">date to depart</param>
        /// <param name="campground">campground to check</param>
        /// <returns>A list of the available sites in the given time frame</returns>
        IList<Site> GetAvailableSites(DateTime arrivalDate, DateTime departureDate, Campground campground);
        //DONE figure out if we want any other methods in our interface
    }
}
