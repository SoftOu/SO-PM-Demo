using System;
using System.Collections.Generic;
using System.Text;

namespace SOPasswordManager.Repo.GridService
{
    public class GridDTO
    {
        public List<Dictionary<string, object>> data { get; set; }
        public string draw { get; set; }
        public string recordsTotal { get; set; }
        public string recordsFiltered { get; set; }
    }
}
