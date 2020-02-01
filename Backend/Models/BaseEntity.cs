using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Models
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
