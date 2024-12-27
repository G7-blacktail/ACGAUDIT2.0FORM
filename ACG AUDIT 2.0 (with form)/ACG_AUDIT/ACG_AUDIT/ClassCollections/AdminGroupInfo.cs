using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG_AUDIT.ClassCollections
{
    internal class AdminGroupInfo
    {
        public List<string> LocalAdmins { get; set; } = new List<string>();
        public List<string> DomainAdmins { get; set; } = new List<string>();
    }
}
