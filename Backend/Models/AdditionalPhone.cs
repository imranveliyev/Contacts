using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Models
{
    public class AdditionalPhone
    {
        public string Phone { get; set; }
        public string ContactId { get; set; }

        public Contact Contact { get; set; }
    }
}
