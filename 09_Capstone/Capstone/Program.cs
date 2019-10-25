using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Capstone.Menu;

namespace Capstone
{
    class Program
    {
        // this is the main method that start the whole thing
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            // this runs the menu
            string connectionString = configuration.GetConnectionString("Project");
            CLIMenu menu = new CLIMenu(connectionString);
            menu.Run();
        }
    }
}
