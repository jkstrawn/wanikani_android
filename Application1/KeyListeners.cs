using System;
using Android.Text;
using Android.Views;

namespace Application1
{
	public class MyTextListener : Java.Lang.Object, ITextWatcher {
		readonly StudyActivity activity;
		readonly HiraganaConverter HiraganaConverter;

		public MyTextListener(StudyActivity _activity) {
			activity = _activity;
			HiraganaConverter = new HiraganaConverter ();
		}

		public void AfterTextChanged (IEditable s)
		{
			var converted = HiraganaConverter.Convert (s.ToString());
			if (s.ToString() == converted) {
				return;
			}
			s.Replace (0, s.Length(), converted);
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

