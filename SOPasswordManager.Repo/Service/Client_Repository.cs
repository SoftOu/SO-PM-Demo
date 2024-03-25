using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.Service
{
    public class Client_Repository : IClient_Repository, IDisposable
    {

        SOPasswordManagerContext context;

        public Client_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public async Task<List<Clients>> GetClientList()
        {
            return await context.Clients.OrderBy(x=>x.ClientName).ToListAsync();
        }

        public async Task<List<City>> GetCityList(int id)
        {
            return await context.City.Where(x => x.CountryId == id).ToListAsync();
        }
        
        public async Task<List<County>> GetCountryList()
        {
            return await context.County.ToListAsync();
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
