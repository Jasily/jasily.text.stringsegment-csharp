using System;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment : IEquatable<StringSegment>, IEquatable<string>
    {
        #region Object

        [PublicAPI, Pure]
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return this.Buffer == null;
            }

            if (obj is string s)
            {
                return this.Equals(s);
            }

            return obj is StringSegment ss && this.Equals(ss);
        }

        public static bool operator ==(object obj, StringSegment ss)
        {
            return ss.Equals(obj);
        }

        public static bool operator !=(object obj, StringSegment ss)
        {
            return !ss.Equals(obj);
        }

        public static bool operator ==(StringSegment ss, object obj)
        {
            return ss.Equals(obj);
        }

        public static bool operator !=(StringSegment ss, object obj)
        {
            return !ss.Equals(obj);
        }

        #endregion

        #region StringSegment

        [PublicAPI, Pure]
        public bool Equals(StringSegment other)
        {
            return this.Equals(other, StringComparison.Ordinal);
        }

        [PublicAPI, Pure]
        public bool Equals(StringSegment other, StringComparison comparisonType)
        {
            var textLength = other.Length;
            if (this.Length != textLength)
            {
                return false;
            }

            return string.Compare(this.Buffer, this.Offset, other.Buffer, other.Offset, textLength, comparisonType) == 0;
        }

        public static bool operator ==(StringSegment ss, StringSegment right)
        {
            return ss.Equals(right);
        }

        public static bool operator !=(StringSegment ss, StringSegment right)
        {
            return !ss.Equals(right);
        }

        #endregion

        #region String

        [PublicAPI, Pure]
        public bool Equals([CanBeNull] string other)
        {
            return this.Equals(other, StringComparison.Ordinal);
        }

        [PublicAPI, Pure]
        public bool Equals([CanBeNull] string other, StringComparison comparisonType)
        {
            if (other == null)
            {
                return this.Buffer == null;
            }

            if (this.Buffer == null)
            {
                return false;
            }

            if (this.Length != other.Length)
            {
                return false;
            }

            return string.Compare(this.Buffer, this.Offset, other, 0, other.Length, comparisonType) == 0;
        }

        #endregion

        public override int GetHashCode()
        {
            if (this.Buffer == null) return 0;
            return this.Buffer.GetHashCode() ^ this.Offset ^ this.Length;
        }
    }
}