using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Filters;
using SOPasswordManager.Models;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.GridService;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;

namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class ProviderController : Controller
    {
        IProvider_Repository _IProvider_Repository;
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        public ProviderController()
        {
            _IProvider_Repository = new Provider_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult SaveProvider(ProviderModel model)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                if (model.Provider_ID == 0)
                {
                    Providers obj = new Providers();
                    obj.ProviderName = model.ProviderName;
                    obj.CountryId = model.countryId;
                    obj.CityId = model.cityId;
                    obj.DateCreated = DateTime.Now;
                    obj.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                    obj.BillingFullName = model.BillingFullName;
                    obj.IdCard = model.IdCard;
                    obj.FullAddress = model.FullAddress;
                    obj.PostalCode = model.PostalCode;
                    context.Set<Providers>().Add(obj);
                    context.SaveChanges();                    

                    RetValue = new { success = true, Message = "Provider successfully inserted." };
                }
                else
                {
                    Providers obj = context.Providers.Find(model.Provider_ID);
                    obj.ProviderId = model.Provider_ID;
                    obj.ProviderName = model.ProviderName;
                    obj.CountryId = model.countryId;
                    obj.CityId = model.cityId;
                    obj.DateUpdated = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                    obj.BillingFullName = model.BillingFullName;
                    obj.IdCard = model.IdCard;
                    obj.FullAddress = model.FullAddress;
                    obj.PostalCode = model.PostalCode;
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    var listContact = context.ProviderContact.Where(x => x.ProviderId == obj.ProviderId).ToList();
                    context.ProviderContact.RemoveRange(listContact);
                    context.SaveChanges();                    

                    RetValue = new { success = true, Message = "Record Updated Successfully." };
                }
            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }
            return Json(RetValue);
        }

        public JsonResult DeleteProvider(int providerId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {

                var projects = context.Projects.Where(x => x.ProviderId == providerId).FirstOrDefault();
                if (projects != null)
                {
                    RetValue = new { success = false, Message = "You can't delete this data.It has reference of other data." };
                }
                else
                {
                    var providerContacts = context.ProviderContact.Where(x => x.ProviderId == providerId).ToList();
                    if (providerContacts.Count > 0)
                    {
                        context.ProviderContact.RemoveRange(providerContacts);
                        context.SaveChanges();
                    }

                    var providers = context.Providers.Where(x => x.ProviderId == providerId).FirstOrDefault();
                    context.Entry(providers).State = EntityState.Deleted;
                    context.SaveChanges();
                    RetValue = new { success = true, Message = "Provider successfully deleted." };
                }
            }
            catch (DbUpdateException EX)
            {
                RetValue = new { success = false, Message = "You can't delete this data.It has reference of other data." };
            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }

            return Json(RetValue);
        }

        [HttpPost]
        public async Task<GridDTO> GetProviderData()
        {
            string requestData = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                requestData = reader.ReadToEnd();
            }
            NameValueCollection data = HttpUtility.ParseQueryString(requestData);
            string mode = Convert.ToString(data["mode"]);
            var Provider_ID = 0;
            string Search = Convert.ToString(data["search[value]"]);
            TempData["IsNewRequest"] = false;
            TempData["inputProvider"] = Search;
            GridColumnsConfig og = new GridColumnsConfig(mode, data, GetProviderDataWhereCause(Provider_ID));
            return await og.GetData();
        }

        public string GetProviderDataWhereCause(int Provider_ID)
        {
            string w = "1=1";
            if (Provider_ID != 0)
            {
                w += " AND Provider_ID = " + Provider_ID;
            }
            return w;
        }
    }
}