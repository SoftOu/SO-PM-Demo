using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Repo.DataService;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.Service
{
    public class Login_Repository : ILogin_Repository, IDisposable
    {

        SOPasswordManagerContext context;

        public Login_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public void SendTwoStepVerificationMail(string UserName, int VerificationCode)
        {
            var getDetail = (from item in context.SystemUser where item.Email == UserName select item).FirstOrDefault();
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("<html>");
            mailBody.Append("<body>");
            mailBody.Append("<table>");
            mailBody.Append("<tr>");
            mailBody.Append("<td>");
            mailBody.Append("<p> Dear <b>" + getDetail.FirstName + "</b>,</p>");
            mailBody.Append("<p>Your One Time Password(OTP) is : " + VerificationCode + "</p>");
            mailBody.Append("</td>");
            mailBody.Append("</tr>");
            mailBody.Append("<tr>");
            mailBody.Append("<td>");
            mailBody.Append("<p> Thanks ,</p>");
            mailBody.Append("<p> SO-Password Manager </p>");
            mailBody.Append("</td>");
            mailBody.Append("</tr>");
            mailBody.Append("</table>");
            mailBody.Append("<body>");
            mailBody.Append("</body>");
            mailBody.Append("</html>");
            Common.SendEmailAsync(UserName, mailBody.ToString(), "SO-Password Manager Two Step Verification ");
        }

        public void SaveOTP(string Email, int OTP)
        {
            var saveOTP = context.SystemUser.Where(a => a.Email == Email).FirstOrDefault();
            saveOTP.VerificationCode = OTP;
            context.SaveChanges();
        }

        public async Task<SystemUser> CheckLogin(string EmailId, string password) 
        {
            try
            {
                var pass = EncryptDecrypt.Encrypt(password);
                return await context.SystemUser.Include(x=>x.Role).Where(x => x.Email == EmailId && x.Password == EncryptDecrypt.Encrypt(password) && x.Status==true).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
