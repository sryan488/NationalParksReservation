using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Menu
{
    public class CLIMenu
    {
        private IParkDAO parkDAO;
        private ICampgroundDAO campDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        static Dictionary<int, string> months = new Dictionary<int, string>()
        {
            { 1,"January" },
            { 2, "February" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" }
        };

        public CLIMenu (string connectionString)
        {
            parkDAO = new ParkSqlDAO(connectionString);
            campDAO = new CampgroundSqlDAO(connectionString);
            siteDAO = new SiteSqlDAO(connectionString);
            reservationDAO = new ReservationSqlDAO(connectionString);
        }
        public void Run()
        {
            while(true)
            {
                IList<Park> parks = parkDAO.GetAllParks();
                //TODO make this prettier
                Console.WriteLine("Select a park for further details:");
                for (int i = 1; i <= parks.Count; i++)
                {
                    Console.WriteLine($"\t{i}) {parks[i - 1].Name}");
                }
                Console.WriteLine("\t0) Exit");
                int userSelection = GetValidSelection(parks.Count);
                if (userSelection == 0)
                {
                    Console.WriteLine("Thank you, come again :)");
                    Thread.Sleep(3000);
                    return;
                }
                userSelection--;
                RunParkInfoMenu(parks[userSelection]);
                Console.ReadLine();
            }
        }

        private void RunParkInfoMenu(Park park)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine($"{park.Name} National Park");
                Console.WriteLine($"Location:\t\t{park.Location}");
                Console.WriteLine($"Established:\t\t{park.EstablishDate:d}");
                Console.WriteLine($"Area:\t\t\t{park.Area:##,#} sq km");
                Console.WriteLine($"Annual Visitors:\t{park.Visitor:##,#}");
                Console.WriteLine($"\n{park.Description}");
                Console.WriteLine($"\nSelect a Command\n\t1) View Campgrounds\n\t2) Search for Reservation\n\t0) Return to Previous Screen");
                int userSelection = GetValidSelection(2);
                if (userSelection == 0)
                {
                    return;
                }
                if (userSelection == 1)
                {
                    RunCampgroundMenu(park);
                }
                if (userSelection == 2)
                {
                    RunReservationSearchMenu(park);
                }
            }
        }

        private void RunReservationSearchMenu(Park park)
        {
            throw new NotImplementedException();//TODO implement this if we have time
        }

        private void RunCampgroundMenu(Park park)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine($"{park.Name} National Park Campgrounds");
                Console.WriteLine("\n");//TODO make this menu look nice
                IList<Campground> campgrounds = campDAO.GetAllCampgrounds(park);
                WriteCampgroundsList(campgrounds);
                Console.WriteLine("Select a Command\n\t1) Search for Available Reservation\n\t0) Return to Previous Screen");
                int userSelection = GetValidSelection(1);
                if(userSelection == 0)
                {
                    return;
                }
                else
                {
                    RunAvailableReservationSearchMenu(campgrounds);
                }
            }
        }

        private void RunAvailableReservationSearchMenu(IList<Campground> campgrounds)
        {
            WriteCampgroundsList(campgrounds);
            Console.Write("\nSelect a Campground to make a reservation or 0 to go back: ");

            int userSelection = GetValidSelection(campgrounds.Count);
            if (userSelection == 0)
            {
                return;
            }
            Console.Write("What is the arrival date (MM/DD/YYYY)? ");
            DateTime userArrival = GetValidDate();
            Console.Write("What is the departure date (MM/DD/YYYY)? ");
            DateTime userDeparture = GetValidDate();
            while(userDeparture.CompareTo(userArrival) <= 0)//you have reserve at least one day
            {
                Console.Write("No, really.... What is the departure date (MM/DD/YYYY)? ");
                userDeparture = GetValidDate();
            }
            userSelection--;
            //TODO do logic to search for available campsites during their dates
            //we did the logic in a SQL statement in the SiteDAO
            Console.Clear();
            Campground campground = campgrounds[userSelection];
            int numberOfDaysToStay = ((TimeSpan)(userDeparture - userArrival)).Days;
            IList<Site> sites = siteDAO.GetAvailableSites(userArrival, userDeparture, campground);
            WriteSiteList(sites);
            Console.WriteLine($"\tThe cost will be {campground.DailyFee * numberOfDaysToStay:c}");
            Console.Write($"\nWhich site would you like to reserve? ");
            int userSiteToReserve = GetValidSiteSelection(sites);
            if(userSiteToReserve == 0)
            {
                return;
            }
            Console.Write("What name should the reservation be made under?");
            string userName = Console.ReadLine();
            //TODO do logic to find the site ID
            int siteID = -1;
            foreach(Site site in sites)
            {
                if(userSiteToReserve == site.SiteNumber)
                {
                    siteID = site.SiteID;
                }
            }
            //Console.WriteLine(siteID);
            int reservationID = reservationDAO.CreateReservation(new Reservation() { SiteID = siteID, Name = userName, FromDate = userArrival, ToDate = userDeparture, CreateDate = DateTime.Now });
            Console.WriteLine($"The reservation has been made and the confirmation id is {reservationID}");

            Console.ReadLine();
            
        }

        private int GetValidSiteSelection(IList<Site> sites)
        {
            int selection = -1;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int sel) && sel >= 0)
                {
                    selection = sel;
                    foreach(Site site in sites)
                    {
                        if(selection == site.SiteNumber)
                        {
                            return selection;
                        }
                    }
                }
                Console.Write($"That was not a valid selection. Select a valid site number (or 0 to exit):");
            }
        }

        private int GetValidSelection(int max)
        {
            int selection = -1;
            while(true)
            {
                if (int.TryParse(Console.ReadLine(), out int sel) && sel >= 0 && sel <= max)
                {
                    selection = sel;
                    break;
                }
                else
                {
                    Console.Write($"That was not a valid selection. Select a number between 0 and {max}:");
                }
            }
            
            return selection;
        }
        private void WriteCampgroundsList(IList<Campground> campgrounds)
        {
            //TODO add the header list for this
            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine($"#{i + 1} {campgrounds[i].Name} {months[campgrounds[i].OpenFrom]} {months[campgrounds[i].OpenTo]} {campgrounds[i].DailyFee:c}");
            }
        }

        private void WriteSiteList(IList<Site> sites)
        {
            foreach(Site site in sites)
            {
                Console.WriteLine($"{site.SiteNumber,-10} {site.MaxOccupancy,-10} {(site.HandicapAccess ? "Yes" : "No" ), -10} {site.MaxRVLength,-10} {(site.HasUtilities ? "Yes" : "N/A"),-10}");
            }
        }

        private DateTime GetValidDate()
        {
            while(true)
            {
                if(DateTime.TryParse(Console.ReadLine(), out DateTime userTime))
                {
                    return userTime.Date;
                }
                else
                {
                    Console.Write("That was not a valid Date, try MM/DD/YYYY: ");
                }
            }
        }
    }
}
