using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{
    public interface ICountry_Repository : IDisposable
    {
        void ManageCountry(CountryModel objUser);
        void DeleteCountry(int countryId);
        Task<County> GetCountry(int countryId);
    }
}
