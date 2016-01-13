using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{
    public class CustomSwitch:SwitchCell
    {
        public static readonly BindableProperty CustomTextProperty = BindableProperty.Create<CustomSwitch, string>(p => p.Text, default(string));

        public string Text
        {
            get { return (String)GetValue(CustomTextProperty); }
            set { SetValue(CustomTextProperty, value); }
        }

    }
}
