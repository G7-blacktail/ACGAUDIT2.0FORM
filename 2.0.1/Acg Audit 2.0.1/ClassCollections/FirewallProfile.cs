using System.Collections.Generic;

namespace ACG_AUDIT.ClassCollections
{
    public class FirewallProfile
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class FirewallProfileList
    {
        public List<FirewallProfile> Profiles { get; set; } = new List<FirewallProfile>();
    }
}