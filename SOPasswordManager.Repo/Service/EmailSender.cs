using System;
using System.Collections.Generic;
using System.Text;
using SOPasswordManager.Repo.ServiceContract;
using SOPasswordManager.Repo.DbEntities;
using System.Threading.Tasks;
using SOPasswordManager.Repo.DTO;
using System.Net.Mail;
using System.Net;

namespace SOPasswordManager.Repo.Service
{
  public  class EmailSender :IEmailSender, IDisposable
    {
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        public EmailSender(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public Task SendEmailAsync(string Email,string Password,string UserName)
        {
            try
            {
                var mm = new MailMessage();
                mm.To.Add(new MailAddress(Email.Trim()));
                mm.From = new MailAddress(DBConnectionString.Email, "SO-Password Manager Support");
                mm.Subject = "Password Recovery";
                mm.Body = string.Format("Dear&nbsp;<b>" + UserName + "</b>,<br />" +
                "<br />Your Password details are as below:</br></br>" +
                "<br/>Password: {0}</br>" + "<br/><br/>Thanks" + "<br/><b>SO-Password Manager</b>",Password );
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = DBConnectionString.Host;
                smtp.EnableSsl = DBConnectionString.EnableSsl;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = DBConnectionString.Email;
                NetworkCred.Password = DBConnectionString.Password;
                smtp.UseDefaultCredentials = DBConnectionString.UseDefaultCredentials;
                smtp.Credentials = NetworkCred;
                smtp.Port = DBConnectionString.Port;
                smtp.Send(mm);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(0);
        }
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
