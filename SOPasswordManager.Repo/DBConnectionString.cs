using System;
using System.Collections.Generic;
using System.Text;

namespace SOPasswordManager.Repo
{
    public class DBConnectionString
    {
        //public static string ConnectionString = "Server=localhost;Database=so-pm;Uid=root;Pwd=;";
        public static string ConnectionString = "Server=51.75.252.115;Database=SO-PM-Demo;Uid=userdemo;Pwd=PassDemo#123;";

        public static string Host = "smtp.gmail.com";
        public static int Port = 587;
        public static string Email = "email@gmail.com";
        public static string Password = "emailpass";
        public static bool EnableSsl = true;
        public static bool UseDefaultCredentials = true;
        public static string SiteURL = "https://localhost:44376";
    }
}
