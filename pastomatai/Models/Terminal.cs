using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class Terminal
    {
        public Terminal()
        {
            Package = new HashSet<Package>();
        }

        public string PhoneNumber { get; set; }
        public int IdTerminal { get; set; }
        public int FkAddressidAddress { get; set; }

        public virtual Address FkAddressidAddressNavigation { get; set; }
        public virtual ICollection<Package> Package { get; set; }
    }
}
