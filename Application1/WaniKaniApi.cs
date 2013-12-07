using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Specialized;

namespace Application1
{
	public class WaniKaniApi
	{
		public WaniKaniApi ()
		{
		}

		public void GetStudyQueueData() {
			using (var client = new WebClientEx())
			{
				var response1 = client.DownloadString("http://www.wanikani.com/review/queue?");
				var token = GetToken (response1);

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

		private string GetToken(string str) {
			string[] words = Regex.Split(str, "meta content=\"");
			var ind = words.Last().IndexOf('"');
			var substr = words.Last().Substring(0, ind);
			return substr;
		}
	}
}

