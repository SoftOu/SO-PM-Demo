using System;
using System.Collections.Generic;
using System.Text;
using SOPasswordManager.Repo.DTO;

namespace SOPasswordManager.Repo.DTO
{
   public class EmailSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string EnableSsl { get; set; }
        public string UseDefaultCredentials { get; set; }
    }
}
