using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    // these are the interfaces that serve as a middle man to communicate with the sql database
    // Database Access Layers
    public interface IParkDAO
    {
        /// <summary>
        /// A method for getting a list of all the parks ordered alphabetically
        /// </summary>
        /// <returns>A list of all Parks</returns>
        IList<Park> GetAllParks();
        /// <summary>
        /// A method for getting the park with a specific ID
        /// </summary>
        /// <param name="parkID">The ID of the park to get</param>
        /// <returns>The Park with the corresponding ID or null if the ID is invalid</returns>
        Park GetParkByID(int parkID);
        //DONE figure out what other access methods we want in our interface
    }
}
