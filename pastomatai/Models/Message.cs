using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class Message
    {
        public string Text { get; set; }
        public bool Sent { get; set; }
        public int IdMessage { get; set; }
        public int FkPackageidPackage { get; set; }
        public int FkLoggedInUseridEndUser { get; set; }

        public virtual LoggedInUser FkLoggedInUseridEndUserNavigation { get; set; }
        public virtual Package FkPackageidPackageNavigation { get; set; }
    }
}
