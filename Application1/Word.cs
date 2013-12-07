using System.Collections.Generic;
using System;
using Android.Graphics;

namespace Application1
{
	public class Word {
		public int Id { get; set; }
		public string Name { get; set; }
		public List<string> Meanings { get; set; }
		public int Level { get; set; }
		public List<string> Synonyms { get; set; }
		public Color Color { get; set; }

		public Word() {
			Meanings = new List<string>();
			Synonyms = new List<string>();
		}

		protected List<string> GetWordsFromJsonArray(string line, string ignoreWord) {
			var array = line.Split ('"');
			var words = new List<string>();

			var invalids = new string[] {"" , "," , "]," , "[" , "[]}," , "[]}]" , ignoreWord};
			foreach (var word in array) {
				if (Array.IndexOf(invalids, word) < 0) {
					words.Add (word);
				}
			}

			return words;
		}
	}

	public class Vocab : Word {
		public List<string> Pronunciations { get; set; }
		public string Audio { get; set; }

		public Vocab(string[] data) {
			Color = Color.Argb(255, 160, 0, 240);

			Id = int.Parse (data [1].Split (',') [0]);
			Name = data [2].Split ('"')[1];
			Meanings = GetWordsFromJsonArray (data [3], "kana");
			Pronunciations = GetWordsFromJsonArray (data [4], "aud");
			Audio = data [5].Split ('"')[1];
			Level = int.Parse (data [6].Split (',')[0]);
			Synonyms = GetWordsFromJsonArray (data [7], " ");
		}
	}

	public class Kanji : Word {
		public string Emphasis { get; set; }
		public List<string> Kunyomi { get; set; }
		public List<string> Onyomi { get; set; }

		public Kanji(string[] data) {
			Color = Color.Argb(255, 240, 0, 160);

			Id = int.Parse (data [1].Split (',') [0]);
			Name = data [2].Split ('"')[1];
			Meanings = GetWordsFromJsonArray (data [3], "kana");
			Emphasis = data [4].Split ('"')[1];
			Kunyomi = GetWordsFromJsonArray (data [5], "on");
			Onyomi = GetWordsFromJsonArray (data [6], "srs");
			Level = int.Parse (data [7].Split (',')[0]);
			Synonyms = GetWordsFromJsonArray (data [8], " ");
		}
	}

	public class Radical : Word {
		public bool IsImage { get; set; }
		
		public Radical(string[] data) {
			Color = Color.Argb(255, 0, 160, 240);

			Id = int.Parse (data [1].Split (',') [0]);
			Name = data [2].Split ('"')[1];
			Meanings = GetWordsFromJsonArray (data [3], "kana");
			Level = int.Parse (data [4].Split (',')[0]);
			Synonyms = GetWordsFromJsonArray (data [5], " ");

			IsImage = (Name.Length > 5);
		}
	}
}

