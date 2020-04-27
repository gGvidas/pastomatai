using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class LoggedInUser
    {
        public LoggedInUser()
        {
            Message = new HashSet<Message>();
            Package = new HashSet<Package>();
            PostMachineFkLoggedInUseridEndUser1Navigation = new HashSet<PostMachine>();
            PostMachineFkLoggedInUseridEndUserNavigation = new HashSet<PostMachine>();
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int IdEndUser { get; set; }

        public virtual EndUser IdEndUserNavigation { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Package> Package { get; set; }
        public virtual ICollection<PostMachine> PostMachineFkLoggedInUseridEndUser1Navigation { get; set; }
        public virtual ICollection<PostMachine> PostMachineFkLoggedInUseridEndUserNavigation { get; set; }
    }
}
