using SOPasswordManager.Repo.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SOPasswordManager.Repo.ServiceContract
{
    public interface IClient_Repository :IDisposable
    {
        Task<List<Clients>> GetClientList();
        Task<List<County>> GetCountryList();
        Task<List<City>> GetCityList(int id);
    }
}
