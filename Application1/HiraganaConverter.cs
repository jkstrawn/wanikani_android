using System;

namespace Application1
{
	public class HiraganaConverter
	{
		public string Convert(string input)
		{
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

			if (Hiragana.Combos.ContainsKey(input))
			{
				input = Hiragana.Combos[input];
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

