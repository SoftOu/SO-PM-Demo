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
    public class City_Repository : ICity_Repository, IDisposable
    {
        SOPasswordManagerContext context;

        public City_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public void ManageCity(CityModel cityModel)
        {
            if (cityModel.CityId == 0)
            {
                City city = new City();
                city.CityName = cityModel.CityName;
                city.CountryId = cityModel.CountryId;
                context.City.Add(city);
                context.SaveChanges();
            }
            else
            {
                var city = context.City.Find(cityModel.CityId);
                if (city != null)
                {
                    city.CityName = cityModel.CityName;
                    city.CountryId = cityModel.CountryId;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteCity(int cityId)
        {
            var city = context.City.Find(cityId);
            if (city != null)
            {
                context.Entry(city).State = EntityState.Deleted;
                context.SaveChanges();
            }
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
