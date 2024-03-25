using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{
    public interface IProvider_Repository : IDisposable
    {
        Task<List<Providers>> GetProviderList();       
    }
}
