using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Filters;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;

namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class CityController : Controller
    {
        ICity_Repository _ICity_Repository;
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        public CityController()
        {
            _ICity_Repository = new City_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddCity(int id = 0)
        {
            try
            {
                var CountryList = context.County.OrderBy(x => x.CountyName).ToList();
                ViewBag.CountryList = CountryList.Select(x => new SelectListItem
                {
                    Value = x.CountryId.ToString(),
                    Text = x.CountyName,
                }).ToList();
                if (id == 0)
                {
                    CityModel model = new CityModel();
                    await context.City.FindAsync(id);
                    return PartialView(model);
                }
                else
                {
                    CityModel model = new CityModel();
                    var objData = context.City.Find(id);
                    model.CityId = objData.CityId;
                    model.CountryId = (int)objData.CountryId;
                    model.CityName = objData.CityName;
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpPost]
        public IActionResult AddCity(CityModel model)
        {
            if (!ModelState.IsValid)
            {

                return PartialView(model);
            }
            else
            {
                
                if (model.CityId == 0)
                {
                    _ICity_Repository.ManageCity(model);
                    TempData["CityAddSuccess"] = "City details successfully inserted.";
                    return Json(new { success = true, url = Url.Action("Index", "City") });
                }
                else
                {
                    _ICity_Repository.ManageCity(model);
                    TempData["CityEditSuccess"] = "City details successfully updated.";
                    return Json(new { success = true, url = Url.Action("Index", "City") });
                }
            }

        }

        public JsonResult DeleteCity(int cityId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                _ICity_Repository.DeleteCity(cityId);
                RetValue = new { success = true, Message = "City details successfully deleted." };
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