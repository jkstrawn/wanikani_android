using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Application1
{
	[Activity (Label = "Application1", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.Main); 

			var aButton = FindViewById<Button> (Resource.Id.aButton);  

			aButton.Click += (sender, e) => {
				var newActivity = new Intent(this, typeof(NewWordsActivity));
				StartActivity(newActivity);
			};

			var bButton = FindViewById<Button> (Resource.Id.bButton);  

			bButton.Click += (sender, e) => {
				var newActivity = new Intent(this, typeof(StudyActivity));
				StartActivity(newActivity);
			};
		}
	}
}