using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;

namespace Application1
{
	[Activity (Label = "StudyActivity", ScreenOrientation = ScreenOrientation.Portrait)]		
	public class StudyActivity : Activity
	{
		static List<Word> words = new List<Word>();

		static int wordIndex = -1;
		EditText guessText;
		LinearLayout parentView;
		readonly JsonConverter jsonConverter = new JsonConverter ();
		readonly HiraganaConverter hiraganaConverter = new HiraganaConverter();
		readonly WaniKaniApi api = new WaniKaniApi();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.StudyVocab);

			parentView = (LinearLayout)FindViewById (Resource.Id.abc);
			guessText = (EditText) FindViewById (Resource.Id.shark);
			guessText.SetOnKeyListener(new MyKeyListener(this));
			guessText.AddTextChangedListener (new MyTextListener (this)); 

			var button = FindViewById<Button>(Resource.Id.submit);
			button.Click += (sender, e) => {
				SubmitGuess();
			};

			conversion();
			DisplayNewWord ();
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
			var temp = guessText.Text;
			guessText.Text = temp;
			guessText.SetSelection(temp.Length);
		}

		public void EnterKeyPressed() {
			SubmitGuess ();
		}

		private void SubmitGuess() {
			var guess = guessText.Text;
			var correctWords = words [wordIndex].Meanings;

			if (GuessIsCorrect(guess, correctWords)) {
				DisplayNewWord ();
			}

			guessText.Text = "";
		}

		private bool GuessIsCorrect (string guess, List<string> correctWords)
		{
			foreach (var word in correctWords) {
				if (word.ToLowerInvariant () == guess.Trim()) {
					return true;
				}
			}

			return false;
		}

		private void DisplayNewWord() {
			wordIndex++;
			var word = words [wordIndex];

			var kanjiText = (TextView) FindViewById (Resource.Id.kanji);
			kanjiText.Text = word.Name;
			parentView.SetBackgroundColor (word.Color);
		}
	}
}