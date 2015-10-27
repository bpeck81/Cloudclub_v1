using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    public class DBImage : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string ContainerName { get; set; }

        public string ResourceName { get; set; }

        public string SasQueryString { get; set; }

        public string DBImageUri { get; set; }

        public DBImage()
        {
            SasQueryString = "";
            DBImageUri = "";
        }
    }
}