using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Google.Authenticator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Filters;
using SOPasswordManager.Models;
using SOPasswordManager.Repo.DataService;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;

namespace SOPasswordManager.Controllers
{
    public class LoginController : Controller
    {
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        ILogin_Repository _ILogin_Repository;
        IUser_Repository _IUser_Repository;
        private const string key = "qaz123!@@)(*";
        public LoginController()
        {
            this._ILogin_Repository = new Login_Repository(new Repo.DbEntities.SOPasswordManagerContext());
            this._IUser_Repository = new User_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        public IActionResult Index()
        {           
            var MemberSession= HttpContext.Session.GetString("MemberSession");
            if(MemberSession!=null)
            {
                return RedirectToAction("ProjectUser", "Projects");
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            var pass = EncryptDecrypt.Decrypt("6tqRcWovza8=");
            
            var objData = await _ILogin_Repository.CheckLogin(Model.Email, Model.Password);
            if (objData != null)
            {
                if (objData.TwoStepVerification == true)
                {   
                    int oneTimeOTP = _IUser_Repository.GenerateRandom4DigitOTP();
                    _ILogin_Repository.SaveOTP(objData.Email, oneTimeOTP);
                    if (objData.Role != null)
                    {
                        HttpContext.Session.SetString("RoleName", objData.Role.RoleName.ToString());
                    }
                    await Task.Run(() => _ILogin_Repository.SendTwoStepVerificationMail(objData.Email, oneTimeOTP));
                    HttpContext.Session.SetString("EMAIL", (objData.Email).ToString());
                    return RedirectToAction("TwoStepVerification", "Login");
                }
                else
                {
                    HttpContext.Session.SetString("MemberSession", objData.SytemUserId.ToString());
                    HttpContext.Session.SetString("Member_Name", (objData.FirstName + " " + objData.LastName).ToString());
                    HttpContext.Session.SetString("EMAIL", (objData.Email).ToString());
                    HttpContext.Session.SetString("Role_ID", objData.RoleId.ToString());                 
                    HttpContext.Session.SetString("IsFirstLogin", objData.IsFirstLogin.ToString());
                    if (objData.Role != null)
                    {
                        HttpContext.Session.SetString("RoleName", objData.Role.RoleName.ToString());
                    }
                    TempData["Success"] = "Login Successfully.";
                }
            }
            else
            {
                TempData["Error"] = "Invalid email or password.";
                return View(Model);
            }
            return RedirectToAction("ProjectUser", "Projects");
        }

        public ActionResult TwoStepVerification()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult TwoStepVerification(TwoFactorModel model)
        {            
            var objData = context.SystemUser.Where(a => a.VerificationCode == model.OTP).FirstOrDefault();
            if (objData != null)
            {
                TempData["Success"] = "Login Successfully.";
                HttpContext.Session.SetString("MemberSession", objData.SytemUserId.ToString());
                HttpContext.Session.SetString("Member_Name", (objData.FirstName + " " + objData.LastName).ToString());
                HttpContext.Session.SetString("EMAIL", (objData.Email).ToString());
                HttpContext.Session.SetString("Role_ID", objData.RoleId.ToString());
                HttpContext.Session.SetString("IsFirstLogin", objData.IsFirstLogin.ToString());               
                return RedirectToAction("ProjectUser", "Projects");
            }
            else
            {
                TempData["Error"] = "You entered Invalid OTP.";
                return View();
            }
            
        }

        public async Task<ActionResult> ResendOTPAsync()
        {
            string email = HttpContext.Session.GetString("EMAIL");
            if (email == "" || email == null)
            {
                TempData["OTPResendFailure"] = "Please login first, to generate OTP.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int oneTimeOTP = _IUser_Repository.GenerateRandom4DigitOTP();
                _ILogin_Repository.SaveOTP(email, oneTimeOTP);                
                await Task.Run(() => _ILogin_Repository.SendTwoStepVerificationMail(email, oneTimeOTP));
                
                TempData["OTPResendSuccess"] = "OTP resent to your registered email.";
                return RedirectToAction("TwoStepVerification", "Login");
            }            
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("MemberSession");
            HttpContext.Session.Remove("Member_Name");
            HttpContext.Session.Remove("Role_ID");
            HttpContext.Session.Remove("EMAIL");
            HttpContext.Session.Remove("IsFirstLogin");
            HttpContext.Session.Remove("RoleName");
        
            return RedirectToAction("Index", "Login");
        }

        public ActionResult ChangePassword()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
            var obj = context.SystemUser.Where(x=>x.SytemUserId == id).FirstOrDefault();
            if(obj!=null)
            {
                if(obj.IsFirstLogin==true)
                {
                    TempData["ForceFullyChange"] = "Please change your password before perform any action.";
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel  model)
        {

           
            string cp = EncryptDecrypt.Encrypt(model.CurrentPassword);
            string np = EncryptDecrypt.Encrypt(model.NewPassword);
            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
            var obj= context.SystemUser.Where(x=>x.Password==cp && x.SytemUserId==id).FirstOrDefault();
            var RetValue = new { success = false, Message = "" };
            try
            {
                if (obj != null)
                {
                    obj.Password = np;
                    obj.IsFirstLogin = false;
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    HttpContext.Session.SetString("IsFirstLogin", obj.IsFirstLogin.ToString());
                    TempData["ForceFullyChange"] = "Password Changed Successfully";
                    //ViewBag.Message = "Password Changed Successfully";
                    return RedirectToAction("ProjectUser", "Projects");
                    //RetValue = new { success = true, Message = "Password Changed Successfully." };
                }
                else
                {
                    TempData["ForceFullyChange"] = "Current password is wrong.";
                   // RetValue = new { success = false, Message = "Current password is wrong." };
                }
            }
            catch (Exception ex)
            {
                TempData["ForceFullyChange"] =  ex.Message.ToString();
            }
            return View(model);

        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string password ,string Email)
        {           
            var searchUser = (from user in context.SystemUser where user.Email == Email select user).FirstOrDefault();         
           
            try
            {
                if(searchUser == null)
                {                   
                    ViewBag.EmailNotFound = "Entered e-mail is not found.";
                    return View();
                }
                else
                {
                    var objUser = (from user in context.SystemUser where user.Email == Email select new { user.FirstName, user.Password }).FirstOrDefault();

                   var mailBody=string.Format("Dear&nbsp;<b>" + objUser.FirstName + "</b>,<br />" +
                "<br />Your Password details are as below:</br></br>" +
                "<br/>Password: {0}</br>" + "<br/><br/>Thanks" + "<br/><b>SO-Password Manager</b>", EncryptDecrypt.Decrypt(objUser.Password));
                    Common.SendEmailAsync(Email, mailBody, "Password Recovery");
                    ViewBag.EmailSend = "Your password has been sent to your e-mail.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
            }
            return View();
        }

    }
}