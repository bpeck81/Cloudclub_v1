using System;

namespace Backend
{
	public class PhoneContact
	{
		public string FirstName{ get; set;}
		public string LastName{ get; set;}
		public string PhoneNumber{ get; set;}

		public PhoneContact (string firstName, string lastName, string phoneNumber){
			FirstName=firstName;
			LastName = lastName;
			PhoneNumber = phoneNumber;
		}

		public override string ToString()
		{
			return FirstName + " " + LastName + ": " + PhoneNumber;
		}


	}
}

