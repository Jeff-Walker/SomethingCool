using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Camelizer {
    public static class Camelizer {
        static readonly Regex NonWordChars = new Regex(@"[^a-zA-Z0-9]+");
        // zero-width match of an uppercase (\p{Lu}) and then a lowercase (\p{Ll}) letter
        static readonly Regex CamelHump = new Regex(@"(?=\p{Lu}\p{Ll})");

        public enum CamelStyle { Upper, Lower }   

        public static string CamelCase(this string input, CamelStyle style = CamelStyle.Upper) {
            return string.Join("", input.SplitIntoWords().SplitCamelHumps().NormalizeCase(style));
        }

        private static IEnumerable<string> NormalizeCase(this IEnumerable<string> words, CamelStyle style) {
            var currentStyle = style;
            foreach (var word in words) {
                yield return NormalizeCase(word, currentStyle);

                if (currentStyle == CamelStyle.Lower) {
                    currentStyle = CamelStyle.Upper;
                }
            }
        }

        private static string NormalizeCase(string word, CamelStyle style) {
            if (string.IsNullOrWhiteSpace(word)) {
                return null;
            }
            var initial = style == CamelStyle.Upper ? char.ToUpper(word[0]) : char.ToLower(word[0]);
            if (word.Length == 1) {
                return initial.ToString(CultureInfo.InvariantCulture);
            }
            return initial + word.Substring(1).ToLower();
        }

        public static IEnumerable<string> SplitCamelHumps(this string input) {
            return CamelHump.Split(input);
        }

        public static IEnumerable<string> SplitCamelHumps(this IEnumerable<string> input) {
            return input.Select(SplitCamelHumps).Flatten();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> stuff) {
            return stuff.SelectMany(x => x);
        }

        private static IEnumerable<string> SplitIntoWords(this string input) {
            return NonWordChars.Split(input);
        }
    }
}
