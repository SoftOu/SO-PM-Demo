using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Filters;
using SOPasswordManager.Models;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;

namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class CountryController : Controller
    {
        ICountry_Repository _ICountry_Repository;
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        public CountryController()
        {
            _ICountry_Repository = new Country_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddCountry(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    CountryModel model = new CountryModel();
                    await context.County.FindAsync(id);
                    return PartialView(model);
                }
                else
                {
                    CountryModel model = new CountryModel();
                    var objData = context.County.Find(id);
                    model.CountryId = objData.CountryId;
                    model.CountryName = objData.CountyName;                    
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpPost]
        public IActionResult AddCountry(CountryModel model)
        {
            if (!ModelState.IsValid)
            {

                return PartialView(model);
            }
            else
            {
                _ICountry_Repository.ManageCountry(model);
                if (model.CountryId == 0)
                {
                    TempData["CountryAddSuccess"] = "Country details successfully inserted.";
                    return Json(new { success = true, url = Url.Action("Index", "Country") });
                }
                else
                {
                    TempData["CountryEditSuccess"] = "Country details successfully updated.";
                    return Json(new { success = true, url = Url.Action("Index", "Country") });
                }
            }

        }

        public JsonResult DeleteCountry(int countryId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                _ICountry_Repository.DeleteCountry(countryId);
                RetValue = new { success = true, Message = "Country details successfully deleted." };

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