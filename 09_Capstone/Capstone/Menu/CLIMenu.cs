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
                //DONE make this prettier
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
                Console.Clear();
                //Console.ReadLine();
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
                Console.WriteLine("\nSelect a Command\n\t1) View Campgrounds");
                //Console.WriteLine("\t2) Search for Reservation (not implemented, will throw exception)");
                Console.WriteLine("\t0) Return to Previous Screen");
                Console.WriteLine();
                int userSelection = GetValidSelection(1);
                if (userSelection == 0)
                {
                    return;
                }
                if (userSelection == 1)
                {
                    RunCampgroundMenu(park);
                }
                if (userSelection == 2)//it won't be
                {
                    RunReservationSearchMenu(park);
                }
            }
        }

        private void RunReservationSearchMenu(Park park)
        {
            throw new NotImplementedException();
        }

        private void RunCampgroundMenu(Park park)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine($"{park.Name} National Park Campgrounds");
                Console.WriteLine();//DONE make this menu look nice
                IList<Campground> campgrounds = campDAO.GetAllCampgrounds(park);
                WriteCampgroundsList(campgrounds);
                Console.WriteLine("\nSelect a Command\n\t1) Search for Available Reservation\n\t0) Return to Previous Screen");
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
            Console.Clear();
            WriteCampgroundsList(campgrounds);
            Console.Write("\nSelect a Campground to make a reservation or 0 to go back: ");
            IList<Site> sites;
            Campground campground;
            DateTime userArrival;
            DateTime userDeparture;
            int numberOfDaysToStay;
            int userSelection = GetValidSelection(campgrounds.Count);
            if (userSelection == 0)
            {
                return;
            }
            userSelection--;
            campground = campgrounds[userSelection];
            while (true)
            {
                Console.Write("What is the arrival date (MM/DD/YYYY)? ");
                userArrival = GetValidDate();
                Console.Write("What is the departure date (MM/DD/YYYY)? ");
                userDeparture = GetValidDate();
                while (userDeparture.CompareTo(userArrival) <= 0)//you have reserve at least one day
                {
                    Console.Write("No, really.... What is the departure date (MM/DD/YYYY)? ");
                    userDeparture = GetValidDate();
                }
                
                //DONE do logic to search for available campsites during their dates
                //we did the logic in a SQL statement in the SiteDAO
                Console.Clear();
                
               
                sites = siteDAO.GetAvailableSites(userArrival, userDeparture, campground);
                //DONE figure out how to figure out if their dates occur in the campground's offseason and if so, remove all the sites
                /*
                 *  If their start month is less than their end month, things are normal so we.......
                 *  If their start year is not the same as their end year, then they're camping over the new year and it's kinda odd how we figure out if they're okay to camp
                 *      -if the campground is not open in december and january..... then they can't stay there
                 *      -if their start month is less than their end month, then they're trying to camp for over a full year so the park needs to be open year round if they want that reservation
                 *      
                 */
                if (userArrival.Year == userDeparture.Year)
                {
                    if (!(userArrival.Month >= campground.OpenFrom && userArrival.Month <= campground.OpenTo && userDeparture.Month >= campground.OpenFrom && userDeparture.Month <= campground.OpenTo))
                    {
                        //in this case, they're NOT safe to keep going
                        sites.Clear();
                    }
                }
                else
                {
                    //in this case, they're camping over the new year and things get weiiiiird
                    if(campground.OpenFrom != 1 && campground.OpenTo != 12)
                    {
                        sites.Clear();
                    }
                }
                if (sites.Count == 0)
                {
                    Console.WriteLine("There were no available campsites for the given time frame. \nWould you like to enter a different set of dates(Y/N)?");
                    string userYorN = Console.ReadLine().ToLower().Trim();
                    if(userYorN.StartsWith("y"))
                    {
                        continue;//continue with the loop, which is to say, go back and ask for dates again
                    }

                    Console.WriteLine("Returning to Campground Menu...");
                    Thread.Sleep(3000);
                    return;
                }
                else
                {
                    break;//if we get here, then they gave us dates with available campsites and we're breaking the loop to continue with reserving
                    //this should happen MOST of the time
                }
            }
            numberOfDaysToStay = ((TimeSpan)(userDeparture - userArrival)).Days;
            Console.WriteLine("Results Matching Your Search Criteria");
            WriteSiteList(sites);
            Console.WriteLine($"\tThe cost will be {campground.DailyFee * numberOfDaysToStay:c}");
            Console.Write($"\nWhich site would you like to reserve (enter 0 to cancel)? ");
            int userSiteToReserve = GetValidSiteSelection(sites);
            if(userSiteToReserve == 0)
            {
                return;
            }
            // DONE make sure they enter a valid name for the reservation
            Console.Write("What name should the reservation be made under? ");
            string userName = GetValidName();
            //DONE do logic to find the site ID
            int siteID = -1;
            foreach(Site site in sites)
            {
                if(userSiteToReserve == site.SiteNumber)
                {
                    siteID = site.SiteID;
                }
            }
            //Console.WriteLine(siteID);
            DateTime currentTime = DateTime.Now;
            int reservationID = reservationDAO.CreateReservation(new Reservation() { SiteID = siteID, Name = userName, FromDate = userArrival, ToDate = userDeparture, CreateDate = currentTime });
            //DONE write out their reservation info here
            Console.WriteLine("Final Reservation Info:\n");
            Console.WriteLine($"\tThe site ID is {siteID}");
            Console.WriteLine($"\tThe reservation is under {userName}");
            Console.WriteLine($"\tYou are staying from {userArrival:d} to {userDeparture:d}");
            Console.WriteLine($"\tThe reservation creation time is {currentTime}");
            Console.WriteLine($"\tThe reservation has been made and the confirmation ID is {reservationID}");

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
                    Console.Write($"That was not a valid selection. Select a number between 0 and {max}: ");
                }
            }
            
            return selection;
        }
        private string GetValidName()
        {
            string name = Console.ReadLine().Trim();
            if(name.Length == 0)
            {
                Console.Write("Your name must be at least 1 character long: ");
                return GetValidName();
            }
            return name;
        }
        private void WriteCampgroundsList(IList<Campground> campgrounds)
        {
            //DONE add the header list for this
            Console.WriteLine($"{"",-6}{"Name",-34}{"Open",-12}{"Close",-12}{"Daily Fee",-12}");//DONE make sure this looks fine
            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine($"#{i + 1, -5}{campgrounds[i].Name,-34}{months[campgrounds[i].OpenFrom],-12}{months[campgrounds[i].OpenTo],-12}{campgrounds[i].DailyFee,-12:c}");
            }
        }

        private void WriteSiteList(IList<Site> sites)
        {
            Console.WriteLine($"{"Site No.",-10}{"Max Occup.",-12}{"Accessible?",-13}{"Max RV Length",-16}{"Utility",-10}");//DONE make sure this looks fine
            foreach(Site site in sites)
            {
                Console.WriteLine($"{site.SiteNumber,-10}{site.MaxOccupancy,-12}{(site.HandicapAccess ? "Yes" : "No" ), -13}{((site.MaxRVLength == 0)? @"N/A" : Convert.ToString(site.MaxRVLength)),-16}{(site.HasUtilities ? "Yes" : "N/A"),-10}");
            }
        }

        private DateTime GetValidDate()
        {
            while(true)
            {
                if(DateTime.TryParse(Console.ReadLine(), out DateTime userTime) && userTime.Date.CompareTo(DateTime.Now.Date) >= 0)
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
