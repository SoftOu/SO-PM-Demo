using SOPasswordManager.Repo.GridService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SOPasswordManager.Models
{
    public class GridColumnsConfig
    {
        Repo.GridService.GridFunctions gridFunctions = new Repo.GridService.GridFunctions();

        public GridColumnsConfig(string mode, NameValueCollection requestData, string whereClause = "")
        {
            gridFunctions.HttpContext = requestData;
            gridFunctions.Mode = mode;
            gridFunctions.PageNumber = 1;
            gridFunctions.RecordPerPage = 10;
            gridFunctions.SortOrder = "desc";
            if (mode == "Clients")
            {
                gridFunctions.TableName = "Clients C LEFT JOIN County CO ON C.Country_ID=CO.Country_ID LEFT JOIN City CT ON C.City_ID = CT.City_ID";
                //gridFunctions.ColumnsName = @"C.Client_ID,C.ClientName,CT.City_ID,CO.Country_ID,CO.County_Name AS Country , CT.City_Name AS City,
                //                              STUFF((
                //                                SELECT ',' + Cast(ContactId as varchar(5))
                //                                FROM ClientContacts CC

                //                                WHERE CC.ClientID = C.Client_ID
                //                                FOR XML PATH('')
                //                              ), 1, 1, '') AS ContactId";
                gridFunctions.ColumnsName = @"C.Client_ID,C.ClientName,CT.City_ID,CO.Country_ID,CO.County_Name AS Country , CT.City_Name AS City,
                                              (SELECT GROUP_CONCAT(DISTINCT ContactID) ContactID 
							                    FROM ClientContacts CC
							                    WHERE CC.ClientID = C.Client_ID) AS ContactId";
                gridFunctions.SortColumn = "ClientName";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.Client_ID,C.ClientName,CT.City_ID,CO.Country_ID,CO.County_Name AS Country , CT.City_Name AS City";
            }

            else if (mode == "ClientUser")
            {
                gridFunctions.TableName = "ClientUser CU LEFT JOIN Clients C ON CU.Client_ID=C.Client_ID  JOIN UserRole UR ON UR.Role_ID = CU.Role_ID";
                gridFunctions.ColumnsName = "C.Client_ID,CU.ClientUser_ID,C.ClientName AS Client,CU.Email,CU.PhoneNumber,CU.FirstName + ' ' + CU.LastName AS Name,UR.Role_Name AS Role";
                gridFunctions.SortColumn = "Name";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "CU.ClientUser_ID,C.ClientName AS Client,CU.Email,CU.PhoneNumber,CU.FirstName + ' ' + CU.LastName AS Name,UR.Role_Name AS Role";
                gridFunctions.ExportedFileName = "ClientUserList";
            }
            else if (mode == "ClientContactList")
            {
                gridFunctions.TableName = "Contacts C INNER JOIN ClientContacts CC ON C.Contact_ID=CC.ContactID";
                gridFunctions.ColumnsName = "C.Contact_ID,C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Role_ID,C.Notes,CC.ClientID ";
                gridFunctions.SortColumn = "Contact_ID";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.Contact_ID,C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Role_ID,C.Notes,CC.ClientID ";
                gridFunctions.ExportedFileName = "ClientContactsList";
            }
            else if (mode == "Projects")
            {
                HttpContextAccessor _HttpContextAccessor = new HttpContextAccessor();
                int User_ID = Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("MemberSession"));
                gridFunctions.TableName = "Projects P JOIN ProjectUser PU ON PU.Project_ID=P.Project_ID AND PU.SytemUser_ID=" + User_ID + " JOIN Clients C ON P.Client_ID=C.Client_ID";
                gridFunctions.ColumnsName = "P.Project_ID,P.Project_Name,P.Description,C.ClientName,P.Client_ID";
                gridFunctions.SortColumn = "Project_Name";
                gridFunctions.WhereClause = whereClause;
                gridFunctions.ExportedColumns = "P.Project_Name,P.Description,C.ClientName";
                gridFunctions.ExportedFileName = "ProjectList";
            }
            else if (mode == "ProjectUsers")
            {
                HttpContextAccessor _HttpContextAccessor = new HttpContextAccessor();
                int User_ID = Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("MemberSession"));
                gridFunctions.TableName = "ProjectData PU JOIN Projects P ON PU.Project_ID=P.Project_ID JOIN ProjectUser PUS ON PUS.Project_ID=P.Project_ID AND PUS.SytemUser_ID=" + User_ID + "";
                gridFunctions.ColumnsName = "P.Project_ID,P.Project_Name,PU.ProjectUser_ID,Name,Email,URL,User_Name,Password,PU.Description";
                gridFunctions.SortColumn = "FirstName";
                gridFunctions.WhereClause = whereClause;
                gridFunctions.ExportedColumns = "P.Project_Name,Name,Email,URL,User_Name,Password,PU.Description";
                gridFunctions.ExportedFileName = "ProjectUserList";
            }
            else if (mode == "ClientContacts")
            {
                gridFunctions.TableName = "Contacts C LEFT JOIN UserRole R ON C.Role_ID = R.Role_ID";
                gridFunctions.ColumnsName = "C.Contact_ID,C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Notes,R.Role_ID,R.Role_Name";
                gridFunctions.SortColumn = "Name";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Notes,R.Role_Name";
                gridFunctions.ExportedFileName = "Client Contacts List";
            }

            else if(mode== "User")
            {
                gridFunctions.TableName = "SystemUser SU LEFT JOIN UserRole R ON SU.Role_ID = R.Role_ID";
                gridFunctions.ColumnsName = "SU.SytemUser_ID,CONCAT(SU.FirstName,' ', SU.LastName) AS Name,SU.Email,SU.PhoneNumber,R.Role_Name,CASE WHEN Status=1 THEN 'Active' ELSE 'InActive' END AS Status";
                gridFunctions.SortColumn = "Name";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Notes,R.Role_Name";
                gridFunctions.ExportedFileName = "User List";
            }


            else if (mode == "ProjectUserList")
            {
                gridFunctions.TableName = "ProjectUser P LEFT JOIN SystemUser S ON P.SytemUser_ID = S.SytemUser_ID";
                gridFunctions.ColumnsName = "S.SytemUser_ID,S.FirstName + ' ' + S.LastName AS Name,S.Email,S.PhoneNumber,S.Status,P.ProjectUser_ID,P.Project_ID";
                gridFunctions.SortColumn = "SytemUser_ID";
                gridFunctions.WhereClause = whereClause;
                gridFunctions.ExportedColumns = "S.SytemUser_ID[Hidden],S.FirstName + ' ' + S.LastName AS Name,S.Email,S.PhoneNumber,S.Status,P.ProjectUser_ID[Hidden],P.Project_ID[Hidden]";
                gridFunctions.ExportedFileName = "Project User List";
            }
            else if (mode == "Country")
            {
                gridFunctions.TableName = " ";
                gridFunctions.ColumnsName = "Country_ID,County_Name AS Country"; 
                gridFunctions.SortColumn = "Country_ID";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "Country_ID[Hidden],County_Name AS Country";
                gridFunctions.ExportedFileName = "Country List";
            }
            else if (mode == "City")
            {
                gridFunctions.TableName = "County C INNER JOIN City CC ON C.Country_ID = CC.Country_ID";
                gridFunctions.ColumnsName = "C.Country_ID,C.County_Name AS Country,CC.City_ID,CC.City_Name AS City";
                gridFunctions.SortColumn = "City_ID";
                gridFunctions.WhereClause = whereClause;
                gridFunctions.ExportedColumns = "C.Country_ID,C.County_Name AS Country,CC.City_ID,CC.City_Name AS City";
                gridFunctions.ExportedFileName = "City List";
            }
            else if (mode == "Providers")
            {
                gridFunctions.TableName = "Providers C LEFT JOIN County CO ON C.Country_ID=CO.Country_ID LEFT JOIN City CT ON C.City_ID = CT.City_ID";
                //gridFunctions.ColumnsName = @"C.Provider_ID,C.ProviderName,CT.City_ID,CO.Country_ID,CO.County_Name AS Country , CT.City_Name AS City, C.BillingFullName, C.IdCard, C.FullAddress, C.PostalCode,
                //                              STUFF((
                //                                SELECT ',' + Cast(ProviderContact_ID as varchar(5))
                //                                FROM ProviderContact CC

                //                                WHERE CC.Provider_ID = C.Provider_ID
                //                                FOR XML PATH('')
                //                              ), 1, 1, '') AS ProviderContact_ID";
                gridFunctions.ColumnsName = @"C.Provider_ID,C.ProviderName,CT.City_ID,CO.Country_ID,CO.County_Name AS Country , CT.City_Name AS City, C.BillingFullName, C.IdCard, C.FullAddress, C.PostalCode,
                                              (SELECT GROUP_CONCAT(DISTINCT ProviderContact_ID) ProviderContact_ID
                                                FROM ProviderContact CC
                                                WHERE CC.Provider_ID = C.Provider_ID) AS ProviderContact_ID";
                gridFunctions.SortColumn = "ProviderName";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.Provider_ID,C.ProviderName,CT.City_ID,CO.Country_ID,CO.County_Name AS Country , CT.City_Name AS City, C.BillingFullName, C.IdCard, C.FullAddress, C.PostalCode";
                gridFunctions.ExportedFileName = "ProviderList";
            }
            else if (mode == "ProviderContactList")
            {
                gridFunctions.TableName = "ProviderContactDetail C INNER JOIN ProviderContact CC ON C.ProviderContactDetail_ID=CC.ProviderContactDetail_ID";
                gridFunctions.ColumnsName = "C.ProviderContactDetail_ID,C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Role_ID,C.Notes,CC.Provider_ID ";
                gridFunctions.SortColumn = "ProviderContactDetail_ID";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.ProviderContactDetail_ID,C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Role_ID,C.Notes,CC.Provider_ID ";
                gridFunctions.ExportedFileName = "ProviderContactsList";
            }
            else if (mode == "ProviderContacts")
            {
                gridFunctions.TableName = "ProviderContactDetail C LEFT JOIN UserRole R ON C.Role_ID = R.Role_ID";
                gridFunctions.ColumnsName = "C.ProviderContactDetail_ID,C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Notes,R.Role_ID,R.Role_Name";
                gridFunctions.SortColumn = "Name";
                gridFunctions.WhereClause = "";
                gridFunctions.ExportedColumns = "C.Name,C.Surname,C.Email1,C.Email2,C.PhoneNumber1,C.PhoneNumber2,C.Notes,R.Role_Name";
                gridFunctions.ExportedFileName = "Provider Contacts List";
            }
               
        }

        public async Task<GridDTO> GetData()
        {
            return await gridFunctions.GetJson();
        }

        public async Task<byte[]> Export()
        {
            return await this.gridFunctions.Export();
        }
    }
}
