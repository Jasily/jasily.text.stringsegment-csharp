using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        public StringsSpliter Split(string separator)
        {
            this.EnsureNotNull();
            return new StringsSpliter(this, new[] {separator}, -1, StringSplitOptions.None);
        }

        public StringsSpliter Split(string[] separator, StringSplitOptions options)
        {
            this.EnsureNotNull();
            return new StringsSpliter(this, separator, -1, options);
        }

        public StringsSpliter Split(string[] separator, int count, StringSplitOptions options)
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
            private readonly string[] _separators;

            internal StringsSpliter(StringSegment value, string[] separators, int count, StringSplitOptions options)
            {
                this._separators = separators ?? throw new ArgumentNullException(nameof(separators));
                this._value = value;
                this._count = count;
                this._options = options;
            }

            public Enumerator GetEnumerator() => new Enumerator(ref this);

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<StringSegment>
            {
                private readonly StringSegment _value;
                private readonly int _limit;
                private readonly StringSplitOptions _options;
                private readonly string[] _separators;
                private int _index;
                private int _count;
                private readonly int[] _nextIndexCache;

                internal Enumerator(ref StringsSpliter tokenizer)
                {
                    this._separators = tokenizer._separators;
                    this._value = tokenizer._value;
                    this._limit = tokenizer._count;
                    this._options = tokenizer._options;
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                    this._nextIndexCache = new int[tokenizer._separators.Length];
                }

                public StringSegment Current { get; private set; }

                object IEnumerator.Current => this.Current;

                public bool MoveNext()
                {
                    if (this._value.Buffer == null)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    if (this._limit >= 0)
                    {
                        if (this._count >= this._limit)
                        {
                            this.Current = default(StringSegment);
                            return false;
                        }

                        if (this._limit == 1)
                        {
                            return this.SliceToEnd();
                        }
                    }

                    while (true)
                    {
                        if (this._index > this._value.Length)
                        {
                            this.Current = default(StringSegment);
                            return false;
                        }

                        var next = this.IndexOfAny(out var separator);

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

                private int IndexOfAny(out string separator)
                {
                    separator = null;
                    var next = int.MaxValue;

                    for (var i = 0; i < this._separators.Length; i++)
                    {
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
                    var next = this._value.Length;

                    if (next == this._index && this._options == StringSplitOptions.RemoveEmptyEntries)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    return this.SliceTo(this._value.Length, null);
                }

                public void Reset()
                {
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                    Array.Clear(this._nextIndexCache, 0, this._nextIndexCache.Length);
                }

                public void Dispose() { }
            }
        }

        public CharsSpliter Split(char ch)
        {
            this.EnsureNotNull();
            return new CharsSpliter(this, new[] { ch }, -1, StringSplitOptions.None);
        }

        public CharsSpliter Split(params char[] separator)
        {
            this.EnsureNotNull();
            return new CharsSpliter(this, separator, -1, StringSplitOptions.None);
        }

        public CharsSpliter Split(char[] separator, StringSplitOptions options)
        {
            this.EnsureNotNull();
            return new CharsSpliter(this, separator, -1, options);
        }

        public CharsSpliter Split(char[] separator, int count, StringSplitOptions options = StringSplitOptions.None)
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
            private readonly char[] _separators;

            internal CharsSpliter(StringSegment value, char[] separators, int count, StringSplitOptions options)
            {
                this._separators = separators ?? throw new ArgumentNullException(nameof(separators));
                this._value = value;
                this._count = count;
                this._options = options;
            }

            public Enumerator GetEnumerator() => new Enumerator(ref this);

            IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public struct Enumerator : IEnumerator<StringSegment>
            {
                private readonly StringSegment _value;
                private readonly int _limit;
                private readonly StringSplitOptions _options;
                private readonly char[] _separators;
                private int _index;
                private int _count;

                internal Enumerator(ref CharsSpliter tokenizer)
                {
                    this._separators = tokenizer._separators;
                    this._value = tokenizer._value;
                    this._limit = tokenizer._count;
                    this._options = tokenizer._options;
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                }

                public StringSegment Current { get; private set; }

                object IEnumerator.Current => this.Current;

                public bool MoveNext()
                {
                    if (this._value.Buffer == null)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    if (this._limit >= 0)
                    {
                        if (this._count >= this._limit)
                        {
                            this.Current = default(StringSegment);
                            return false;
                        }

                        if (this._limit == 1)
                        {
                            return this.SliceToEnd();
                        }
                    }

                    while (true)
                    {
                        if (this._index > this._value.Length)
                        {
                            this.Current = default(StringSegment);
                            return false;
                        }

                        var next = this._value.IndexOfAny(this._separators, this._index);

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
                    var next = this._value.Length;

                    if (next == this._index && this._options == StringSplitOptions.RemoveEmptyEntries)
                    {
                        this.Current = default(StringSegment);
                        return false;
                    }

                    return this.SliceTo(this._value.Length);
                }

                public void Reset()
                {
                    this.Current = default(StringSegment);
                    this._index = 0;
                    this._count = 0;
                }

                public void Dispose() { }
            }
        }
    }
}