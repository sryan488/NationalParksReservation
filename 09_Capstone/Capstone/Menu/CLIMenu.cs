using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Menu
{
    // created the climenu class (Command Line Interface Menu)
    public class CLIMenu
    {
        // Properties that can only be used in this class because they are set to private
        private IParkDAO parkDAO;
        private ICampgroundDAO campDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        // created a dictionary so that we could turn ints into string month name
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
        
        //This is our constructor (reognize by having the same name as the class) and the string is the only argument that it accepts
        public CLIMenu (string connectionString)
        {
            // Initializing our properties to be used through the class (they need the connection string to conect to the database)
            parkDAO = new ParkSqlDAO(connectionString);
            campDAO = new CampgroundSqlDAO(connectionString);
            siteDAO = new SiteSqlDAO(connectionString);
            reservationDAO = new ReservationSqlDAO(connectionString);
        }
        //this is the main mnu running
        public void Run()
        { //While running, if this is still true, execute (will always be tue because we said so)
            while(true)
            {
                // Ilist is the interface list and parks is the parks table from sql as a c# list
                // this returns a list that is the table from sql uing the get all parks method we created in parksqldao
                IList<Park> parks = parkDAO.GetAllParks();
                //DONE make this prettier
                Console.WriteLine("Select a park for further details:");
                //This is wriiten at the top of the menu when it is run
                for (int i = 1; i <= parks.Count; i++)
                { // for each element in the list starting at index 1, write the index number, park in the index minus
                    // one because indexes start at  and our list starts at 1
                    Console.WriteLine($"\t{i}) {parks[i - 1].Name}");
                }
                // this is option 0 to exit the menu
                Console.WriteLine("\t0) Exit");
                // this feeds the length of the parks list GetValidSelection method making sure it's and integer
                // and assigns it to user selection
                int userSelection = GetValidSelection(parks.Count);
                // if user selection is equal to 0, write the message, wait 3 seconds and return aka close
                if (userSelection == 0)
                {
                    Console.WriteLine("Thank you, come again :)");
                    Thread.Sleep(3000);
                    return;
                }
                // this subtracts 1 from the user selection to lign up with the list index element bc it starts at 0
                userSelection--;
                // this is the run park info method below using the user's input
                RunParkInfoMenu(parks[userSelection]);
                // This clears the console
                Console.Clear();
                //Console.ReadLine();
            }
        }
        // method that does not return anything so it's void
        private void RunParkInfoMenu(Park park)
        {
            while(true) // while this is true, run this loop
            {
                Console.Clear(); // clear the cosnsole
                Console.WriteLine($"{park.Name} National Park"); // writes the park name with National Park after
                Console.WriteLine($"Location:\t\t{park.Location}"); // 2 tabs and then park location
                Console.WriteLine($"Established:\t\t{park.EstablishDate:d}"); // 2 tabs and establish date
                Console.WriteLine($"Area:\t\t\t{park.Area:##,#} sq km"); // 3 tabs and area formatted with sq km after
                Console.WriteLine($"Annual Visitors:\t{park.Visitor:##,#}"); // tab # of visitors formatted
                Console.WriteLine($"\n{park.Description}"); // this prints park discription
                // this forms a new line and asks the user to select a command
                Console.WriteLine("\nSelect a Command\n\t1) View Campgrounds");
                //Console.WriteLine("\t2) Search for Reservation (not implemented, will throw exception)");
                Console.WriteLine("\t0) Return to Previous Screen");
                //this is just a new line
                Console.WriteLine();
                // assigns GVS method to the selection of 1
                int userSelection = GetValidSelection(1);
                // if the users selection is 0, jus return to the previous screen
                if (userSelection == 0)
                {
                    return;
                }
                // if the user selects 1, it calls the runcampgroundmenu that is fed park list(table from sql db)
                if (userSelection == 1)
                {
                    RunCampgroundMenu(park);
                }
                //if user selects 2 it would run a menu we did't create
                if (userSelection == 2)//it won't be
                {
                    RunReservationSearchMenu(park);
                }
            }
        }
        // this is the run reservation search menu with the list of parks (park table) as its arguement (if we did it)
        private void RunReservationSearchMenu(Park park)
        {
            throw new NotImplementedException();
        }
        // this is the runcampgroundmenu the is fed park list (park table)
        private void RunCampgroundMenu(Park park)
        {
            // loop this while true
            while(true)
            {// clear the console
                Console.Clear();
                // prints the park name 
                Console.WriteLine($"{park.Name} National Park Campgrounds");
                //this is just a spacing line
                Console.WriteLine();//DONE make this menu look nice
                // this returns the list (campground table) from the park selected using the method w created in cgsqldao
                IList<Campground> campgrounds = campDAO.GetAllCampgrounds(park);
                // this writes the list out
                WriteCampgroundsList(campgrounds);
                // this asks the user to make a choice to search for a reservation or return to the previous screen
                Console.WriteLine("\nSelect a Command\n\t1) Search for Available Reservation\n\t0) Return to Previous Screen");
                // assigns the GVS method thats fed 1 to userSelection
                int userSelection = GetValidSelection(1);
                // if user selection is 0, return to previous menu
                if (userSelection == 0)
                {
                    return;
                }
                // if it's not 0, run call the RARSM(cg) method
                else
                {
                    RunAvailableReservationSearchMenu(campgrounds);
                }
            }
        }
        // this method RARSM uses the ilist cg cg aka the campground table
        private void RunAvailableReservationSearchMenu(IList<Campground> campgrounds)
        {   // clears the console
            Console.Clear();
            // uses the WCL function with the cg list to print out the cg list (cg table from sql db)
            WriteCampgroundsList(campgrounds);
            // asks the user to select a campground to make a rezz or 0 to return to prev menu
            Console.Write("\nSelect a Campground to make a reservation or 0 to go back: ");
            // list of sites from the site table n sql
            IList<Site> sites;
            // cg list from cg class
            Campground campground;
            // the date of arrival
            DateTime userArrival;
            // date of departure
            DateTime userDeparture;
            // number of days they are staying
            int numberOfDaysToStay;
            // assigns GVS method with he number of cg's given to it to uSelection
            int userSelection = GetValidSelection(campgrounds.Count);
            // if users selects 0, return to prev menu
            if (userSelection == 0)
            {
                return;
            }
            // subtracts 1 from user selection bc the index starts at 0 while the options start at 1
            userSelection--;
            //this is the assigns the cg selection to cg
            campground = campgrounds[userSelection];
            // execute this loop while true
            while (true)
            {
                // asks user for arrival date 
                Console.Write("What is the arrival date (MM/DD/YYYY)? ");
                // this assigns the GVD method to uArrival
                userArrival = GetValidDate();
                // asks user for departure date
                Console.Write("What is the departure date (MM/DD/YYYY)? ");
                //this assigns the GCD method to uDeparture
                userDeparture = GetValidDate();
                // while loop in while loop that compares the uD to uA making sure that the reserve at least a day
                while (userDeparture.CompareTo(userArrival) <= 0)//you have reserve at least one day
                {
                    // returns ths if they don't reserve atleast 1 day
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
                 // if the user arrival year is the same as departure year, move on to next if statement
                if (userArrival.Year == userDeparture.Year)
                {
                    // if this statement is NOT(!) - userArrmonth is greater than/equal to the cg's open from month AND uArrM is less than/equal to cg open to month AND uDepM is greater than/equal to cg open from month AND uDepM is less than/equal to cg open to mm
                    if (!(userArrival.Month >= campground.OpenFrom && userArrival.Month <= campground.OpenTo && userDeparture.Month >= campground.OpenFrom && userDeparture.Month <= campground.OpenTo))
                    {
                        // clear the sites because they can't stay then
                        //in this case, they're NOT safe to keep going
                        sites.Clear();
                    }
                }
                else
                {    // if the cg open from month is NOT 1 AND cg open to month is NOT 12, then clear the sites
                    //in this case, they're camping over the new year and things get weiiiiird
                    if(campground.OpenFrom != 1 && campground.OpenTo != 12)
                    {
                        sites.Clear();
                    }
                }
                // if the count of the list is 0, tell the user there are no sites available at that time, asks if they want to try again
                if (sites.Count == 0)
                {
                    Console.WriteLine("There were no available campsites for the given time frame. \nWould you like to enter a different set of dates(Y/N)?");
                    // this takes the user input, makes it ower case and trim off extra spaces
                    string userYorN = Console.ReadLine().ToLower().Trim();
                    // if the user's input starts w/ y or is y then continuie with the rez loop again
                    if (userYorN.StartsWith("y"))
                    {
                        continue;//continue with the loop, which is to say, go back and ask for dates again
                    }
                    // this is if they don't respond y, yes, or anything starting with a letter other than y
                    Console.WriteLine("Returning to Campground Menu...");
                    Thread.Sleep(3000); // waits 3 seconds
                    return; // returns to previous menu
                }
                else
                {
                    break;//if we get here, then they gave us dates with available campsites and we're breaking the loop to continue with reserving
                    //this should happen MOST of the time
                }
            }
            // this gives the number of days they stay to nODTS to use to give the total cost
            numberOfDaysToStay = ((TimeSpan)(userDeparture - userArrival)).Days;
            // prints the message
            Console.WriteLine("Results Matching Your Search Criteria");
            // prints the sites
            WriteSiteList(sites);
            // prints the cost
            Console.WriteLine($"\tThe cost will be {campground.DailyFee * numberOfDaysToStay:c}");
            // asks for user input
            Console.Write($"\nWhich site would you like to reserve (enter 0 to cancel)? ");
            // assigns GVS(sites) to uSTR
            int userSiteToReserve = GetValidSiteSelection(sites);
            // if uSTO is 0, return to menu
            if (userSiteToReserve == 0)
            {
                return;
            }
            // DONE make sure they enter a valid name for the reservation
            // asks for a name that the reservation will be made under
            Console.Write("What name should the reservation be made under? ");
            //assigns GVN to uName
            string userName = GetValidName();
            //DONE do logic to find the site ID
            //starts siteI at -1
            int siteID = -1;
            // for each site in the list of sites (site table)
            foreach (Site site in sites)
            {
                // if the uSTR is the site number
                if(userSiteToReserve == site.SiteNumber)
                { // the siteID is now the site id
                    siteID = site.SiteID;
                }
            }
            //Console.WriteLine(siteID);
            // assings the current time when called
            DateTime currentTime = DateTime.Now;
            // this is assigns the create rez method to the reservationID int
            int reservationID = reservationDAO.CreateReservation(new Reservation() { SiteID = siteID, Name = userName, FromDate = userArrival, ToDate = userDeparture, CreateDate = currentTime });
            //DONE write out their reservation info here
            // this is the confirmation info printed to the user
            Console.WriteLine("Final Reservation Info:\n");
            Console.WriteLine($"\tThe site ID is {siteID}");
            Console.WriteLine($"\tThe reservation is under {userName}");
            Console.WriteLine($"\tYou are staying from {userArrival:d} to {userDeparture:d}");
            Console.WriteLine($"\tThe reservation creation time is {currentTime}");
            Console.WriteLine($"\tThe reservation has been made and the confirmation ID is {reservationID}");
            // prints it all out to the console
            Console.ReadLine();
            
        }
        // this is the GVSS method from the list of sites
        private int GetValidSiteSelection(IList<Site> sites)
        {
            // assigns the selecion alue to -1
            int selection = -1;
            // execute the loop while true
            while (true)
            {
                // parses the user input from a string to an int, then outs the int sel(user input) and makes sure it's greater than 0
                if (int.TryParse(Console.ReadLine(), out int sel) && sel >= 0)
                {
                    // assigns the parsed user input to selection
                    selection = sel;
                    // forech site in sites, if the user selection equals a site number, return the user input
                    foreach(Site site in sites)
                    {
                        if(selection == site.SiteNumber)
                        {
                            return selection;
                        }
                    }
                }// if none of that passes, write this message
                Console.Write($"That was not a valid selection. Select a valid site number (or 0 to exit):");
            }
        }
        //GVS method with the int max
        private int GetValidSelection(int max)
        {// asigns -1 to selection
            int selection = -1;
            //execute while true
            while (true)
            {
                // parses the string user input to int, outs/ assigns it to int sel, the makes sure it's greater/ equal to 0a nd less than or equal to the max int
                if (int.TryParse(Console.ReadLine(), out int sel) && sel >= 0 && sel <= max)
                {
                    // if it is, the user input is assigned to selection and breaks
                    selection = sel;
                    break;
                }
                else
                {
                    // if sel isn't, write this message
                    Console.Write($"That was not a valid selection. Select a number between 0 and {max}: ");
                }
            }
            //returns selection which is the pasred user input as an int
            return selection;
        }
        // GVN method
        private string GetValidName()
        {
            // assigns the user input to the string name and trims extra jazz
            string name = Console.ReadLine().Trim();
            // if the user input's length is 0 aka nothing
            if (name.Length == 0)
            {
                // ask user to enter valid name and returns them to the method
                Console.Write("Your name must be at least 1 character long: ");
                return GetValidName();//I wanted to call this one recursively for no reason in particular
            }
            // if the user's input isn't blank, then return the name
            return name;
        }
        // WCL method usng the list of campgrounds we got from the sql database cg table
        private void WriteCampgroundsList(IList<Campground> campgrounds)
        {
            //DONE add the header list for this
            // this prints out a header with the names of the columns in the list/table
            Console.WriteLine($"{"",-6}{"Name",-34}{"Open",-12}{"Close",-12}{"Daily Fee",-12}");//DONE make sure this looks fine
            // for each index elment (cg) in cg, write it out
            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine($"#{i + 1, -5}{campgrounds[i].Name,-34}{months[campgrounds[i].OpenFrom],-12}{months[campgrounds[i].OpenTo],-12}{campgrounds[i].DailyFee,-12:c}");
            }
        }
        // WSL method from the site list/table 
        private void WriteSiteList(IList<Site> sites)
        {
            // craetes the header/column names
            Console.WriteLine($"{"Site No.",-10}{"Max Occup.",-12}{"Accessible?",-13}{"Max RV Length",-16}{"Utility",-10}");//DONE make sure this looks fine
            //for each site in the list sites, write it out 
            foreach (Site site in sites)
            {
                Console.WriteLine($"{site.SiteNumber,-10}{site.MaxOccupancy,-12}{(site.HandicapAccess ? "Yes" : "No" ), -13}{((site.MaxRVLength == 0)? @"N/A" : Convert.ToString(site.MaxRVLength)),-16}{(site.HasUtilities ? "Yes" : "N/A"),-10}");
            }
        }
        // GVD method
        private DateTime GetValidDate()
        {
            // while this is true, run this loop
            while(true)
            {
                // parses the user input to a date time, assigns it to userTime and compares it to the current date making sure it's at least 1 day
                if(DateTime.TryParse(Console.ReadLine(), out DateTime userTime) && userTime.Date.CompareTo(DateTime.Now.Date) >= 0)
                {
                    // returns the user input as a datetime
                    return userTime.Date;
                }
                else
                {
                    // tells the user that it's not a valid date
                    Console.Write("That was not a valid Date, try MM/DD/YYYY: ");
                }
            }
        }
    }
}
