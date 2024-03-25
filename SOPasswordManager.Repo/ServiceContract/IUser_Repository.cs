using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{ 
    public interface IUser_Repository :IDisposable
    {
        Task<SystemUser> GetUser(int userId);
        Task<bool> IsEmailExist(string Email, int SytemUserId);
        void manageUser(SystemUserModel objUser);
        void deleteUser(int userId);
        Task<List<UserRole>> getUserRole();
        int GenerateRandom4DigitOTP();        
    }
}
