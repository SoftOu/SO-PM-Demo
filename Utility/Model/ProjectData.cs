using System;

namespace Utility.Model
{
    public class ProjectData
    {
        public int ProjectUser_ID { get; set; }
        public int? Project_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public DateTime? Date_Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Date_Updated { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
