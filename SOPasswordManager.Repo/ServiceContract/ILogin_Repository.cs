using SOPasswordManager.Repo.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{
    public interface ILogin_Repository : IDisposable
    {
        Task<SystemUser> CheckLogin(string EmailId, string password);
        void SendTwoStepVerificationMail(string UserName, int VerificationCode);
        void SaveOTP(string Email, int OTP);
    }
}
