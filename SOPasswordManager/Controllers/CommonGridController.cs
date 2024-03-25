using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using SOPasswordManager.Models;
using SOPasswordManager.Repo.GridService;

namespace SOPasswordManager.Controllers
{
    public class CommonGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// For bind grid in whole project
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<GridDTO> GetGridData()
        {
            string requestData = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                requestData = reader.ReadToEnd();
            }
            NameValueCollection data = HttpUtility.ParseQueryString(requestData);
            string mode = Convert.ToString(data["mode"]);
            GridColumnsConfig og = new GridColumnsConfig(mode, data, "1=1");
            return await og.GetData();
        }
        /// <summary>
        /// For export data from grid.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ExportData()
        {
            string requestData = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                requestData = reader.ReadToEnd();
            }
            NameValueCollection data = HttpUtility.ParseQueryString(requestData);
            string mode = Convert.ToString(data["mode"]);
            GridColumnsConfig og = new GridColumnsConfig(mode, data, "");

            if (data["type"] == "excel")
            {
                return File(await og.Export(), "application/vnd.ms-excel", data["mode"] + ".xls");
            }
            if (data["type"] == "pdf")
            {
                return File(await og.Export(), "application/pdf", data["mode"] + ".pdf");
            }
            if (data["type"] == "csv")
            {
                return File(await og.Export(), "application/pdf", data["mode"] + ".csv");
            }

            return View();

        }
    }
}