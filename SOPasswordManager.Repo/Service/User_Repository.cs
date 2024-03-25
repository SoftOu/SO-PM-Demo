using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using SOPasswordManager.Repo.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SOPasswordManager.Repo.DataService;

namespace SOPasswordManager.Repo.Service
{
    public class User_Repository : IUser_Repository, IDisposable
    {
        SOPasswordManagerContext context;

        public User_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public void manageUser(SystemUserModel objUser)
        {
            if (objUser.SytemUserId == 0)
            {
                var Password = GetRandomPassword();
                SystemUser objSystemUser = new SystemUser();
                objSystemUser.FirstName = objUser.FirstName;
                objSystemUser.LastName = objUser.LastName;
                objSystemUser.Email = objUser.Email;
                objSystemUser.PhoneNumber = objUser.PhoneNumber;
                objSystemUser.RoleId = objUser.RoleId;
                objSystemUser.Password = EncryptDecrypt.Encrypt(Password);
                objSystemUser.IsFirstLogin = true;
                objSystemUser.Status = objUser.Status;
                context.SystemUser.Add(objSystemUser);
                context.SaveChanges();
                Task.Run(() => SendRegistrationMail(objSystemUser.FirstName, objSystemUser.Email, Password));
            }
            else
            {
                var objSystemUser = context.SystemUser.Find(objUser.SytemUserId);
                if (objSystemUser != null)
                {
                    objSystemUser.FirstName = objUser.FirstName;
                    objSystemUser.LastName = objUser.LastName;
                    objSystemUser.Email = objUser.Email;
                    objSystemUser.PhoneNumber = objUser.PhoneNumber;
                    objSystemUser.RoleId = objUser.RoleId;
                    objSystemUser.Status = objUser.Status;
                }
                context.SaveChanges();
            }
        }

        public async Task<List<UserRole>> getUserRole()
        {
            return await context.UserRole.ToListAsync();
        }

        public void deleteUser(int userId)
        {
            var userDetail = context.SystemUser.Find(userId);
            if (userDetail != null)
            {
                context.Entry(userDetail).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public async Task<SystemUser> GetUser(int userId)
        {
            return await context.SystemUser.FindAsync(userId);
        }

        public async Task<bool> IsEmailExist(string Email, int SytemUserId)
        {
            var UserDetail = await context.SystemUser.Where(x => x.Email == Email && x.SytemUserId!= SytemUserId).FirstOrDefaultAsync();
            if(UserDetail!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetRandomPassword()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public int GenerateRandom4DigitOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public void SendRegistrationMail(string Name, string UserName, string Password)
        {

            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("<html>");
            mailBody.Append("<body>");
            mailBody.Append("<table>");
            mailBody.Append("<tr>");
            mailBody.Append("<td>");
            mailBody.Append("<p> Dear <b>" + Name + "</b>,</p>");
            mailBody.Append("<p>Your user has been created in SO-Password Manager. You can login using below detail</p>");
            mailBody.Append("<p> URL: <a href=" + DBConnectionString.SiteURL + ">"+ DBConnectionString.SiteURL + "</a></p>");
            mailBody.Append("<p> User Name:" + UserName + "</p>");
            mailBody.Append("<p> Password:" + Password + "</p>");
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
            Common.SendEmailAsync(UserName, mailBody.ToString(), "SO-Password Manager Login Detail ");
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
