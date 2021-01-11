namespace Hand2Note.Calc
{
    public static class LexerStringExtensions
    {

        public static bool HasSubstringAt(this string text, int index, string substring)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (index + substring.Length > text.Length) return false;

            return string.Compare(text.Substring(index, substring.Length), substring) == 0;
        }
    }
}
