using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analog_Tamigo_API.Models.Responses
{
    public class Contact
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public object LastName { get; set; }
        public string MobilePhone { get; set; }
    }

}