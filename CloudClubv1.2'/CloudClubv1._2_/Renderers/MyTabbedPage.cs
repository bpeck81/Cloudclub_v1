using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{
    public class MyTabbedPage: TabbedPage
    {
        public MyTabbedPage()
        {
            ColorHandler ch = new ColorHandler();
            this.Padding = 1;
            BackgroundColor = ch.fromStringToColor("white");
        }

    }
}
