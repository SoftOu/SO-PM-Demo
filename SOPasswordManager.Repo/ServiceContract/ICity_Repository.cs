using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{
    public interface ICity_Repository : IDisposable
    {
        void ManageCity(CityModel objUser);
        void DeleteCity(int cityId);
    }
}
