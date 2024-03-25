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
    public class ProviderContacts_Repository : IProviderContacts_Repository, IDisposable
    {
        SOPasswordManagerContext context;

        public ProviderContacts_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
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

