using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;

namespace FrontEnd
{
    public class SaveFileDictionary
    {
        public Dictionary<string, int> dict;

        public SaveFileDictionary()
        {
            dict = new Dictionary<string, int>();
            dict.Add("USERID", 0);
            dict.Add("CLOUDREGION", 1);
        }


    }
}
