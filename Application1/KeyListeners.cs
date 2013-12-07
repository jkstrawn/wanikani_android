using System;
using Android.Text;
using Android.Views;

namespace Application1
{
	public class MyTextListener : Java.Lang.Object, ITextWatcher {
		readonly StudyActivity activity;
		bool editable = true;

		public MyTextListener(StudyActivity _activity) {
			activity = _activity;
		}

		public void AfterTextChanged (IEditable s)
		{
			if (editable) {
				editable = false;
				activity.OnTextChange ();
				editable = true;
			}
		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after) {}

		public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count) {}
	}

	public class MyKeyListener : Java.Lang.Object, View.IOnKeyListener {
		readonly StudyActivity activity;

		public MyKeyListener(StudyActivity _activity) {
			activity = _activity;
		}

		public bool OnKey (View v, Keycode keyCode, KeyEvent e)
		{
			if (e.KeyCode == Keycode.Enter && e.Action == 0) {
				activity.EnterKeyPressed ();				
			}
			return false;
		}
	}
}

