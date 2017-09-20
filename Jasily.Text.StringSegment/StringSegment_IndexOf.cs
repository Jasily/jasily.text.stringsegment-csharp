using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        #region index of chars

        [Pure]
        public int IndexOf(char c, int startIndex, int count)
        {
            this.EnsureSegmentRange(startIndex, count);
            if (this.Buffer == null) return -1;

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.IndexOf(c, startIndex + this.Offset, count);
            return index >= 0 ? index - this.Offset : index;
        }

        [Pure]
        public int IndexOf(char c, int startIndex)
        {
            return this.IndexOf(c, startIndex, this.Length - startIndex);
        }

        [Pure]
        public int IndexOf(char c)
        {
            return this.IndexOf(c, 0, this.Length);
        }

        [Pure]
        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            this.EnsureSegmentRange(startIndex, count);
            if (this.Buffer == null) return -1;

            var index = this.Buffer.IndexOfAny(anyOf, this.Offset + startIndex, count);
            if (index == -1)
            {
                return index;
            }

            return index - this.Offset;
        }
        
        [Pure]
        public int IndexOfAny(char[] anyOf, int startIndex)
        {
            return this.IndexOfAny(anyOf, startIndex, this.Length - startIndex);
        }

        [Pure]
        public int IndexOfAny(char[] anyOf)
        {
            return this.IndexOfAny(anyOf, 0, this.Length);
        }

        [Pure]
        public int LastIndexOf(char value)
        {
            if (!this.HasValue)
            {
                return -1;
            }

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.LastIndexOf(value, this.Offset + this.Length - 1, this.Length);
            if (index == -1)
            {
                return -1;
            }

            return index - this.Offset;
        }

        #endregion

        #region index of string

        [PublicAPI, Pure]
        public int IndexOf([NotNull] string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.EnsureNotNull();

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.IndexOf(value, this.Offset, this.Length, comparison);
            return index >= 0 ? index - this.Offset : index;
        }

        [PublicAPI, Pure]
        public int IndexOf([NotNull] string value, int startIndex, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > this.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            this.EnsureNotNull();

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.IndexOf(value, this.Offset + startIndex, this.Length - startIndex, comparison);
            return index >= 0 ? index - this.Offset : index;
        }

        [PublicAPI, Pure]
        public int IndexOf([NotNull] string value, int startIndex, int count, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > this.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > this.Length) throw new ArgumentOutOfRangeException(nameof(count));
            this.EnsureNotNull();

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.IndexOf(value, this.Offset + startIndex, count, comparison);
            return index >= 0 ? index - this.Offset : index;
        }

        #endregion

        #region index of StringSegment



        #endregion
    }
}