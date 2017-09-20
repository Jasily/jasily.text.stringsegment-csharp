using System;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI, Pure]
        public bool Contains([NotNull] string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return this.IndexOf(value, comparisonType) > -1;
        }

        [PublicAPI, Pure]
        public bool Contains([NotNull] string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return this.IndexOf(value, startIndex, comparisonType) > -1;
        }

        [PublicAPI, Pure]
        public bool Contains([NotNull] string value, int startIndex, int length, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return this.IndexOf(value, startIndex, length, comparisonType) > -1;
        }
    }
}