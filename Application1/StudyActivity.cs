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
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Text;

namespace Application1
{
	[Activity (Label = "StudyActivity", ScreenOrientation = ScreenOrientation.Portrait)]		
	public class StudyActivity : Activity
	{

		static List<Word> words = new List<Word>();

		static int wordIndex = -1;
		EditText guessText;
		LinearLayout parentView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			conversion();

			SetContentView(Resource.Layout.StudyVocab);

			parentView = (LinearLayout)FindViewById (Resource.Id.abc);
			guessText = (EditText) FindViewById (Resource.Id.shark);
			guessText.SetOnKeyListener(new MyListener(this));
			var myWatcher = new MyWatcher (this);
			guessText.AddTextChangedListener (myWatcher); 
			DisplayNewWord ();

			var button = FindViewById<Button>(Resource.Id.submit);
			button.Click += (sender, e) => {
				conversion();
			};
		}

		public class MyWatcher : Java.Lang.Object, ITextWatcher {
			readonly StudyActivity activity;
			bool editable = true;

			public MyWatcher(StudyActivity _activity) {
				activity = _activity;
			}

			public void AfterTextChanged (IEditable s)
			{
				Console.WriteLine ("attempting to change");
				if (editable) {
					Console.WriteLine ("changing text");
					editable = false;
					activity.ConvertToHiragana ();
					editable = true;
				}
			}

			public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
			{

			}

			public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count)
			{

			}
		}

		public void ConvertToHiragana() {
			var temp = guessText.Text;
			//guessText.Text = temp;
			guessText.SetSelection(temp.Length);
		}

		protected override void OnStart() {
			base.OnStart ();

			InputMethodManager inputMethodManager=(InputMethodManager)GetSystemService(Context.InputMethodService);
			inputMethodManager.ToggleSoftInput(ShowFlags.Forced, 0);
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
			var word = words [wordIndex];

			var kanjiText = (TextView) FindViewById (Resource.Id.kanji);
			kanjiText.Text = word.Name;
			parentView.SetBackgroundColor (word.Color);
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

		protected class MyListener : Java.Lang.Object, View.IOnKeyListener {
			readonly StudyActivity activity;

			public MyListener(StudyActivity _activity) {
				activity = _activity;
			}

			public bool OnKey (View v, Keycode keyCode, KeyEvent e)
			{
				if (e.KeyCode == Keycode.Enter && e.Action == 0) {
					activity.SubmitGuess ();				
				}
				return false;
			}
		}

		public string ConvertToJapanese(string input)
		{
			var combos = new Dictionary<string, string>
			{
				{"wa", "わ"},
				{"ra", "ら"},
				{"ya", "や"},
				{"ma", "ま"},
				{"ha", "は"},
				{"na", "な"},
				{"ta", "た"},
				{"sa", "さ"},
				{"ka", "か"},
				{"a", "あ"},
				{"ri", "り"},
				{"mi", "み"},
				{"hi", "ひ"},
				{"ni", "に"},
				{"chi", "ち"},
				{"shi", "し"},
				{"ki", "き"},
				{"i", "い"},
				{"ru", "る"},
				{"yu", "ゆ"},
				{"mu", "む"},
				{"fu", "ふ"},
				{"nu", "ぬ"},
				{"tsu", "つ"},
				{"su", "す"},
				{"ku", "く"},
				{"u", "う"},
				{"re", "れ"},
				{"me", "め"},
				{"he", "へ"},
				{"ne", "ね"},
				{"te", "て"},
				{"se", "せ"},
				{"ke", "け"},
				{"e", "え"},
				{"wo", "を"},
				{"ro", "ろ"},
				{"yo", "よ"},
				{"mo", "も"},
				{"ho", "ほ"},
				{"no", "の"},
				{"to", "と"},
				{"so", "そ"},
				{"ko", "こ"},
				{"o", "お"},
				{"pa", "ぱ"},
				{"ba", "ば"},
				{"da", "だ"},
				{"za", "ざ"},
				{"ga", "が"},
				{"pi", "ぴ"},
				{"bi", "び"},
				{"ji", "じ"},
				{"gi", "ぎ"},
				{"pu", "ぷ"},
				{"bu", "ぶ"},
				{"dzu", "づ"},
				{"zu", "ず"},
				{"gu", "ぐ"},
				{"pe", "ぺ"},
				{"be", "べ"},
				{"de", "で"},
				{"ze", "ぜ"},
				{"ge", "げ"},
				{"po", "ぽ"},
				{"bo", "ぼ"},
				{"do", "ど"},
				{"zo", "ぞ"},
				{"go", "ご"},
				{"pya", "ぴゃ"},
				{"bya", "びゃ"},
				{"ja", "じゃ"},
				{"gya", "ぎゃ"},
				{"rya", "りゃ"},
				{"mya", "みゃ"},
				{"hya", "ひゃ"},
				{"nya", "にゃ"},
				{"cha", "ちゃ"},
				{"sha", "しゃ"},
				{"kya", "きゃ"},
				{"pyu", "ぴゅ"},
				{"byu", "びゅ"},
				{"ju", "じゅ"},
				{"gyu", "ぎゅ"},
				{"ryu", "りゅ"},
				{"myu", "みゅ"},
				{"hyu", "ひゅ"},
				{"nyu", "にゅ"},
				{"chu", "ちゅ"},
				{"shu", "しゅ"},
				{"kyu", "きゅ"},
				{"pyo", "ぴょ"},
				{"byo", "びょ"},
				{"jo", "じょ"},
				{"gyo", "ぎょ"},
				{"ryo", "りょ"},
				{"myo", "みょ"},
				{"hyo", "ひょ"},
				{"nyo", "にょ"},
				{"cho", "ちょ"},
				{"sho", "しょ"},
				{"kyo", "きょ"}
			};

			var beginning = "";

			foreach (var character in input)
			{
				var code = (int) character;
				if (code > 1000)
				{
					beginning += input[0];
					input = input.Substring(1, input.Length - 1);
				}
			}

			if (combos.ContainsKey(input))
			{
				input = combos[input];
			}
			else if (ShouldAddSmallTsu(input))
			{
				input = ReplaceFirstCharacter(input, 'っ');
			}
			else if (ShouldConvertN(input))
			{
				input = ReplaceFirstCharacter(input, 'ん');
			}

			return beginning + input;
		}

		private static bool ShouldAddSmallTsu(string input)
		{
			return input.Length > 1 && input[0] == input[1];
		}

		private static bool ShouldConvertN(string input)
		{
			return input.Length > 1 && input[0] == 'n';
		}

		private string ReplaceFirstCharacter(string input, char character)
		{
			input = character + input.Substring(1, input.Length - 1); ;
			return input;
		}
	}
}