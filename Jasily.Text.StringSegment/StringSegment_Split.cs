using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable MemberCanBePrivate.Global

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        #region API: split

        /// <summary>
        /// See <see cref="string.Split(string[], StringSplitOptions)"/>.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public StringsSpliter Split(string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return new StringsSpliter(ref this, new[] {separator}, int.MaxValue, options);
        }

        /// <summary>
        /// See <see cref="string.Split(string[], StringSplitOptions)"/>.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public StringsSpliter Split([CanBeNull] string[] separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return new StringsSpliter(ref this, separator, int.MaxValue, options);
        }

        /// <summary>
        /// See <see cref="string.Split(string[], int, StringSplitOptions)"/>.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="count"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public StringsSpliter Split([CanBeNull] string[] separator, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            return new StringsSpliter(ref this, separator, count, options);
        }

        /// <summary>
        /// See <see cref="string.Split(char[], StringSplitOptions)"/>.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public CharsSpliter Split(char ch, StringSplitOptions options = StringSplitOptions.None)
        {
            return new CharsSpliter(ref this, new[] { ch }, int.MaxValue, options);
        }

        /// <summary>
        /// See <see cref="string.Split(char[])"/>.
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public CharsSpliter Split([CanBeNull] params char[] separator)
        {
            return new CharsSpliter(ref this, separator, int.MaxValue, StringSplitOptions.None);
        }

        /// <summary>
        /// See <see cref="string.Split(char[], StringSplitOptions)"/>.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public CharsSpliter Split([CanBeNull] char[] separator, StringSplitOptions options)
        {
            return new CharsSpliter(ref this, separator, int.MaxValue, options);
        }

        /// <summary>
        /// See <see cref="string.Split(char[], int, StringSplitOptions)"/>.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="count"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [PublicAPI, Pure]
        public CharsSpliter Split([CanBeNull] char[] separator, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            return new CharsSpliter(ref this, separator, count, options);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public struct StringsSpliter : IEnumerable<StringSegment>
        {
            private readonly StringSegment _value;
            private readonly int _count;
            private readonly StringSplitOptions _options;
            [CanBeNull] private readonly string[] _separators;

            internal StringsSpliter(ref StringSegment value, [CanBeNull] string[] separators, int count, StringSplitOptions options)
            {
                Debug.Assert(count >= 0);
                if (options != StringSplitOptions.None && options != StringSplitOptions.RemoveEmptyEntries)
                {
                    throw new ArgumentException();
                }
                value.EnsureNotNull();

                this._separators = separators;
                this._value = value;
                this._count = count;
                this._options = options;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public Enumerator GetEnumerator() => new Enumerator(ref this);

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            /// <summary>
            /// Enumerator for <see cref="StringsSpliter"/>.
            /// </summary>
            public struct Enumerator : IEnumerator<StringSegment>
            {
                private readonly StringSegment _value;
                private readonly int _limit;
                private readonly StringSplitOptions _options;
                [CanBeNull] private readonly string[] _separators;
                private int _index;
                private int _count;
                [CanBeNull] private readonly int[] _nextIndexCache;

                internal Enumerator(ref StringsSpliter tokenizer)
                {
                    // value
                    this._value = tokenizer._value;

                    // args
                    this._separators = tokenizer._separators;
                    this._limit = tokenizer._count;
                    this._options = tokenizer._options;

                    // init
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                    // ReSharper disable once PossibleNullReferenceException
                    this._nextIndexCache = this._separators?.Length > 0 ? new int[this._separators.Length] : null;
                }

                /// <inheritdoc />
                public StringSegment Current { get; private set; }

                object IEnumerator.Current => this.Current;

                /// <inheritdoc />
                public bool MoveNext()
                {
                    if (this._value.Buffer == null || this._count >= this._limit)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    if (this._separators?.Length == 0 || this._limit == 1)
                    {
                        return this.SliceToEnd();
                    }

                    while (true)
                    {
                        if (this._index > this._value.Length)
                        {
                            this.Current = default(StringSegment);
                            return false;
                        }

                        var next = this.IndexOfNext(out var separator);

                        if (next == -1)
                        {
                            return this.SliceToEnd();
                        }

                        if (next == this._index && this._options == StringSplitOptions.RemoveEmptyEntries)
                        {
                            this._index = next + 1;
                        }
                        else if (this._count == this._limit - 1)
                        {
                            return this.SliceToEnd();
                        }
                        else
                        {
                            return this.SliceTo(next, separator);
                        }
                    }
                }

                private int IndexOfNext(out string separator)
                {
                    separator = null;
                    var next = int.MaxValue;

                    if (this._separators != null)
                    {
                        for (var i = 0; i < this._separators.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(this._separators[i]))
                            {
                                // ReSharper disable once PossibleNullReferenceException
                                var idx = this._nextIndexCache[i];

                                if (idx < this._index || idx == 0)
                                {
                                    idx = this._value.IndexOf(this._separators[i], this._index);
                                    this._nextIndexCache[i] = idx;
                                }

                                if (idx >= 0 && idx < next)
                                {
                                    separator = this._separators[i];
                                    next = idx;
                                }
                            }
                        }
                    }
                    else
                    {
                        next = this._value.IndexOfWhiteSpace(this._index);
                    }

                    return next != int.MaxValue ? next : -1;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private bool SliceTo(int next, [CanBeNull] string separator)
                {
                    this.Current = this._value.SubSegment(this._index, next - this._index);
                    this._index = next + (separator?.Length ?? 1);
                    this._count++;
                    return true;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private bool SliceToEnd()
                {
                    var valLen = this._value.Length;

                    if (this._index > valLen ||
                        valLen == this._index && this._options == StringSplitOptions.RemoveEmptyEntries)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    return this.SliceTo(this._value.Length, null);
                }

                /// <summary>
                /// Throw <see cref="NotSupportedException"/>.
                /// </summary>
                /// <exception cref="NotSupportedException"></exception>
                public void Reset() => throw new NotSupportedException();

                /// <inheritdoc />
                public void Dispose() { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public struct CharsSpliter : IEnumerable<StringSegment>
        {
            private readonly StringSegment _value;
            private readonly int _count;
            private readonly StringSplitOptions _options;
            [CanBeNull] private readonly char[] _separators;

            internal CharsSpliter(ref StringSegment value, [CanBeNull] char[] separators, int count, StringSplitOptions options)
            {
                Debug.Assert(count >= 0);
                if (options != StringSplitOptions.None && options != StringSplitOptions.RemoveEmptyEntries)
                {
                    throw new ArgumentException();
                }
                value.EnsureNotNull();

                this._separators = separators;
                this._value = value;
                this._count = count;
                this._options = options;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public Enumerator GetEnumerator() => new Enumerator(ref this);

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
            
            /// <summary>
            /// Enumerator for <see cref="T:Jasily.Text.StringSegment.CharsSpliter" />.
            /// </summary>
            public struct Enumerator : IEnumerator<StringSegment>
            {
                private readonly StringSegment _value;
                private readonly int _limit;
                private readonly StringSplitOptions _options;
                [CanBeNull] private readonly char[] _separators;
                private int _index;
                private int _count;

                internal Enumerator(ref CharsSpliter tokenizer)
                {
                    // value
                    this._value = tokenizer._value;

                    // argd
                    this._limit = tokenizer._count;
                    this._options = tokenizer._options;
                    this._separators = tokenizer._separators;

                    // init
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                }

                /// <inheritdoc />
                public StringSegment Current { get; private set; }

                object IEnumerator.Current => this.Current;

                /// <inheritdoc />
                public bool MoveNext()
                {
                    if (this._value.Buffer == null || this._count >= this._limit)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    if (this._separators?.Length == 0 || this._limit == 1)
                    {
                        return this.SliceToEnd();
                    }

                    while (true)
                    {
                        if (this._index > this._value.Length)
                        {
                            this.Current = default(StringSegment);
                            return false;
                        }

                        var next = this._separators == null
                            ? this._value.IndexOfWhiteSpace(this._index)
                            : this._value.IndexOfAny(this._separators, this._index);

                        if (next == -1)
                        {
                            return this.SliceToEnd();
                        }

                        if (next == this._index && this._options == StringSplitOptions.RemoveEmptyEntries)
                        {
                            this._index++;
                        }
                        else if (this._count == this._limit - 1)
                        {
                            return this.SliceToEnd();
                        }
                        else
                        {
                            return this.SliceTo(next);
                        }
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private bool SliceTo(int next)
                {
                    this.Current = this._value.SubSegment(this._index, next - this._index);
                    this._index = next + 1;
                    this._count++;
                    return true;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private bool SliceToEnd()
                {
                    var valLen = this._value.Length;

                    if (this._index > valLen ||
                        valLen == this._index && this._options == StringSplitOptions.RemoveEmptyEntries)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    return this.SliceTo(this._value.Length);
                }

                /// <summary>
                /// Throw <see cref="NotSupportedException"/>.
                /// </summary>
                /// <exception cref="NotSupportedException"></exception>
                public void Reset() => throw new NotSupportedException();

                /// <inheritdoc />
                public void Dispose() { }
            }
        }
    }
}