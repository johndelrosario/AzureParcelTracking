using System;
using System.Collections.Generic;
using System.Text;

namespace AzureParcelTracking.Application.Models
{
    public class UserRecord : BaseRecord
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
