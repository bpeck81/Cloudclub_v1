using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class SearchArray
    {
        public string CloudId { get; set; }

        public List<string> Tags { get; set; }

        public SearchArray(List<string> tags, string cloudId) {
            Tags = tags;
            CloudId = cloudId;
        }
    }
}
