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
	}

	public class Vocab : Word {
		public List<string> Pronunciations { get; set; }
		public string Audio { get; set; }

		public Vocab (int id, string name, List<string> meanings, List<string> pronunciations, string audio, int level, List<string> synonyms)
		{
			Color = Color.Argb(255, 160, 0, 240);

			Id = id;
			Name = name;
			Meanings = meanings;
			Pronunciations = pronunciations;
			Audio = audio;
			Level = level;
			Synonyms = synonyms;
		}
	}

	public class Kanji : Word {
		public string Emphasis { get; set; }
		public List<string> Kunyomi { get; set; }
		public List<string> Onyomi { get; set; }

		public Kanji (int id, string name, List<string> meanings, string emphasis, List<string> kunyomi, 
			List<string> onyomi, int level, List<string> synonyms)
		{
			Color = Color.Argb(255, 240, 0, 160);

			Id = id;
			Name = name;
			Meanings = meanings;
			Emphasis = emphasis;
			Kunyomi = kunyomi;
			Onyomi = onyomi;
			Level = level;
			Synonyms = synonyms;
		}
	}

	public class Radical : Word {
		public bool IsImage { get; set; }

		public Radical (int id, string name, List<string> meanings, int level, List<string> synonyms)
		{
			Color = Color.Argb(255, 0, 160, 240);

			Id = id;
			Name = name;
			Meanings = meanings;
			Level = level;
			Synonyms = synonyms;

			IsImage = (Name.Length > 5);
		}
	}
}

