package md583a5db7bf3698299f4dd7c4eaa4db5c5;


public class ContactsAdapter
	extends android.app.ListActivity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CloudClubv1._2_.Droid.ContactsAdapter, CloudClubv1._2_.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ContactsAdapter.class, __md_methods);
	}


	public ContactsAdapter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ContactsAdapter.class)
			mono.android.TypeManager.Activate ("CloudClubv1._2_.Droid.ContactsAdapter, CloudClubv1._2_.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
