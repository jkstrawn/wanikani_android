using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;
using Android.Text;
using Android.Views;

namespace Application1
{
	[Activity (Label = "StudyActivity", ScreenOrientation = ScreenOrientation.Portrait)]		
	public class StudyActivity : Activity
	{
		static List<Word> words = new List<Word>();

		static int wordIndex = 0;
		static bool typingHiragana = false;

		public EditText guessText;
		TextView kanjiText;
		TextView typeText;
		LinearLayout parentView;

		readonly static JsonConverter jsonConverter = new JsonConverter ();
		readonly static HiraganaConverter hiraganaConverter = new HiraganaConverter();
		readonly static WaniKaniApi api = new WaniKaniApi();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.StudyVocab);

			parentView = (LinearLayout)FindViewById (Resource.Id.abc);
			kanjiText = (TextView) FindViewById (Resource.Id.kanji);
			typeText = (TextView) FindViewById (Resource.Id.type);
			guessText = (EditText) FindViewById (Resource.Id.shark);


			guessText.SetRawInputType (InputTypes.TextFlagNoSuggestions);
			guessText.SetOnKeyListener(new MyKeyListener(this));
			guessText.AddTextChangedListener (new MyTextListener (this)); 

			var button = FindViewById<Button>(Resource.Id.submit);
			button.Click += (sender, e) => {
				SubmitGuess();
			};

			conversion();
			DisplayNewWord ();
		}

		public class MyInputFilter : Java.Lang.Object, IInputFilter {
			public EditText GuessText;
			string converted = "";

			public MyInputFilter(EditText _guessText) {
				GuessText = _guessText;
			}
			public Java.Lang.ICharSequence FilterFormatted (Java.Lang.ICharSequence source, 
				int start, int end, ISpanned dest, int dstart, int dend)
			{
				/*
				Console.WriteLine ("current: \"" + GuessText.Text + "\", in: \"" + source + "\"");
				if (source.Length () > 0) {
					converted = hiraganaConverter.Convert (GuessText.Text + source);
					//GuessText.Text = "";
					Console.WriteLine ("converted: \"" + converted + "\"");
				}
				Console.WriteLine ("returning converted: \"" + converted + "\"");
				return new Java.Lang.String (converted);

				Console.WriteLine (start + ", " + end + ", " + dstart + ", " + dend);
				Console.WriteLine (dest);
				return source;

*/
				/*
				if (source.Length ()  == 0) {
					Console.WriteLine ("backspace");
				}

				Console.WriteLine ("still works 1");
				if (source.Length () > 0) {
					if(source.CharAt(source.Length() - 1) == 'a') {
						Console.WriteLine ("source: \"" + source + '"');
						Console.WriteLine ("dest: \"" + dest + '"');
						//GuessText.DispatchKeyEvent (new KeyEvent(0, Keycode.Del));
					}
					return new Java.Lang.String (source.CharAt (source.Length () - 1).ToString());
				}
				*/
				return null;
			}
		}



		protected override void OnStart() {
			base.OnStart ();

			InputMethodManager inputMethodManager=(InputMethodManager)GetSystemService(Context.InputMethodService);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, 0);
		}

		private void conversion() {
			var str = "[{\"id\":204,\"rad\":\"魚\",\"en\":[\"Fish\"],\"srs\":6,\"syn\":[]},{\"id\":242,\"kan\":\"夏\",\"en\":[\"Summer\"],\"emph\":\"kunyomi\",\"kun\":[\"なつ\"],\"on\":[\"げ\"],\"srs\":6,\"syn\":[]},{\"id\":684,\"voc\":\"対立\",\"en\":[\"Confrontation\",\"Opposition\"],\"kana\":[\"たいりつ\"],\"aud\":\"aed2307c04d1490692609db190a4992f5465eec7.mp3\",\"srs\":1,\"syn\":[]},{\"id\":197,\"rad\":\"夫\",\"en\":[\"Husband\"],\"srs\":6,\"syn\":[]},{\"id\":1384,\"voc\":\"中学生\",\"en\":[\"Middle School Student\",\"Junior High School Student\",\"Middle Schooler\",\"Junior High Schooler\",\"Junior High Student\"],\"kana\":[\"ちゅうがくせい\"],\"aud\":\"5bc6dca8693209350a1b220fe0f2d3dab7b7dece.mp3\",\"srs\":6,\"syn\":[]},{\"id\":1385,\"voc\":\"不人気\",\"en\":[\"Unpopular\",\"Not Popular\"],\"kana\":[\"ふにんき\"],\"aud\":\"ea5f5d7757c728650c50f9b937c214d84ce8960c.mp3\",\"srs\":6,\"syn\":[]},{\"id\":489,\"voc\":\"次\",\"en\":[\"Next\"],\"kana\":[\"つぎ\"],\"aud\":\"127340ae4a838567eb286f50751e4ff80ed590bc.mp3\",\"srs\":6,\"syn\":[]}]";
			words = jsonConverter.Convert (str);
		}

		public void OnTextChange() {
			/*
			Console.WriteLine (guessText.Text);
			if (typingHiragana) {
				var temp = guessText.Text;
				var converted = hiraganaConverter.Convert (temp);
				Console.WriteLine (converted);
				if (converted != temp) {
					guessText.Text = converted;
					guessText.SetSelection(converted.Length);
				}
			}
			*/
		}

		public void EnterKeyPressed() {
			SubmitGuess ();
		}

		private void SubmitGuess() {
			var guess = guessText.Text;

			if (IsGuessCorrect(guess)) {
				DisplayNewWord ();
			}

			guessText.Text = "";
		}

		private bool IsGuessCorrect(string guess) {
			if (typingHiragana) {
				return IsHiraganaGuessCorrect (guess);
			}
			return IsEnglishGuessCorrect (guess);
		}

		private bool IsEnglishGuessCorrect (string guess)
		{
			var meanings = words [wordIndex].Meanings;

			foreach (var word in meanings) {
				if (word.ToLowerInvariant () == guess.Trim()) {
					words [wordIndex].NeedMeaning = false;
					return true;
				}
			}

			return false;
		}

		private bool IsHiraganaGuessCorrect(string guess) {
			var readings = words [wordIndex].GetReadings ();

			foreach (var word in readings) {
				if (word.ToLowerInvariant () == guess.Trim()) {
					words [wordIndex].NeedReading = false;
					return true;
				}
			}

			return false;
		}

		private void DisplayNewWord() {
			var currentWord = words [wordIndex];

			kanjiText.Text = currentWord.Name;
			parentView.SetBackgroundColor (currentWord.Color);

			if (currentWord.NeedMeaning) {
				Console.WriteLine ("show new meaning");
				typeText.Text = "Meaning";
				typingHiragana = false;
			} else if (currentWord.NeedReading) {
				Console.WriteLine ("show new reading");
				typeText.Text = "Reading";
				typingHiragana = true;
			} else {
				Console.WriteLine ("get next word");
				wordIndex++;
				DisplayNewWord ();	
			}
		}
	}
}