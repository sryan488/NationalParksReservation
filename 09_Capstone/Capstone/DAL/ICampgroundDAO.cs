using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        /// <summary>
        /// A method for getting all of the campgrounds
        /// </summary>
        /// <returns>A list of all of the campgrounds</returns>
        IList<Campground> GetAllCampgrounds();
        /// <summary>
        /// A method for getting all of the campgrounds in a park
        /// </summary>
        /// <param name="park">The park to get campgrounds in</param>
        /// <returns>A list of all campgrounds in the specified park</returns>
        IList<Campground> GetAllCampgrounds(Park park);
        /// <summary>
        /// A method for getting all campgrounds that are open at a specific time
        /// </summary>
        /// <param name="time">The time to check if a campground is open at</param>
        /// <returns>A list of all campgrounds open at the specified time</returns>
        IList<Campground> GetAllOpenCampgrounds(DateTime time);
        /// <summary>
        /// A method for getting the campground with the specific ID
        /// </summary>
        /// <param name="campgroundID">The ID of the campground</param>
        /// <returns>The Campground with the specified ID</returns>
        Campground GetCampgroundByID(int campgroundID);
        //TODO figure out what other methods we may want in our interface
    }
}
