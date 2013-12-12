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

		public bool NeedMeaning;
		public int MeaningsWrong;
		public bool NeedReading;
		public int ReadingsWrong;

		public Word(int id, string name, List<string> meanings, int level, List<string> synonyms) {
			Id = id;
			Name = name;
			Meanings = meanings;
			Level = level;
			Synonyms = synonyms;

			NeedMeaning = true;
			MeaningsWrong = 0;
			NeedReading = true;
			ReadingsWrong = 0;
		}

		public List<string> GetReadings() {
			return new List<string> ();
		}

		public string ConstructUrl() {
			return "";
		}

		protected string ConstructUrl(char type) {
			return "http://www.wanikani.com/json/progress?" + 
				type + Id + "[]=" + MeaningsWrong + "&" + 
				type + Id + "[]=" + ReadingsWrong;
		}
	}

	public class Vocab : Word {
		public List<string> Pronunciations { get; set; }
		public string Audio { get; set; }

		public Vocab (int id, string name, List<string> meanings, List<string> pronunciations, string audio, int level, List<string> synonyms)
			:base(id, name, meanings, level, synonyms)
		{
			Color = Color.Argb(255, 160, 0, 240);

			Pronunciations = pronunciations;
			Audio = audio;
		}

		public string ContructUrl() {
			return ConstructUrl ('v');
		}

		public List<string> GetReadings() {
			return Pronunciations;
		}
	}

	public class Kanji : Word {
		public string Emphasis { get; set; }
		public List<string> Kunyomi { get; set; }
		public List<string> Onyomi { get; set; }

		public Kanji (int id, string name, List<string> meanings, string emphasis, List<string> kunyomi, 
			List<string> onyomi, int level, List<string> synonyms) :base(id, name, meanings, level, synonyms)
		{
			Color = Color.Argb(255, 240, 0, 160);

			Emphasis = emphasis;
			Kunyomi = kunyomi;
			Onyomi = onyomi;
		}

		public string ContructUrl() {
			return ConstructUrl ('k');
		}

		public List<string> GetReadings() {
			return Onyomi;
		}
	}

	public class Radical : Word {
		public bool IsImage { get; set; }

		public Radical (int id, string name, List<string> meanings, int level, List<string> synonyms)
			:base(id, name, meanings, level, synonyms)
		{
			Color = Color.Argb(255, 0, 160, 240);
			NeedReading = false;

			IsImage = (Name.Length > 5);
		}

		public string ContructUrl() {
			return ConstructUrl ('r');
		}
	}
}

