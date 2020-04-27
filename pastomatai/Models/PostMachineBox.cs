using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class PostMachineBox
    {
        public bool IsOpen { get; set; }
        public string Pin { get; set; }
        public string Size { get; set; }
        public int IdPostMachineBox { get; set; }
        public int FkPostMachineidPostMachine { get; set; }
        public int? FkPackageidPackage { get; set; }

        public virtual Package FkPackageidPackageNavigation { get; set; }
        public virtual PostMachine FkPostMachineidPostMachineNavigation { get; set; }
    }
}
