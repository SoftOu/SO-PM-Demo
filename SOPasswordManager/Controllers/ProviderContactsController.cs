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
using SOPasswordManager.Repo.DTO;
using SOPasswordManager.Repo.GridService;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;

namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class ProviderContactsController : Controller
    {
        IProvider_Repository _IProvider_Repository;
        IProviderContacts_Repository _IProviderContacts_Repository;
        SOPasswordManagerContext context = new SOPasswordManagerContext();

        public ProviderContactsController()
        {
            _IProviderContacts_Repository = new ProviderContacts_Repository(new Repo.DbEntities.SOPasswordManagerContext());
            _IProvider_Repository = new Provider_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProviderContact(int id = 0)
        {            
            var ProviderList = context.Providers.OrderBy(x => x.ProviderName).ToList();

            ViewBag.ProviderList = ProviderList.Select(x => new SelectListItem
            {
                Value = x.ProviderId.ToString(),
                Text = x.ProviderName,
            }).ToList();

            if (id == 0)
            {
                ProviderContactsModel model = new ProviderContactsModel();
                return PartialView(model);
            }
            else
            {
                ProviderContactsModel model = new ProviderContactsModel();

                var data = (from c in context.ProviderContactDetail
                            where c.ProviderContactDetailId == id
                            select new
                            {
                                c.ProviderContactDetailId,
                                c.Name,
                                c.Surname,
                                c.Email1,
                                c.Email2,
                                c.PhoneNumber1,
                                c.PhoneNumber2,
                                c.Notes
                            }).FirstOrDefault();

                var data2 = (from cc in context.ProviderContact
                             where cc.ProviderContactDetailId == id
                             select new { cc.ProviderId }).FirstOrDefault();

                model.ProviderContactDetailId = data.ProviderContactDetailId;
                model.providerId = data2.ProviderId;
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
        public IActionResult AddProviderContact(ProviderContactsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(model);
                }
                if (model.ProviderContactDetailId == 0)
                {
                    ProviderContactDetail obj = new ProviderContactDetail();
                    obj.Name = model.Name;
                    obj.Surname = model.Surname;
                    obj.Email1 = model.Email1;
                    obj.Email2 = model.Email2;
                    obj.PhoneNumber1 = model.PhoneNumber1;
                    obj.PhoneNumber2 = model.PhoneNumber2;
                    obj.Notes = model.Notes;
                    context.ProviderContactDetail.Add(obj);
                    context.SaveChanges();

                    ProviderContact providerContacts = new ProviderContact();
                    providerContacts.ProviderId = model.providerId;
                    providerContacts.ProviderContactDetailId = obj.ProviderContactDetailId;
                    context.Set<ProviderContact>().Add(providerContacts);
                    context.SaveChanges();

                    TempData["ProviderContactAddSuccess"] = "Provider contact successfully inserted.";
                    return Json(new { success = true, url = Url.Action("Index", "ProviderContacts") });
                }
                else
                {
                    ProviderContactDetail obj = context.ProviderContactDetail.Find(model.ProviderContactDetailId);
                    obj.Name = model.Name;
                    obj.Surname = model.Surname;
                    obj.Email1 = model.Email1;
                    obj.Email2 = model.Email2;
                    obj.PhoneNumber1 = model.PhoneNumber1;
                    obj.PhoneNumber2 = model.PhoneNumber2;
                    obj.Notes = model.Notes;
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();

                    var updateClientContact = from x in context.ProviderContact where x.ProviderContactDetailId == model.ProviderContactDetailId select x;

                    foreach (var item in updateClientContact)
                    {
                        {
                            item.ProviderId = model.providerId;
                        }
                    }
                    context.SaveChanges();

                    TempData["ProviderContactEditSuccess"] = "Provider contact details successfully updated.";
                    TempData["IsNewRequest"] = true;
                    return Json(new { success = true, url = Url.Action("Index", "ProviderContacts") });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }           
        }

        public JsonResult DeleteProviderContact(int providerContactDetail_ID)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                var c_contacts = context.ProviderContact.Where(x => x.ProviderContactId == providerContactDetail_ID).ToList();
                if (c_contacts != null && c_contacts.Count > 0)
                {
                    context.ProviderContact.RemoveRange(c_contacts);
                    context.SaveChanges();
                }

                var contacts = new ProviderContactDetail { ProviderContactDetailId = providerContactDetail_ID };
                context.Entry(contacts).State = EntityState.Deleted;
                context.SaveChanges();
                RetValue = new { success = true, Message = "Provider contact details successfully deleted." };

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

    }
}