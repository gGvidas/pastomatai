using System;
using System.Collections.Generic;

namespace pastomatai.Models
{
    public partial class Address
    {
        public Address()
        {
            EndUser = new HashSet<EndUser>();
            PostMachine = new HashSet<PostMachine>();
        }

        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string HouseNumber { get; set; }
        public int IdAddress { get; set; }

        public virtual Terminal Terminal { get; set; }
        public virtual ICollection<EndUser> EndUser { get; set; }
        public virtual ICollection<PostMachine> PostMachine { get; set; }
    }
}
