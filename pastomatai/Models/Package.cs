using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class Package
    {
        public Package()
        {
            Message = new HashSet<Message>();
        }

        public DateTime? PutInTime { get; set; }
        public DateTime? CollectionTime { get; set; }
        public string Size { get; set; }
        public string PackageState { get; set; }
        public string Email { get; set; }
        public string ReceiversNumber { get; set; }
        public int IdPackage { get; set; }
        public int? FkLoggedInUseridEndUser { get; set; }
        public int? FkTerminalidTerminal { get; set; }
        public int FkEndUseridEndUser { get; set; }

        public virtual EndUser FkEndUseridEndUserNavigation { get; set; }
        public virtual LoggedInUser FkLoggedInUseridEndUserNavigation { get; set; }
        public virtual Terminal FkTerminalidTerminalNavigation { get; set; }
        public virtual PostMachineBox PostMachineBox { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
