using JetBrains.Annotations;

namespace Jasily.Text
{
    public static class StringExtensions
    {
        [PublicAPI, Pure]
        public static StringSegment ToSegment([CanBeNull] this string str) => new StringSegment(str);
    }
}