using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SOPasswordManager.Repo
{
    public class Configuration
    {

        public string SqlServerConnectionString { get; set; }
        
        public Configuration()
        {

            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            SqlServerConnectionString = DBConnectionString.ConnectionString; ;

        }
    }
}
