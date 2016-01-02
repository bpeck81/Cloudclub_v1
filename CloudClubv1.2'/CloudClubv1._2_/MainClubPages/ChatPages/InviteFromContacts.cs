using Plugin.Contacts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{
    public class InviteFromContacts:ContentPage
    {
        List<Contact> contactList;
        public InviteFromContacts(List<Contact> contactList)
        {
            this.contactList = contactList;
            var lContact = new Label
            {
               // Text = contactList[0].DisplayName

            };
            this.Content = lContact;   
        }

    }
}
