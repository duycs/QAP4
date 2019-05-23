using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
