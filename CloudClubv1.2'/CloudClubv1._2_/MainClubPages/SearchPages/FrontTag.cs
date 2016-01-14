using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{

    public class FrontTag :INotifyPropertyChanged
    {
        public string Tag { get; set; }
        public bool removeTag { get; set; }
        
        public FrontTag(string tag)
        {
            this.Tag = tag;
            removeTag = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateProperty(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
