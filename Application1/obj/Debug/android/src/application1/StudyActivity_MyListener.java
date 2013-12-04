package application1;


public class StudyActivity_MyListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnKeyListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onKey:(Landroid/view/View;ILandroid/view/KeyEvent;)Z:GetOnKey_Landroid_view_View_ILandroid_view_KeyEvent_Handler:Android.Views.View/IOnKeyListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Application1.StudyActivity/MyListener, Application1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", StudyActivity_MyListener.class, __md_methods);
	}


	public StudyActivity_MyListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == StudyActivity_MyListener.class)
			mono.android.TypeManager.Activate ("Application1.StudyActivity/MyListener, Application1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public StudyActivity_MyListener (application1.StudyActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == StudyActivity_MyListener.class)
			mono.android.TypeManager.Activate ("Application1.StudyActivity/MyListener, Application1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Application1.StudyActivity, Application1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public boolean onKey (android.view.View p0, int p1, android.view.KeyEvent p2)
	{
		return n_onKey (p0, p1, p2);
	}

	private native boolean n_onKey (android.view.View p0, int p1, android.view.KeyEvent p2);

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
