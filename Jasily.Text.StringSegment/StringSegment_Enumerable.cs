using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment : IEnumerable<char>
    {
        [PublicAPI, Pure]
        public CharEnumerator GetEnumerator()
        {
            this.EnsureNotNull();
            return new CharEnumerator(this);
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public struct CharEnumerator : IEnumerator<char>
        {
            private readonly StringSegment _segment;
            private int _offset;

            internal CharEnumerator(StringSegment segment)
            {
                this._segment = segment;
                this._offset = -1;
                this.Current = default(char);
            }

            public bool MoveNext()
            {
                if (this._segment.Buffer == null) return false;
                this._offset++;
                if (this._segment.Length <= this._offset) return false;
                this.Current = this._segment[this._segment.Offset + this._offset];
                return true;
            }

            public void Reset() => this._offset = -1;

            public char Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public void Dispose() { }
        }
    }
}