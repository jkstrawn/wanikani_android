using System;

namespace Application1
{
	public class HiraganaConverter
	{
		public string Convert(string text)
		{
			if (text.Trim () == "") {
				return text;
			}

			var converted = ConvertChunk (text);

			if (converted == text.Trim ()) {
				converted = RetryConvert (text);
			}

			return converted;
		}

		public string ConvertChunk(string text) {
			var beginning = "";
			var input = text.Trim();

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
			else if (ShouldConvertN(input))
			{
				input = ReplaceFirstCharacter(input, 'ん');
			}
			else if (ShouldAddSmallTsu(input))
			{
				input = ReplaceFirstCharacter(input, 'っ');
			}

			return beginning + input;
		}

		private string RetryConvert(string text) {
			var charArray = text.ToCharArray();
			var input = charArray[0].ToString();

			for (var i = 1; i < charArray.Length; i++) {
				input = ConvertChunk (input);
				input += charArray[i];
			}

			return input;
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

