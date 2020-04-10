using System;
using System.Collections.Generic;
using System.Text;

namespace AzureParcelTracking.Commands.Models
{
    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Postcode { get; set; }
    }
}
