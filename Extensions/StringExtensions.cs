using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QAP4.Extensions
{
    public static class StringExtensions
    {
        public static string EmptyIfNull(this object value)
        {
            if (value == null)
            {
                return "";
            }
            else
                return value.ToString();
        }

        public static int ZeroIfNull(this object value)
        {
            if (value == null)
            {
                return 0;
            }
            else
                return (int)value;
        }

        /// <summary>
        /// Count words with Regex.
        /// </summary>
        public static int CountWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }

        /// <summary>
        /// Count word with loop and character tests.
        /// </summary>
        public static int CountWords2(string s)
        {
            int c = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i - 1]) == true)
                {
                    if (char.IsLetterOrDigit(s[i]) == true ||
                        char.IsPunctuation(s[i]))
                    {
                        c++;
                    }
                }
            }
            if (s.Length > 2)
            {
                c++;
            }
            return c;
        }

        public static string GetWords(string input, int take)
        {
            string[] words = GetWords(input);
            int wordsLength = words.Length;
            string result = "";
            if (wordsLength >= take)
                for (int i = 0; i < take; ++i)
                {
                    result += words[i].ToString() + " ";
                }
            else
                result = input;
            return result;
        }


        public static string GetWords(string input, int from, int take)
        {
            string[] words = GetWords(input);
            int wordsLength = words.Length;
            string result = "";
            if (wordsLength >= take)
                for (int i = from; i < take; ++i)
                {
                    result += words[i].ToString() + " ";
                }
            else
                result = input;
            return result;
        }


        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        private static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }


        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                //throw new ArgumentException("ARGH!");
                input = "Chưa có tiêu đề";
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string UppercaseFirstEach(string s)
        {
            char[] a = s.ToLower().ToCharArray();

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = i == 0 || a[i - 1] == ' ' ? char.ToUpper(a[i]) : a[i];

            }

            return new string(a);
        }

        public static string StringToSlug(string title, bool remapToAscii = true, int maxlength = 255){
            int length = title.Length;
			bool prevdash = false;
			StringBuilder stringBuilder = new StringBuilder(length);
			char c;

			for (int i = 0; i < length; ++i)
			{
				c = title[i];
				if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
				{
					stringBuilder.Append(c);
					prevdash = false;
				}
				else if (c >= 'A' && c <= 'Z')
				{
					// tricky way to convert to lower-case
					stringBuilder.Append((char)(c | 32));
					prevdash = false;
				}
				else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
					(c == '\\') || (c == '-') || (c == '_') || (c == '='))
				{
					if (!prevdash && (stringBuilder.Length > 0))
					{
						stringBuilder.Append('-');
						prevdash = true;
					}
				}
				else if (c >= 128)
				{
					int previousLength = stringBuilder.Length;

					if (remapToAscii)
					{
						stringBuilder.Append(RemapInternationalCharToAscii(c));
					}
					else
					{
						stringBuilder.Append(c);
					}

					if (previousLength != stringBuilder.Length)
					{
						prevdash = false;
					}
				}

				if (i == maxlength)
				{
					break;
				}
			}

			if (prevdash)
			{
				return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
			}
			else
			{
				return stringBuilder.ToString();
			}
        }

        /// <summary>
		/// Remaps the international character to their equivalent ASCII characters. See
		/// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
		/// </summary>
		/// <param name="character">The character to remap to its ASCII equivalent.</param>
		/// <returns>The remapped character</returns>
		public static string RemapInternationalCharToAscii(char character)
		{
			string s = character.ToString().ToLowerInvariant();
			if ("ạậàåáâấầäãåąāăắằảẩẫ".Contains(s))
			{
				return "a";
			}
			else if ("ẹèéêếềểệëęẻ".Contains(s))
			{
				return "e";
			}
			else if ("ịìíîïıiỉĩ".Contains(s))
			{
				return "i";
			}
			else if ("ọộốồòóôõöøőðơớờợỏ".Contains(s))
			{
				return "o";
			}
			else if ("ùụúûüŭůưựừứũửữủ".Contains(s))
			{
				return "u";
			}
			else if ("çćčĉ".Contains(s))
			{
				return "c";
			}
			else if ("żźž".Contains(s))
			{
				return "z";
			}
			else if ("śşšŝ".Contains(s))
			{
				return "s";
			}
			else if ("ñń".Contains(s))
			{
				return "n";
			}
			else if ("ỳýÿỵỷ".Contains(s))
			{
				return "y";
			}
			else if ("ğĝ".Contains(s))
			{
				return "g";
			}
			else if ("ř".Contains(s))
			{
				return "r";
			}
			else if ("ł".Contains(s))
			{
				return "l";
			}
			else if ("đ".Contains(s))
			{
				return "d";
			}
			else if ("ß".Contains(s))
			{
				return "ss";
			}
			else if ("Þ".Contains(s))
			{
				return "th";
			}
			else if ("ĥ".Contains(s))
			{
				return "h";
			}
			else if ("ĵ".Contains(s))
			{
				return "j";
			}
			else
			{
				return string.Empty;
			}
		}

        
    }
}
