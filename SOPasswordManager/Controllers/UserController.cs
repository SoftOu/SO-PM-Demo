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
    public class UserController : Controller
    {
        IUser_Repository _IUser_Repository;

        public UserController()
        {
            _IUser_Repository = new User_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddUser(int id = 0)
        {
            try
            {
                SystemUserModel objUser = new SystemUserModel();
                if (id!=0)
                {
                    var userDetail = await _IUser_Repository.GetUser(id);
                    if(userDetail!=null)
                    {
                        objUser.FirstName = userDetail.FirstName;
                        objUser.LastName = userDetail.LastName;
                        objUser.Email = userDetail.Email;
                        objUser.PhoneNumber = userDetail.PhoneNumber;
                        objUser.RoleId = userDetail.RoleId;
                        objUser.SytemUserId = userDetail.SytemUserId;
                        objUser.Status = userDetail.Status;
                    }
                    
                }
                var roles = await _IUser_Repository.getUserRole();
                ViewBag.RoleList = roles.Select(x => new SelectListItem
                {
                    Value = x.RoleId.ToString(),
                    Text = x.RoleName,
                }).ToList();

                return PartialView(objUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        [HttpPost]
        public IActionResult AddUser(SystemUserModel model)
        {
            if (!ModelState.IsValid)
            {
                
                return PartialView(model);
            }
            else
            {
                _IUser_Repository.manageUser(model);
                if (model.SytemUserId == 0)
                {
                    TempData["UsersAddSuccess"] = "User details successfully inserted.";
                    return Json(new { success = true, url = Url.Action("Index", "User") });
                }
                else
                {
                    TempData["UsersEditSuccess"] = "User details successfully updated.";
                    return Json(new { success = true, url = Url.Action("Index", "User") });
                }
            }
            
        }

        public JsonResult DeleteUser(int userId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                _IUser_Repository.deleteUser(userId);
                RetValue = new { success = true, Message = "User details successfully deleted." };

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

        public async Task<IActionResult> ValidateEmail(string Email,int SytemUserId)
        {
            bool IsExist =await _IUser_Repository.IsEmailExist(Email, SytemUserId);
            if(IsExist)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
            
        }


    }
}