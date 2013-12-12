using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace Application1
{
	[Activity (Label = "NewWordsActivity")]			
	[IntentFilter (new[]{Intent.ActionMain}, Categories = new[]{ "mono.support4demo.sample" })]
	public class NewWordsActivity : FragmentActivity
	{
		const int NUM_ITEMS = 6;
		MyAdapter adapter;
		ViewPager pager;

		static List<Word> words = new List<Word> ();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView(Resource.Layout.FragmentList);

			var footer = FindViewById<TextView> (Resource.Id.footer);
			adapter = new MyAdapter(SupportFragmentManager);

			pager = FindViewById<ViewPager>(Resource.Id.pager);
			pager.Adapter = adapter;
			var mpag = new MyListener (footer);
			pager.SetOnPageChangeListener (mpag);
			/*                                                                                                                                                         
			var button = FindViewById<Button>(Resource.Id.goto_first);
			button.Click += (sender, e) => {
				pager.CurrentItem = 0;        
			};
			button = FindViewById<Button>(Resource.Id.goto_last);
			button.Click += (sender, e) => {
				pager.CurrentItem = NUM_ITEMS - 1;
			};
			*/
		}

		protected class MyListener : Java.Lang.Object, ViewPager.IOnPageChangeListener {

			readonly TextView footer;

			public MyListener(TextView _footer) {
				footer = _footer;
			}

			public void OnPageSelected (int position)
			{
				footer.Text = "page " + position;
			}

			public void OnPageScrollStateChanged (int state)
			{
			}

			public void OnPageScrolled (int position, float positionOffset, int positionOffsetPixels)
			{
			}

			public void Dispose ()
			{
			}
		}
		
		protected class MyAdapter : FragmentPagerAdapter 
		{
			public MyAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
			{
			}

			public override int Count {
				get {
					return NUM_ITEMS;
				}
			}

			public override Android.Support.V4.App.Fragment GetItem (int position)
			{
				return new ArrayFragment(position);
			}

		}

		protected class ArrayFragment : Android.Support.V4.App.Fragment
		{
			string name;

			string meaning;

			public ArrayFragment()
			{
			}

			public ArrayFragment(int wordIndex)
			{
				var args = new Bundle();
				args.PutInt("wordIndex", wordIndex);
				Arguments = args;
			}

			public override void OnCreate (Bundle p0)
			{
				base.OnCreate (p0);

				var wordIndex = Arguments != null ? Arguments.GetInt("wordIndex") : 0;
				var word = words [wordIndex];

				name = word.Name;
				meaning = word.Meanings [0];
			}

			public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
			{
				var view = inflater.Inflate(Resource.Layout.Fragment, container, false);
				var textName = view.FindViewById<TextView>(Resource.Id.text);
				textName.Text = name;
				return view;
			}

			public override void OnActivityCreated (Bundle p0)
			{
				base.OnActivityCreated (p0);
			}
		}
	}
}

