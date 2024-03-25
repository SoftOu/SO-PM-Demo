using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ClientController : Controller
    {
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        IClient_Repository _IClient_Repository;

        public ClientController()
        {
            _IClient_Repository = new Client_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        #region CLIENT

        public IActionResult Index()
        {
            //ViewBag.clientcontactlist = new SelectListItem();
            return View();
        }
        
        [HttpPost]
        public async Task<GridDTO> GetClientData()
        {
            string requestData = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                requestData = reader.ReadToEnd();
            }
            NameValueCollection data = HttpUtility.ParseQueryString(requestData);
            string mode = Convert.ToString(data["mode"]);            
            var Client_ID = 0;        
            string Search = Convert.ToString(data["search[value]"]);
            TempData["IsNewRequest"] = false;
            TempData["inputClient"] = Search;
            GridColumnsConfig og = new GridColumnsConfig(mode, data, GetClientDataWhereCause(Client_ID));
            return await og.GetData();
        }

        public string GetClientDataWhereCause(int Client_ID)
        {
            string w = "1=1";
            if (Client_ID != 0)
            {
                w += " AND Client_ID = " + Client_ID;
            }
            return w;
        }

        public JsonResult GetClientContactList()
        {           
            //var clientsList = (from c in context.Contacts
            //            where !context.ClientContacts.Any(f => f.ContactId == c.ContactId)
            //            select c).ToList();
            //clientsList.Select(x => new SelectListItem
            //{
            //    Value = x.ContactId.ToString(),
            //    Text = x.Name
            //}).ToList();
            var clientlist = (from c in context.Contacts
                              join cc in context.ClientContacts on c.ContactId equals cc.ContactId
                              where c.ContactId == cc.ContactId
                              select new 
                              {  c.Name,
                                 c.ContactId,
                              }).ToList();
            return Json(clientlist);
        }


        public JsonResult GetClientlist()
        {
            var clients = context.Clients.ToList();
            clients.Select(x => new SelectListItem
            {
                Value = x.ClientId.ToString(),
                Text = x.ClientName,
            }).ToList();
            return Json(clients);
        }

        public JsonResult SaveClients(ClientModel model)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {

                if (model.Client_ID == 0)
                {
                    Clients obj = new Clients();
                    obj.ClientName = model.ClientName;
                    obj.CountryId = model.countryId;
                    obj.CityId = model.cityId;
                    obj.DateCreated = DateTime.Now;
                    obj.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                    context.Set<Clients>().Add(obj);
                    context.SaveChanges();
                    //if(model.contactId != null)
                    //{
                    //    var contactId = model.contactId.Split(',');
                    //    foreach (var item in contactId)
                    //    {
                    //        ClientContacts contacts = new ClientContacts();
                    //        contacts.ContactId = Convert.ToInt32(item);
                    //        contacts.ClientId = obj.ClientId;
                    //        context.Set<ClientContacts>().Add(contacts);
                    //        context.SaveChanges();
                    //    }
                    //}
                    
                    RetValue = new { success = true, Message = "Client successfully inserted." };
                }
                else
                {
                    Clients obj = context.Clients.Find(model.Client_ID);
                    obj.ClientId = model.Client_ID;
                    obj.ClientName = model.ClientName;
                    obj.CountryId = model.countryId;
                    obj.CityId = model.cityId;
                    obj.DateUpdated = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    var listContact = context.ClientContacts.Where(x => x.ClientId == obj.ClientId).ToList();
                    context.ClientContacts.RemoveRange(listContact);
                    context.SaveChanges();
                   //if(model.contactId!=null)
                   // {
                   //     var contactId = model.contactId.Split(',');
                   //     foreach (var item in contactId)
                   //     {
                   //         ClientContacts contacts = new ClientContacts();
                   //         contacts.ContactId = Convert.ToInt32(item);
                   //         contacts.ClientId = obj.ClientId;
                   //         context.Set<ClientContacts>().Add(contacts);
                   //         context.SaveChanges();
                   //     }
                   // }
                    
                    RetValue = new { success = true, Message = "Record Updated Successfully." };
                }
            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }
            return Json(RetValue);
        }

        public JsonResult DeleteClient(int clientId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {

                var projects = context.Projects.Where(x => x.ClientId == clientId).FirstOrDefault();
                if (projects != null)
                {
                    RetValue = new { success = false, Message = "You can't delete this data.It has reference of other data." };
                }
                else
                {
                    var clientContacts = context.ClientContacts.Where(x => x.ClientId == clientId).ToList();
                    if(clientContacts.Count > 0)
                    {
                        context.ClientContacts.RemoveRange(clientContacts);
                        context.SaveChanges();
                    }

                    var client = context.Clients.Where(x => x.ClientId == clientId).FirstOrDefault();
                    context.Entry(client).State = EntityState.Deleted;
                    context.SaveChanges();
                    RetValue = new { success = true, Message = "Client successfully deleted." };
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

        #endregion


        #region CLIENTUSER

        public async Task<IActionResult> ClientUser()
        {
            var ClientList = await _IClient_Repository.GetClientList();

            ViewBag.ClientList = ClientList.Select(x => new SelectListItem
            {
                Value = x.ClientId.ToString(),
                Text = x.ClientName,
            }).ToList();

            return View();
        }

        public JsonResult GetCountries()
        {
            var countries = context.County.ToList().OrderBy(x => x.CountyName);
            countries.Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountyName,
            }).ToList();
            return Json(countries);
            //return Json(context.County.ToList());
        }

        public JsonResult GetCities(int CountryId)
        {
            var cities = from city in context.City where city.CountryId == CountryId orderby city.CityName select city;

            return Json(cities);

        }



        public JsonResult GetContacts(int ClientId)
        {
            var clientContacts = context.ClientContacts.Where(x => x.ClientId == ClientId).Select(x=>x.ContactId).ToList();

            var existingContact = context.ClientContacts.Where(x=>x.ClientId!= ClientId).Select(x => x.ContactId).ToList();

            var contacts = (from c in context.Contacts
                          where existingContact.Contains(c.ContactId) == false
                          select new SelectListItem
                          {
                              Text=c.Name,
                              Value=c.ContactId.ToString(),
                          }).ToList();

            return Json(new { contacts=contacts, clientContacts= clientContacts });
        }

        public JsonResult RoleNameList()
        {
            var roles = context.UserRole.ToList();
            roles.Select(x => new SelectListItem
            {
                Value = x.RoleId.ToString(),
                Text = x.RoleName,
            }).ToList();
            return Json(roles);
        }

        [HttpGet]
        public IActionResult AddClientUser(int id = 0)
        {
            var Clientlist = context.Clients.ToList();

            ViewBag.ClientList = Clientlist.Select(x => new SelectListItem
            {
                Value = x.ClientId.ToString(),
                Text = x.ClientName,
            }).ToList();

            var roles = context.UserRole.ToList();
            ViewBag.RoleNameList = roles.Select(x => new SelectListItem
            {
                Value = x.RoleId.ToString(),
                Text = x.RoleName,
            }).ToList();

            if (id == 0)
            {
                ClientUserModel model = new ClientUserModel();
                context.ClientUser.FindAsync(id);
                return PartialView(model);
            }
            else
            {
                ClientUserModel model = new ClientUserModel();
                var objData = context.ClientUser.Find(id);
                model.ClientId = objData.ClientId;
                model.RoleId = objData.RoleId;
                model.ClientUserId = objData.ClientUserId;
                model.FirstName = objData.FirstName;
                model.LastName = objData.LastName;
                model.Email = objData.Email;
                model.PhoneNumber = objData.PhoneNumber;
                return PartialView(model);
            }

        }

        [HttpPost]
        public IActionResult AddClientUser(ClientUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            if (model.ClientUserId == 0)
            {
                ClientUser obj = new ClientUser();
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.Email = model.Email;
                obj.PhoneNumber = model.PhoneNumber;
                obj.ClientId = model.ClientId;
                obj.RoleId = model.RoleId;
                obj.DateCreated = DateTime.Now;
                obj.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                context.Set<ClientUser>().Add(obj);
                context.SaveChanges();
                TempData["ClientUserAddSuccess"] = "Client user successfully inserted.";
                return Json(new { success = true, url = Url.Action("ClientUser", "Client") });
            }
            else
            {
                ClientUser obj = context.ClientUser.Find(model.ClientUserId);
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.Email = model.Email;
                obj.PhoneNumber = model.PhoneNumber;
                obj.ClientId = model.ClientId;
                obj.RoleId = model.RoleId;
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
                TempData["ClientUserEditSuccess"] = "Client user successfully updated.";
                return Json(new { success = true, url = Url.Action("ClientUser", "Client") });
            }
        }

        public JsonResult DeleteClientUser(int clientUserId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                var client = new ClientUser { ClientUserId = clientUserId };
                context.Entry(client).State = EntityState.Deleted;
                context.SaveChanges();
                RetValue = new { success = true, Message = "User successfully deleted." };

            }
            catch (DbUpdateException)
            {
                RetValue = new { success = false, Message = "You can't delete this data.It has reference of other data." };
            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }
            return Json(RetValue);
        }

        #endregion

    }
   
}