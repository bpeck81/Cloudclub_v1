package md583a5db7bf3698299f4dd7c4eaa4db5c5;


public class CustomSearchBarRenderer
	extends md5530bd51e982e6e7b340b73e88efe666e.SearchBarRenderer
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CloudClubv1._2_.Droid.CustomSearchBarRenderer, CloudClubv1._2_.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CustomSearchBarRenderer.class, __md_methods);
	}


	public CustomSearchBarRenderer (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == CustomSearchBarRenderer.class)
			mono.android.TypeManager.Activate ("CloudClubv1._2_.Droid.CustomSearchBarRenderer, CloudClubv1._2_.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public CustomSearchBarRenderer (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == CustomSearchBarRenderer.class)
			mono.android.TypeManager.Activate ("CloudClubv1._2_.Droid.CustomSearchBarRenderer, CloudClubv1._2_.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomSearchBarRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == CustomSearchBarRenderer.class)
			mono.android.TypeManager.Activate ("CloudClubv1._2_.Droid.CustomSearchBarRenderer, CloudClubv1._2_.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
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