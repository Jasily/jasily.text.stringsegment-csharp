using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        private interface ISpiterEnumerator
        {
            StringSegment Value { get; }

            int Count { get; }

            int Limit { get; }

            int Index { get; }

            StringSplitOptions Options { get; }
        }

        private class SpliterHelper
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool HasNext<T>(ref T itor) where T : ISpiterEnumerator
            {
                if (itor.Value.Buffer == null)
                {
                    return false;
                }

                if (itor.Limit >= 0 && itor.Count >= itor.Limit)
                {
                    return false;
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool HasEnd<T>(ref T itor) where T : ISpiterEnumerator
            {
                var valLen = itor.Value.Length;

                if (itor.Index > valLen)
                {
                    return false;
                }

                if (valLen == itor.Index && itor.Options == StringSplitOptions.RemoveEmptyEntries)
                {
                    return false;
                }

                return true;
            }
        }

        public StringsSpliter Split(string separator)
        {
            this.EnsureNotNull();
            return new StringsSpliter(this, new[] {separator}, -1, StringSplitOptions.None);
        }

        public StringsSpliter Split([CanBeNull] string[] separator, StringSplitOptions options)
        {
            this.EnsureNotNull();
            return new StringsSpliter(this, separator, -1, options);
        }

        public StringsSpliter Split([CanBeNull] string[] separator, int count, StringSplitOptions options)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            this.EnsureNotNull();
            return new StringsSpliter(this, separator, count, options);
        }

        public struct StringsSpliter : IEnumerable<StringSegment>
        {
            private readonly StringSegment _value;
            private readonly int _count;
            private readonly StringSplitOptions _options;
            [CanBeNull] private readonly string[] _separators;

            internal StringsSpliter(StringSegment value, [CanBeNull] string[] separators, int count, StringSplitOptions options)
            {
                this._separators = separators;
                this._value = value;
                this._count = count;
                this._options = options;
            }

            public Enumerator GetEnumerator() => new Enumerator(ref this);

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<StringSegment>, ISpiterEnumerator
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

                public StringSegment Current { get; private set; }

                object IEnumerator.Current => this.Current;

                public bool MoveNext()
                {
                    if (!SpliterHelper.HasNext(ref this))
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
                        else if (this._limit >= 0 && this._count == this._limit - 1)
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
                    if (SpliterHelper.HasEnd(ref this))
                    {
                        return this.SliceTo(this._value.Length, null);
                    }
                    else
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }
                }

                /// <inheritdoc />
                public void Reset()
                {
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                    if (this._nextIndexCache != null)
                    {
                        Array.Clear(this._nextIndexCache, 0, this._nextIndexCache.Length);
                    }
                }

                public void Dispose() { }

                #region ISpiterEnumerator

                StringSegment ISpiterEnumerator.Value => this._value;

                int ISpiterEnumerator.Count => this._count;

                int ISpiterEnumerator.Limit => this._limit;

                int ISpiterEnumerator.Index => this._index;

                StringSplitOptions ISpiterEnumerator.Options => this._options;

                #endregion
            }
        }

        public CharsSpliter Split(char ch)
        {
            this.EnsureNotNull();
            return new CharsSpliter(this, new[] { ch }, -1, StringSplitOptions.None);
        }

        public CharsSpliter Split([CanBeNull] params char[] separator)
        {
            this.EnsureNotNull();
            return new CharsSpliter(this, separator, -1, StringSplitOptions.None);
        }

        public CharsSpliter Split([CanBeNull] char[] separator, StringSplitOptions options)
        {
            this.EnsureNotNull();
            return new CharsSpliter(this, separator, -1, options);
        }

        public CharsSpliter Split([CanBeNull] char[] separator, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            this.EnsureNotNull();
            return new CharsSpliter(this, separator, count, options);
        }

        public struct CharsSpliter : IEnumerable<StringSegment>
        {
            private readonly StringSegment _value;
            private readonly int _count;
            private readonly StringSplitOptions _options;
            [CanBeNull] private readonly char[] _separators;

            internal CharsSpliter(StringSegment value, [CanBeNull] char[] separators, int count, StringSplitOptions options)
            {
                this._separators = separators;
                this._value = value;
                this._count = count;
                this._options = options;
            }

            public Enumerator GetEnumerator() => new Enumerator(ref this);

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<StringSegment>, ISpiterEnumerator
            {
                private readonly StringSegment _value;
                private readonly int _limit;
                private readonly StringSplitOptions _options;
                [CanBeNull] private readonly char[] _separators;
                private int _index;
                private int _count;

                internal Enumerator(ref CharsSpliter tokenizer)
                {
                    this._separators = tokenizer._separators;
                    this._value = tokenizer._value;
                    this._options = tokenizer._options;
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                    this._limit = this._separators?.Length == 0 ? Math.Min(1, tokenizer._count) : tokenizer._count;
                }

                public StringSegment Current { get; private set; }

                object IEnumerator.Current => this.Current;

                public bool MoveNext()
                {
                    if (!SpliterHelper.HasNext(ref this))
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
                        else if (this._limit >= 0 && this._count == this._limit - 1)
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
                    if (SpliterHelper.HasEnd(ref this))
                    {
                        return this.SliceTo(this._value.Length);
                    }
                    else
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }
                }

                public void Reset()
                {
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                }

                public void Dispose() { }

                #region ISpiterEnumerator

                StringSegment ISpiterEnumerator.Value => this._value;

                int ISpiterEnumerator.Count => this._count;

                int ISpiterEnumerator.Limit => this._limit;

                int ISpiterEnumerator.Index => this._index;

                StringSplitOptions ISpiterEnumerator.Options => this._options;

                #endregion
            }
        }
    }
}