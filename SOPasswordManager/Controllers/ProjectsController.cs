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
using SOPasswordManager.Repo.DataService;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.GridService;
using SOPasswordManager.Repo.Service;
using SOPasswordManager.Repo.ServiceContract;

namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class ProjectsController : Controller
    {
        SOPasswordManagerContext context = new SOPasswordManagerContext();
        IClient_Repository _IClient_Repository;
        IProject_Repository _IProject_Repository;

        public ProjectsController()
        {
            _IClient_Repository = new Client_Repository(new Repo.DbEntities.SOPasswordManagerContext());
            _IProject_Repository = new Project_Repository(new Repo.DbEntities.SOPasswordManagerContext());
        }

        #region PROJECT

        public ActionResult Index()
        {
            //var ClientList = await _IClient_Repository.GetClientList();            

            if (HttpContext.Session.GetString("RoleName") == "Admin")
            {
                var ClientList = context.Clients.OrderBy(x => x.ClientName).ToList();
                ViewBag.ClientList = ClientList.Select(x => new SelectListItem
                {
                    Value = x.ClientId.ToString(),
                    Text = x.ClientName,
                }).ToList();

            }
            else
            {
                int logedUserId = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                var Client = (from c in context.Clients
                              join
p in context.Projects on c.ClientId equals p.ClientId
                              join
pu in context.ProjectUser on p.ProjectId equals pu.ProjectId
                              join
su in context.SystemUser on pu.SytemUserId equals su.SytemUserId
                              where su.SytemUserId == logedUserId
                              select c).Distinct().ToList();

                ViewBag.ClientList = Client.Select(x => new SelectListItem
                {
                    Value = x.ClientId.ToString(),
                    Text = x.ClientName,
                }).Distinct().ToList();
            }

            var IsExistingSearch = (TempData["IsExistingProjectSearch"] != null ? TempData["IsExistingProjectSearch"].ToString() : "No");
            TempData["inputStrsearch"] = (IsExistingSearch == "Yes" ? (TempData["inputStrsearch"]) : "");

            return View();
        }

        [HttpPost]
        public async Task<GridDTO> GetProjects()
        {
            string requestData = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                requestData = reader.ReadToEnd();
            }

            NameValueCollection data = HttpUtility.ParseQueryString(requestData);
            string mode = Convert.ToString(data["mode"]);
            //var ProjectID = Convert.ToInt32(data["SearchParams[Project_ID]"]);
            var Client_ID = Convert.ToString(data["SearchParams[Client_ID]"]);
            //HttpContext.Session.SetString("SearchProject_ID", ProjectID.ToString());
            //HttpContext.Session.SetString("SearchClient_ID", Client_ID.ToString());

            string Search = Convert.ToString(data["search[value]"]);
            TempData["IsNewRequest"] = false;

            TempData["inputStrsearch"] = Search;
            GridColumnsConfig og = new GridColumnsConfig(mode, data, GetProjDataWhereCause(Client_ID));
            return await og.GetData();
        }

        public string GetProjDataWhereCause(string Client_ID)
        {
            string w = "1=1";
            if (Client_ID != "")
            {
                // TODO : we assign static "P." because are query.
                w += " AND P.Client_ID IN (" + Client_ID + ")";
            }
            return w;
        }


        [HttpGet]
        public ActionResult AddProject(int id = 0)
        {
            //var Clientlist = context.Clients.OrderBy(x => x.ClientName).ToList();
            if (HttpContext.Session.GetString("RoleName") == "Admin")
            {
                var ClientList = context.Clients.OrderBy(x => x.ClientName).ToList();
                ViewBag.ClientList = ClientList.Select(x => new SelectListItem
                {
                    Value = x.ClientId.ToString(),
                    Text = x.ClientName,
                }).ToList();

            }
            else
            {
                int logedUserId = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                var Client = (from c in context.Clients
                              join
p in context.Projects on c.ClientId equals p.ClientId
                              join
pu in context.ProjectUser on p.ProjectId equals pu.ProjectId
                              join
su in context.SystemUser on pu.SytemUserId equals su.SytemUserId
                              where su.SytemUserId == logedUserId
                              select c).Distinct().ToList();

                ViewBag.ClientList = Client.Select(x => new SelectListItem
                {
                    Value = x.ClientId.ToString(),
                    Text = x.ClientName,
                }).Distinct().ToList();
            }

            if (id == 0)
            {
                ProjectModel model = new ProjectModel();
                context.Projects.FindAsync(id);
                return PartialView(model);
            }
            else
            {
                ProjectModel model = new ProjectModel();
                var objData = context.Projects.Find(id);
                model.ProjectId = objData.ProjectId;
                model.ProjectName = objData.ProjectName;
                model.Description = objData.Description;
                model.ClientId = objData.ClientId;
                return PartialView(model);
            }
        }


        [HttpPost]
        public ActionResult AddProject(ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            TempData["IsExistingProjectSearch"] = "Yes";

            if (model.ProjectId == 0)
            {
                Projects obj = new Projects();
                obj.ProjectName = model.ProjectName;
                obj.Description = model.Description;
                obj.ClientId = model.ClientId;
                obj.DateCreated = DateTime.Now;
                obj.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                context.Set<Projects>().Add(obj);
                context.SaveChanges();

                ProjectUser objProjectUser = new ProjectUser();
                objProjectUser.ProjectId = obj.ProjectId;
                objProjectUser.SytemUserId = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                context.Set<ProjectUser>().Add(objProjectUser);
                context.SaveChanges();

                TempData["ProjectAddSuccess"] = "Project details successfully inserted.";
                return Json(new { success = true, url = Url.Action("Index", "Projects") });
            }
            else
            {
                Projects obj = context.Projects.Find(model.ProjectId);
                obj.ProjectName = model.ProjectName;
                obj.Description = model.Description;
                obj.ClientId = model.ClientId;
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
                TempData["ProjectEditSuccess"] = "Project details successfully updated.";
                return Json(new { success = true, url = Url.Action("Index", "Projects") });
            }
        }

        public JsonResult DeleteProject(int projectId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                var project = new Projects { ProjectId = projectId };
                context.Entry(project).State = EntityState.Deleted;
                context.SaveChanges();
                RetValue = new { success = true, Message = "Project successfully deleted." };

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

        [HttpGet]
        public PartialViewResult AddProjectUserData(string Id)
        {
            Models.AddProjectUser user = new AddProjectUser();
            int Project_ID = Convert.ToInt32(Id);
            ViewBag.id = Project_ID;
            return PartialView(user);
        }

        [HttpPost]
        public JsonResult AddProjectUserData(AddProjectUser model)
        {
            int arrLength = model.SytemUserId.Length;
            for (int i = 0; i < arrLength; i++)
            {
                ProjectUser obj = new ProjectUser();
                obj.ProjectId = model.projectId;
                obj.SytemUserId = model.SytemUserId[i];
                context.Set<ProjectUser>().Add(obj);
                context.SaveChanges();
            }
            //TempData["ProjectUserAddSuccess1"] = "Project User successfully inserted.";           
            return Json(new { success = true, url = Url.Action("Index", "Projects") });
        }


        public JsonResult GetProjectUserDataList(int projectId = 0)
        {
            var projectUser = context.ProjectUser.Where(x => x.ProjectId == projectId).Select(x => x.SytemUserId).ToList();
            var Users = context.SystemUser.Where(x => projectUser.Contains(x.SytemUserId) == false).Select(x => new SelectListItem
            {
                Value = x.SytemUserId.ToString(),
                Text = x.FirstName
            }).ToList();
            return Json(Users);
        }

        public JsonResult DeleteUser(int ProjectUser_ID)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                var systemUser = new ProjectUser { ProjectUserId = ProjectUser_ID };
                context.Entry(systemUser).State = EntityState.Deleted;
                context.SaveChanges();
                RetValue = new { success = true, Message = "User successfully deleted." };

            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }
            return Json(RetValue);
        }

        #endregion


        #region PROJECTUSER

        public async Task<IActionResult> ProjectUser()
        {
            List<SelectListItem> Projects = new List<SelectListItem>();
            List<SelectListItem> Clients = new List<SelectListItem>();
            Clients.Add(new SelectListItem { Text = "Select Client", Value = "0" });
            int logedUserId = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
            var IsExistingSearch = (TempData["IsExistingSearch"] != null ? TempData["IsExistingSearch"].ToString() : "No");
            TempData["inputStr"] = (IsExistingSearch == "Yes" ? (TempData["inputStr"]) : "");
            var Client_ID = (IsExistingSearch == "Yes" ? Convert.ToInt32(HttpContext.Session.GetString("SearchClient_ID")) : 0);
            var Project_ID = (IsExistingSearch == "Yes" ? Convert.ToInt32(HttpContext.Session.GetString("SearchProject_ID")) : 0);
            if (HttpContext.Session.GetString("RoleName") == "Admin")
            {

                var Clients1 = context.Clients.OrderBy(x => x.ClientName).ToList();

                Clients.AddRange(Clients1.Select(x => new SelectListItem
                {
                    Value = x.ClientId.ToString(),
                    Text = x.ClientName,
                }).Distinct().ToList());

            }
            else
            {


                Clients.AddRange((from c in context.Clients
                                  join p in context.Projects on c.ClientId equals p.ClientId
                                  join pu in context.ProjectUser on p.ProjectId equals pu.ProjectId
                                  join su in context.SystemUser on pu.SytemUserId equals su.SytemUserId
                                  where su.SytemUserId == logedUserId
                                  select new SelectListItem
                                  {
                                      Value = c.ClientId.ToString(),
                                      Text = c.ClientName,
                                  }).Distinct().ToList());
            }

            ViewBag.ClientList = Clients;
            ViewBag.Project_ID = Project_ID;

            Projects.Add(new SelectListItem { Text = "Select Projects", Value = "0" });
            var LoginUser = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
            if (Client_ID > 0)
            {
                Projects.AddRange((from projects in context.Projects
                                   join projectUser in context.ProjectUser on projects.ProjectId equals projectUser.ProjectId
                                   where projects.ClientId == Client_ID && projectUser.SytemUserId == LoginUser
                                   orderby projects.ProjectName
                                   select new SelectListItem
                                   {
                                       Text = projects.ProjectName,
                                       Value = projects.ProjectId.ToString()
                                   }).ToList());
            }
            else
            {
                try
                {
                    //Add Projects to Dropdownlist
                    Projects.AddRange((from projects in context.Projects
                                       join projectUser in context.ProjectUser on projects.ProjectId equals projectUser.ProjectId
                                       where projectUser.SytemUserId == LoginUser
                                       orderby projects.ProjectName
                                       select new SelectListItem
                                       {
                                           Text = projects.ProjectName,
                                           Value = projects.ProjectId.ToString()
                                       }).ToList());
                }
                catch (Exception ex)
                {

                }
            }

            ViewBag.ClientProjects = Projects;
            return View();
        }

        [HttpGet]
        public IActionResult AddProjectUser(int id = 0, int sProjectId = 0)
        {
            var LoginUser = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
            ViewBag.ProjectList = (from projects in context.Projects
                                   join projectUser in context.ProjectUser on projects.ProjectId equals projectUser.ProjectId
                                   where projectUser.SytemUserId == LoginUser
                                   orderby projects.ProjectName
                                   select new SelectListItem
                                   {
                                       Text = projects.ProjectName,
                                       Value = projects.ProjectId.ToString()
                                   }).ToList();

            if (id == 0)
            {
                return PartialView(new ProjectUserModel
                {
                    projectId = sProjectId
                });
            }

            ProjectUserModel model = new ProjectUserModel();
            var objData = context.ProjectData.Find(id);
            model.ProjectUserId = objData.ProjectUserId;
            model.projectId = objData.ProjectId;
            model.Description = objData.Description;
            model.Url = objData.Url;
            model.Name = objData.Name;

            if (!string.IsNullOrEmpty(objData.UserName))
                model.UserName = EncryptDecrypt.Decrypt(objData.UserName);

            if (!string.IsNullOrEmpty(objData.Email))
                //model.Email = objData.Email;
                model.Email = EncryptDecrypt.Decrypt(objData.Email);

            if (!string.IsNullOrEmpty(objData.Password))
                model.Password = EncryptDecrypt.Decrypt(objData.Password);

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult AddProjectUser(ProjectUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            TempData["IsExistingSearch"] = "Yes";

            var RetValue = new { success = false, Message = "" };
            try
            {
                if (model.ProjectUserId == 0)
                {
                    ProjectData obj = new ProjectData
                    {
                        ProjectId = model.projectId,
                        Description = model.Description,
                        Url = model.Url,
                        Name = model.Name
                    };

                    if (!string.IsNullOrEmpty(model.Email))
                        obj.Email = EncryptDecrypt.Encrypt(model.Email);

                    if (!string.IsNullOrEmpty(model.UserName))
                        obj.UserName = EncryptDecrypt.Encrypt(model.UserName);

                    if (!string.IsNullOrEmpty(model.Password))
                        obj.Password = EncryptDecrypt.Encrypt(model.Password);

                    obj.DateCreated = DateTime.Now;
                    obj.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                    context.Set<ProjectData>().Add(obj);
                    context.SaveChanges();
                    TempData["ProjectUserAddSuccess"] = "Project details successfully inserted.";

                    //return Json(new { success = true, url = Url.Action("ProjectUser", "Projects") });
                }
                else
                {
                    ProjectData obj = context.ProjectData.Find(model.ProjectUserId);
                    obj.ProjectId = model.projectId;
                    obj.Description = model.Description;
                    obj.Url = model.Url;
                    obj.Name = model.Name;

                    if (!string.IsNullOrEmpty(model.Email))
                        obj.Email = EncryptDecrypt.Encrypt(model.Email);

                    if (!string.IsNullOrEmpty(model.UserName))
                        obj.UserName = EncryptDecrypt.Encrypt(model.UserName);

                    if (!string.IsNullOrEmpty(model.Password))
                        obj.Password = EncryptDecrypt.Encrypt(model.Password);

                    obj.DateUpdated = DateTime.Now;
                    obj.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    TempData["ProjectUserEditSuccess"] = "Project details successfully updated.";
                    TempData["IsNewRequest"] = true;
                    //return Json(new { success = true, url = Url.Action("ProjectUser", "Projects") });
                }
            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }

            return Json(new { success = true, url = Url.Action("ProjectUser", "Projects") });
        }

        public JsonResult DeleteProjectUser(int projectUserId)
        {
            var RetValue = new { success = false, Message = "" };
            try
            {
                var projectUser = new ProjectData { ProjectUserId = projectUserId };
                context.Entry(projectUser).State = EntityState.Deleted;
                context.SaveChanges();
                RetValue = new { success = true, Message = "Project details successfully deleted." };

            }
            catch (Exception ex)
            {
                RetValue = new { success = false, Message = ex.Message.ToString() };
            }
            return Json(RetValue);
        }

        public JsonResult GetClientUser(int projectId)
        {
            var clientIds = (from project in context.Projects where project.ProjectId == projectId select new { project.ClientId }).FirstOrDefault();
            var clientUserIds = (from clientUser in context.ClientUser where clientUser.ClientId == clientIds.ClientId select new { clientUser.ClientUserId, clientUser.FirstName, clientUser.LastName }).ToList();
            return Json(clientUserIds);
        }

        public JsonResult GetProjectlist()
        {
            var projects = (from project in context.Projects select project).ToList();
            return Json(projects);
        }

        public IActionResult GetClientsProjects(int client_Id)
        {
            int logedUserId = Convert.ToInt32(HttpContext.Session.GetString("MemberSession"));
            var ClientProjects = _IProject_Repository.GetClientProjects(client_Id, logedUserId);
            return Json(ClientProjects);
        }

        public IActionResult GetCredentials(int projectUser_Id)
        {
            var Credentials = _IProject_Repository.GetCredentials(projectUser_Id);
            Credentials.UserName = EncryptDecrypt.Decrypt(Credentials.UserName);
            Credentials.Password = EncryptDecrypt.Decrypt(Credentials.Password);
            return PartialView("_UserCredentials", Credentials);
        }

        public JsonResult GetPassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                password = EncryptDecrypt.Decrypt(password);
            }
            return Json(password);
        }

        [HttpPost]
        public async Task<GridDTO> GetProjectData()
        {
            string requestData;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                requestData = await reader.ReadToEndAsync();
            }

            NameValueCollection data = HttpUtility.ParseQueryString(requestData);
            string mode = Convert.ToString(data["mode"]);
            var projectId = Convert.ToInt32(data["SearchParams[Project_ID]"]);
            var clientId = Convert.ToInt32(data["SearchParams[Client_ID]"]);
            HttpContext.Session.SetString("SearchProject_ID", projectId.ToString());
            HttpContext.Session.SetString("SearchClient_ID", clientId.ToString());

            string search = Convert.ToString(data["search[value]"]);
            TempData["IsNewRequest"] = false;

            TempData["inputStr"] = search;
            GridColumnsConfig og = new GridColumnsConfig(mode, data, GetProjectDataWhereCause(projectId));

            return await og.GetData();
        }

        public string GetProjectDataWhereCause(int ProjectID)
        {
            string w = "1=1";
            if (ProjectID != 0)
            {
                //AAAA modified
                //w += " AND Project_ID = " + ProjectID;
                w += " AND P.Project_ID = " + ProjectID;
            }
            return w;
        }

        #endregion

        public JsonResult GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
        {
            var RetValue = new { success = false, Message = "" };
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%&*@\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                //return "Password length must be between 8 and 128.";
                RetValue = new { success = true, Message = "Password length must be between 8 and 128." };
            }

            string characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            System.Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                bool moreThanTwoIdenticalInARow =
                    characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }
            RetValue = new { success = true, Message = string.Join(null, password) };
            return Json(RetValue);
        }
    }
}