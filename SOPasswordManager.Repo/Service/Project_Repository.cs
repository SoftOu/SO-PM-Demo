using Microsoft.AspNetCore.Mvc.Rendering;
using SOPasswordManager.Repo.DataService;
using SOPasswordManager.Repo.DbEntities;
using SOPasswordManager.Repo.DTO;
using SOPasswordManager.Repo.ServiceContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace SOPasswordManager.Repo.Service
{
    public class Project_Repository : IProject_Repository, IDisposable
    {
        SOPasswordManagerContext context;

        public Project_Repository(SOPasswordManagerContext _Context)
        {
            context = _Context;
        }

        public  List<SelectListItem> GetClientProjects(int client_Id ,int logedUserId)
        {
           
            if (client_Id==0)
            {
                return context.Projects.OrderBy(x=>x.ProjectName).Select(x => new SelectListItem
                {
                    Text = x.ProjectName,
                    Value = x.ProjectId.ToString()
                }).ToList();
            }
            else
            {
                      
                var d= (from c in context.Clients join p in context.Projects on c.ClientId equals p.ClientId join pu in context.ProjectUser on p.ProjectId
                       equals pu.ProjectId join su in context.SystemUser on pu.SytemUserId equals su.SytemUserId where su.SytemUserId == logedUserId
                        select new SelectListItem
                        {
                            Value = p.ProjectId.ToString(),
                            Text = p.ProjectName.ToString(),
                        }).Distinct().ToList();
                return d;
                //return context.Projects.OrderBy(x => x.ProjectName).Where(x => x.ClientId == client_Id).Select(x => new SelectListItem
                //{
                //    Text = x.ProjectName,
                //    Value = x.ProjectId.ToString()
                //}).ToList();
            }
        }

        public CredentialsDataViewModel GetCredentials(int projectUser_Id)
        {

            var credentialDetail = (from projectData in context.ProjectData
                                    where projectData.ProjectUserId == projectUser_Id
                                    select new CredentialsDataViewModel
                                    {
                                        URL=projectData.Url,
                                        UserName = projectData.UserName != null ? projectData.UserName : "",
                                        Description = projectData.Description,
                                        //Password = projectData.Password != null ? EncryptDecrypt.Decrypt(projectData.Password) : ""
                                        Password = projectData.Password != null ? projectData.Password : ""

                                    }).FirstOrDefault();
          
            return credentialDetail;

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        
        #endregion
    }
}
