using System;
using System.Collections.Generic;

namespace Application1
{
	public class JsonConverter
	{
		public List<Word> Convert(string text) {
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
				word = ConvertVocab(data);
			} else if (text.IndexOf ("\"rad\"") > 0) {
				word = ConvertRadical(data);
			} else {
				word = ConvertKanji(data);
			}

			return word;
		}

		private List<string> GetWordsFromJsonArray(string line, string ignoreWord) {
			var possibleWords = line.Split ('"');
			var words = new List<string>();

			foreach (var word in possibleWords) {
				if (DoesWordContainNoInvalids(word, ignoreWord)) {
					words.Add (word);
				}
			}

			return words;
		}

		private bool DoesWordContainNoInvalids(string word, string ignoreWord) {
			var invalids = new string[] {"" , "," , "]," , "[" , "[]}," , "[]}]" , ignoreWord};
			return Array.IndexOf (invalids, word) < 0;
		}

		private Vocab ConvertVocab(string[] data) {
			var id = int.Parse (data [1].Split (',') [0]);
			var name = data [2].Split ('"')[1];
			var meanings = GetWordsFromJsonArray (data [3], "kana");
			var pronunciations = GetWordsFromJsonArray (data [4], "aud");
			var audio = data [5].Split ('"')[1];
			var level = int.Parse (data [6].Split (',')[0]);
			var synonyms = GetWordsFromJsonArray (data [7], " ");

			return new Vocab(id, name, meanings, pronunciations, audio, level, synonyms); 		
		}


		private Kanji ConvertKanji(string[] data) {
			var id = int.Parse (data [1].Split (',') [0]);
			var name = data [2].Split ('"')[1];
			var meanings = GetWordsFromJsonArray (data [3], "kana");
			var emphasis = data [4].Split ('"')[1];
			var kunyomi = GetWordsFromJsonArray (data [5], "on");
			var onyomi = GetWordsFromJsonArray (data [6], "srs");
			var level = int.Parse (data [7].Split (',')[0]);
			var synonyms = GetWordsFromJsonArray (data [8], " ");

			return new Kanji(id, name, meanings, emphasis, kunyomi, onyomi, level, synonyms);
		}

		private Radical ConvertRadical(string[] data) {
			var id = int.Parse (data [1].Split (',') [0]);
			var name = data [2].Split ('"')[1];
			var meanings = GetWordsFromJsonArray (data [3], "kana");
			var level = int.Parse (data [4].Split (',')[0]);
			var synonyms = GetWordsFromJsonArray (data [5], " ");

			return new Radical(id, name, meanings, level, synonyms);
		}
	}
}

