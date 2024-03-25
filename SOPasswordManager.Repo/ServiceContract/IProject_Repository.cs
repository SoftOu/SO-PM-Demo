using Microsoft.AspNetCore.Mvc.Rendering;
using SOPasswordManager.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.ServiceContract
{
    public interface IProject_Repository : IDisposable
    {
        List<SelectListItem> GetClientProjects(int client_Id,int logedUserId);

        CredentialsDataViewModel GetCredentials(int projectUser_Id);
    }
}
