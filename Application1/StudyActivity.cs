using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Android.Views;
using Android.Views.InputMethods;
using System.Collections.Generic;
using System.Net;
using System.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Application1
{
	[Activity (Label = "StudyActivity")]		
	public class StudyActivity : Activity
	{

		static List<Word> words = new List<Word> {
			new Word{ Name = "买", Meanings = new List<string>{"dolphin"}},
			new Word{ Name = "传", Meanings = new List<string>{"killer whale"}},
			new Word{ Name = "你", Meanings = new List<string>{"eel"}},
			new Word{ Name = "侠", Meanings = new List<string>{"manatee"}},
			new Word{ Name = "倀", Meanings = new List<string>{"otter"}},
			new Word{ Name = "倐", Meanings = new List<string>{"shark"}}
		};

		static int wordIndex = -1;
		EditText guessText;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			conversion();

			SetContentView(Resource.Layout.StudyVocab);

			guessText = (EditText) FindViewById (Resource.Id.shark);

			Console.WriteLine ("stuff: " + guessText.Enabled);
			guessText.SetOnKeyListener(new MyListener(this));
			DisplayNewWord ();

			var button = FindViewById<Button>(Resource.Id.submit);
			button.Click += (sender, e) => {
				conversion();
			};
		}

		private void conversion() {
			var str = "[{\"id\":204,\"rad\":\"魚\",\"en\":[\"Fish\"],\"srs\":6,\"syn\":[]},{\"id\":242,\"kan\":\"夏\",\"en\":[\"Summer\"],\"emph\":\"kunyomi\",\"kun\":[\"なつ\"],\"on\":[\"げ\"],\"srs\":6,\"syn\":[]},{\"id\":684,\"voc\":\"対立\",\"en\":[\"Confrontation\",\"Opposition\"],\"kana\":[\"たいりつ\"],\"aud\":\"aed2307c04d1490692609db190a4992f5465eec7.mp3\",\"srs\":1,\"syn\":[]},{\"id\":197,\"rad\":\"夫\",\"en\":[\"Husband\"],\"srs\":6,\"syn\":[]},{\"id\":1384,\"voc\":\"中学生\",\"en\":[\"Middle School Student\",\"Junior High School Student\",\"Middle Schooler\",\"Junior High Schooler\",\"Junior High Student\"],\"kana\":[\"ちゅうがくせい\"],\"aud\":\"5bc6dca8693209350a1b220fe0f2d3dab7b7dece.mp3\",\"srs\":6,\"syn\":[]},{\"id\":1385,\"voc\":\"不人気\",\"en\":[\"Unpopular\",\"Not Popular\"],\"kana\":[\"ふにんき\"],\"aud\":\"ea5f5d7757c728650c50f9b937c214d84ce8960c.mp3\",\"srs\":6,\"syn\":[]},{\"id\":489,\"voc\":\"次\",\"en\":[\"Next\"],\"kana\":[\"つぎ\"],\"aud\":\"127340ae4a838567eb286f50751e4ff80ed590bc.mp3\",\"srs\":6,\"syn\":[]}]";
			words = convertToJson (str);
		}

		private List<Word> convertToJson(string text) {
			var words = new List<Word>();
			var items = text.Split ('{');

			foreach (var item in items) {
				if (item.Length > 10) {
					var word = convertToWord (item);
					words.Add (word);
				}
			}

			return words;
		}

		private Word convertToWord(string text) {
			Word word;
			var data = text.Split (':');
			if (text.IndexOf ("\"voc\"") > 0) {
				word = new Vocab (data);
			} else if (text.IndexOf ("\"rad\"") > 0) {
				word = new Radical (data);
			} else {
				word = new Kanji (data);
			}

			return word;
		}

		private void getStudyQueueData() {
			using (var client = new WebClientEx())
			{
				var response1 = client.DownloadString("http://www.wanikani.com/review/queue?");
				var token = getToken (response1);

				var values = new NameValueCollection
				{
					{ "authenticity_token" , token },
					{ "user[login]" , "jkstrawn" },
					{ "user[password]" , "gold2066" },
					{ "user[remember_me]" , "0" }
				};

				client.Headers.Set("Accept", "*/*");
				client.UploadValues("http://www.wanikani.com/login", values);

				client.Headers.Set("Accept", "*/*");
				var str = client.DownloadString("http://www.wanikani.com/review/queue?");

				Console.WriteLine (str);
			}
		}

		public class Root {

		}

		public class WebClientEx : WebClient
		{
			public CookieContainer CookieContainer { get; private set; }

			public WebClientEx()
			{
				CookieContainer = new CookieContainer();
			}

			protected override WebRequest GetWebRequest(Uri address)
			{
				var request = base.GetWebRequest(address);
				if (request is HttpWebRequest)
				{
					(request as HttpWebRequest).CookieContainer = CookieContainer;
				}
				return request;
			}
		}

		private string getToken(string str) {
			string[] words = Regex.Split(str, "meta content=\"");
			var ind = words.Last().IndexOf('"');
			var substr = words.Last().Substring(0, ind);
			return substr;
		}

		private void DisplayNewWord() {
			wordIndex++;

			var kanjiText = (TextView) FindViewById (Resource.Id.kanji);
			kanjiText.Text = words [wordIndex].Name;
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
				Console.WriteLine (word.ToLowerInvariant() + " vs " + guess);
				if (word.ToLowerInvariant () == guess) {
					return true;
				}
			}

			return false;
		}

		protected class MyListener : Java.Lang.Object, View.IOnKeyListener {
			readonly StudyActivity activity;

			public MyListener(StudyActivity _activity) {
				activity = _activity;
			}

			public bool OnKey (View v, Keycode keyCode, KeyEvent e)
			{
				if (e.KeyCode == Keycode.Enter) {
					activity.SubmitGuess ();				
				}
				return false;
			}
		}
	}
}