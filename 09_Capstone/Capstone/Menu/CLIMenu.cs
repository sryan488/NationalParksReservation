using System;
using System.Collections.Generic;
using System.Text;
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

        public CLIMenu (string connectionString)
        {
            parkDAO = new ParkSqlDAO(connectionString);
            campDAO = new CampgroundSqlDAO(connectionString);
            siteDAO = new SiteSqlDAO(connectionString);
            reservationDAO = new ReservationSqlDAO(connectionString);
        }
        public void Run()
        {

        }
    }
}
