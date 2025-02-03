﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACG_AUDIT.ClassCollections
{
    internal class InstalledSoftware
    {
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? Vendor { get; set; }

    }
    internal class InstalledSoftwareList
    {
        public List<InstalledSoftware> SoftwareList { get; set; } = new List<InstalledSoftware>();
    }
}
