﻿using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    // these are the interfaces that serve as a middle man to communicate with the sql database
    // Database Access Layers
    public interface IReservationDAO
    {
        /// <summary>
        /// A method for getting a list of all reservations
        /// </summary>
        /// <returns>A list of all reservations</returns>
        IList<Reservation> GetAllReservations();
        /// <summary>
        /// A method for getting a lsit of reservations for a specific campsite
        /// </summary>
        /// <param name="campsite">The campsite to get reservations of</param>
        /// <returns>A list of all reservations at the specified campsite</returns>
        IList<Reservation> GetAllReservations(Site campsite);
        /// <summary>
        /// A method for getting the reservation with the specified ID
        /// </summary>
        /// <param name="reservationID">The ID of the reservation to get</param>
        /// <returns>The reservation with the specified ID</returns>
        Reservation GetReservationByID(int reservationID);
        /// <summary>
        /// A method for creating a new reservation
        /// </summary>
        /// <param name="newReservation">The reservation to be added</param>
        /// <returns>The reservation ID</returns>
        int CreateReservation(Reservation newReservation);
    }
}
