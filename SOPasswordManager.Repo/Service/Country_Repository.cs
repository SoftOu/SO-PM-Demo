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
    public class Country_Repository : ICountry_Repository, IDisposable
    {
        SOPasswordManagerContext context;

        public Country_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public void ManageCountry(CountryModel countryModel)
        {
            if (countryModel.CountryId == 0)
            {
                County county = new County();
                county.CountyName = countryModel.CountryName;               
                context.County.Add(county);
                context.SaveChanges();
            }
            else
            {
                var county = context.County.Find(countryModel.CountryId);
                if (county != null)
                {
                    county.CountyName = countryModel.CountryName; 
                    context.SaveChanges();
                }
            }
        }

        public void DeleteCountry(int countryId)
        {
            var county = context.County.Find(countryId);
            if (county != null)
            {
                context.Entry(county).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }       

        public async Task<County> GetCountry(int countryId)
        {
            return await context.County.FindAsync(countryId);
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
