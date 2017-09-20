using System;
using System.Collections.Generic;

namespace Jasily.Text
{
    public class StringSegmentComparer : IEqualityComparer<StringSegment>
    {
        public static StringSegmentComparer Ordinal { get; }
            = new StringSegmentComparer(StringComparison.Ordinal, StringComparer.Ordinal);

        public static StringSegmentComparer OrdinalIgnoreCase { get; }
            = new StringSegmentComparer(StringComparison.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);

        private StringSegmentComparer(StringComparison comparison, StringComparer comparer)
        {
            this.Comparison = comparison;
            this.Comparer = comparer;
        }

        private StringComparison Comparison { get; }

        private StringComparer Comparer { get; }

        public bool Equals(StringSegment x, StringSegment y)
        {
            return x.Equals(y, this.Comparison);
        }

        public int GetHashCode(StringSegment obj)
        {
            if (obj.Buffer == null) return 0;
            return this.Comparer.GetHashCode(obj.Value) ^ obj.Offset ^ obj.Length;
        }
    }
}