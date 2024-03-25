using System;
using System.Collections.Generic;

namespace SOPasswordManager.DbEntities
{
    public partial class ProjectData
    {
        public int ProjectUserId { get; set; }
        public int? ProjectId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }

        public Projects Project { get; set; }
    }
}
