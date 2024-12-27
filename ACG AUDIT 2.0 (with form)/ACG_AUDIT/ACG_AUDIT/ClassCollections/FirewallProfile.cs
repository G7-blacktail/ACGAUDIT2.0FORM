using System.Collections.Generic;

namespace ACG_AUDIT.ClassCollections
{
    internal class FirewallProfile
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

    internal class FirewallProfileList
    {
        public List<FirewallProfile> Profiles { get; set; } = new List<FirewallProfile>();
    }
}