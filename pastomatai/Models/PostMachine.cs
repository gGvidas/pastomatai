using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class PostMachine
    {
        public PostMachine()
        {
            PostMachineBox = new HashSet<PostMachineBox>();
        }

        public bool TurnedOn { get; set; }
        public string PostMachineState { get; set; }
        public int IdPostMachine { get; set; }
        public int FkLoggedInUseridEndUser { get; set; }
        public int FkLoggedInUseridEndUser1 { get; set; }
        public int FkAddressidAddress { get; set; }

        public virtual Address FkAddressidAddressNavigation { get; set; }
        public virtual LoggedInUser FkLoggedInUseridEndUser1Navigation { get; set; }
        public virtual LoggedInUser FkLoggedInUseridEndUserNavigation { get; set; }
        public virtual ICollection<PostMachineBox> PostMachineBox { get; set; }
    }
}
