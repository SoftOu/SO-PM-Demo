using System;
using System.Collections.Generic;

namespace SOPasswordManager.DbEntities
{
    public partial class ProjectUser
    {
        public int ProjectUserId { get; set; }
        public int? ProjectId { get; set; }
        public int? SytemUserId { get; set; }

        public Projects Project { get; set; }
        public SystemUser SytemUser { get; set; }
    }
}
