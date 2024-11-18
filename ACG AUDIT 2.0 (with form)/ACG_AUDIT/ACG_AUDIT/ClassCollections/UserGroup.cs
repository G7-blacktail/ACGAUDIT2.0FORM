using System.Collections.Generic;

namespace ACG_AUDIT.ClassCollections
{
    internal class UserGroup
    {
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Domain { get; set; }
        public bool IsLocalAccount { get; set; }
        public List<string> Groups { get; set; } = new List<string>();
    }

    internal class UserGroupList
    {
        public List<UserGroup> Users { get; set; } = new List<UserGroup>();
    }
}