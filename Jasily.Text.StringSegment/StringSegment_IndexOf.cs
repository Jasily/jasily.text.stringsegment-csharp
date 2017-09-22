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

        /// <summary>
        /// See <see cref="string.IndexOf(char, int, int)"/>.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOf(char c, int startIndex, int count)
        {
            this.EnsureSegmentRange(startIndex, count);
            if (this.Buffer == null) return -1;

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.IndexOf(c, startIndex + this.Offset, count);
            return index >= 0 ? index - this.Offset : index;
        }

        /// <summary>
        /// See <see cref="string.IndexOf(char, int)"/>.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOf(char c, int startIndex) => this.IndexOf(c, startIndex, this.Length - startIndex);

        /// <summary>
        /// See <see cref="string.IndexOf(char)"/>.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOf(char c) => this.IndexOf(c, 0, this.Length);

        /// <summary>
        /// See <see cref="string.IndexOfAny(char[], int, int)"/>.
        /// </summary>
        /// <param name="anyOf"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            this.EnsureSegmentRange(startIndex, count);
            if (this.Buffer == null) return -1;

            var index = this.Buffer.IndexOfAny(anyOf, this.Offset + startIndex, count);
            return index >= 0 ? index - this.Offset : index;
        }

        /// <summary>
        /// See <see cref="string.IndexOfAny(char[], int)"/>.
        /// </summary>
        /// <param name="anyOf"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOfAny(char[] anyOf, int startIndex) => this.IndexOfAny(anyOf, startIndex, this.Length - startIndex);

        /// <summary>
        /// See <see cref="string.IndexOfAny(char[])"/>.
        /// </summary>
        /// <param name="anyOf"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOfAny(char[] anyOf) => this.IndexOfAny(anyOf, 0, this.Length);

        /// <summary>
        /// See <see cref="string.LastIndexOf(char)"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int LastIndexOf(char value)
        {
            if (this.Buffer == null) return -1;

            // ReSharper disable once PossibleNullReferenceException
            var index = this.Buffer.LastIndexOf(value, this.Offset + this.Length - 1, this.Length);
            return index >= 0 ? index - this.Offset : index;
        }

        #endregion

        #region index of string

        /// <summary>
        /// See <see cref="string.IndexOf(string, StringComparison)"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOf([NotNull] string value, StringComparison comparison = StringComparison.Ordinal)
            => this.IndexOf(value, 0, this.Length, comparison);

        /// <summary>
        /// See <see cref="string.IndexOf(string, int, StringComparison)"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOf([NotNull] string value, int startIndex, StringComparison comparison = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, this.Length - startIndex, comparison);

        /// <summary>
        /// See <see cref="string.IndexOf(string, int, int, StringComparison)"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOf([NotNull] string value, int startIndex, int count, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > this.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > this.Length) throw new ArgumentOutOfRangeException(nameof(count));

            if (this.Buffer == null) return -1;
            var index = this.Buffer.IndexOf(value, this.Offset + startIndex, count, comparison);
            return index >= 0 ? index - this.Offset : index;
        }

        #endregion

        #region index of StringSegment



        #endregion

        #region index of other

        /// <summary>
        /// Search the next <see cref="char.IsWhiteSpace(char)"/> start by <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public int IndexOfWhiteSpace(int startIndex = 0)
        {
            if (startIndex < 0 || startIndex > this.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (this.Buffer == null) return -1;
            for (var i = startIndex; i < this.Length; i++)
            {
                if (char.IsWhiteSpace(this.Buffer, this.Offset + i))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion
    }
}