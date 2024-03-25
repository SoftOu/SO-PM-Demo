using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string Email ,string Paswword,string UserName);
    }
}
