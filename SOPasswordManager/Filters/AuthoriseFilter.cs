using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPasswordManager.Filters
{
    public class AuthoriseFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var rd = context.RouteData;
            if (context.HttpContext.Session.GetString("MemberSession") == null)
            {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Index");
                redirectTargetDictionary.Add("controller", "Login");
                context.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
            else
            {
                string IsFirstLogin = context.HttpContext.Session.GetString("IsFirstLogin").ToString();
                //if (IsFirstLogin != null || IsFirstLogin != "null" || IsFirstLogin != "" || IsFirstLogin != "False" || IsFirstLogin != "false")
                if (IsFirstLogin != "")
                {
                    bool isFirstlogin = Convert.ToBoolean(IsFirstLogin);
                    if (isFirstlogin)
                    {
                        RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                        redirectTargetDictionary.Add("action", "ChangePassword");
                        redirectTargetDictionary.Add("controller", "Login");
                        context.Result = new RedirectToRouteResult(redirectTargetDictionary);
                    }
                }
            }
        }
    }
}
