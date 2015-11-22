using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CloudClubv1._2_;


namespace FrontEnd
{
    public class MyViewCell :ViewCell
    {
        public static readonly BindableProperty CornerRadiusProperty =  //This format allows for databinding to these properties
            BindableProperty.Create<MyViewCell, double>(p => p.CornerRadius, 0);

        public double CornerRadius
        {
            get { return (double)base.GetValue(CornerRadiusProperty); }
            set { base.SetValue(CornerRadiusProperty, value); }
        }

    }
}
