package application1;


public class StudyActivity_SharkListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.TextView.OnEditorActionListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onEditorAction:(Landroid/widget/TextView;ILandroid/view/KeyEvent;)Z:GetOnEditorAction_Landroid_widget_TextView_ILandroid_view_KeyEvent_Handler:Android.Widget.TextView/IOnEditorActionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Application1.StudyActivity/SharkListener, Application1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", StudyActivity_SharkListener.class, __md_methods);
	}


	public StudyActivity_SharkListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == StudyActivity_SharkListener.class)
			mono.android.TypeManager.Activate ("Application1.StudyActivity/SharkListener, Application1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onEditorAction (android.widget.TextView p0, int p1, android.view.KeyEvent p2)
	{
		return n_onEditorAction (p0, p1, p2);
	}

	private native boolean n_onEditorAction (android.widget.TextView p0, int p1, android.view.KeyEvent p2);

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
