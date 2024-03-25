using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Filters;
using SOPasswordManager.Models;
using SOPasswordManager.Repo.DataService;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;


namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class ClientContactsController : Controller
    {
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        IClientContacts_Repository _IClientContacts_Repository;
        IClient_Repository _IClient_Repository;

        public ClientContactsController()
        {
            _IClientContacts_Repository = new ClientContacts_Repository(new Repo.DbEntities.SOPasswordManagerContext());
            _IClient_Repository = new Client_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        #region ClientContact

        public IActionResult Index()
        {            
            return View();
        }
       
        public async Task<IActionResult> AddClientContact(int id = 0)
        {            
            var ClientList = await _IClient_Repository.GetClientList();

            ViewBag.ClientList = ClientList.Select(x => new SelectListItem
            {
                Value = x.ClientId.ToString(),
                Text = x.ClientName,
            }).ToList();

            if (id == 0)
            {
                ClientContactsModel model = new ClientContactsModel();
                return PartialView(model);
            }
            else
            {
               ClientContactsModel model = new ClientContactsModel();
               
                var data = (from c in context.Contacts
                           where c.ContactId == id
                           select new 
                           {
                               c.ContactId,
                               c.Name,
                               c.Surname,
                               c.Email1,
                               c.Email2,
                               c.PhoneNumber1,
                               c.PhoneNumber2,
                               c.Notes
                           }).FirstOrDefault();

                var data2 = (from cc in context.ClientContacts
                             where cc.ContactId == id
                             select new  {cc.ClientId }).FirstOrDefault();
                             
                model.ContactId = data.ContactId;
                model.clientId = data2.ClientId;
                model.Name = data.Name;
                model.Surname = data.Surname;
                model.Email1 = data.Email1;
                model.Email2 = data.Email2;
                model.PhoneNumber1 = data.PhoneNumber1;
                model.PhoneNumber2 = data.PhoneNumber2;
                model.Notes = data.Notes;

                return PartialView(model);
            }
        }

        [HttpPost]
        public IActionResult AddClientContact(ClientContactsModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            if (model.ContactId == 0)
            {
                Contacts obj = new Contacts();
                obj.Name = model.Name;
                obj.Surname = model.Surname;
                obj.Email1 = model.Email1;
                obj.Email2 = model.Email2;
                obj.PhoneNumber1 = model.PhoneNumber1;
                obj.PhoneNumber2 = model.PhoneNumber2;
                obj.Notes = model.Notes;             
                context.Set<Contacts>().Add(obj);
                context.SaveChanges();

                ClientContacts clientContacts = new ClientContacts();
                clientContacts.ClientId = model.clientId;
                clientContacts.ContactId = obj.ContactId;
                context.Set<ClientContacts>().Add(clientContacts);
                context.SaveChanges();



                TempData["ClientContactAddSuccess"] = "Contact successfully inserted.";
                return Json(new { success = true, url = Url.Action("Index", "ClientContacts") });
            }
            else
            {
                Contacts obj = context.Contacts.Find(model.ContactId);
                obj.Name = model.Name;
                obj.Surname = model.Surname;
                obj.Email1 = model.Email1;
                obj.Email2 = model.Email2;
                obj.PhoneNumber1 = model.PhoneNumber1;
                obj.PhoneNumber2 = model.PhoneNumber2;
                obj.Notes = model.Notes;              
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();

                var updateClientContact = from x in context.ClientContacts where x.ContactId == model.ContactId select x;

                foreach (var item in updateClientContact)
                {
                    {
                        item.ClientId = model.clientId;                        
                    }
                }
                context.SaveChanges();

                TempData["ClientContactEditSuccess"] = "Contact details successfully updated.";
                TempData["IsNewRequest"] = true;
                return Json(new { success = true, url = Url.Action("Index", "ClientContacts") });
            }
        }

        public JsonResult DeleteClientContact(int contactId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                var c_contacts = context.ClientContacts.Where(x => x.ContactId == contactId).ToList();
                if(c_contacts != null && c_contacts.Count > 0)
                {
                    context.ClientContacts.RemoveRange(c_contacts);
                    context.SaveChanges();
                }

               
                var contacts = new Contacts { ContactId = contactId };
                context.Entry(contacts).State = EntityState.Deleted;
                context.SaveChanges();
                RetValue = new { success = true, Message = "Contact details successfully deleted." };

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