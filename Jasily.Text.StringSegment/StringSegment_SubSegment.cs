using System;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI, Pure]
        public StringSegment SubSegment(int offset)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

            this.EnsureNotNull();
            if (offset > this.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            // ReSharper disable once AssignNullToNotNullAttribute
            return new StringSegment(this.Buffer, this.Offset + offset, this.Length - offset);
        }

        [PublicAPI, Pure]
        public StringSegment SubSegment(int offset, int length)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            this.EnsureNotNull();
            if (offset > this.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + length > this.Length) throw new ArgumentOutOfRangeException(nameof(length));

            // ReSharper disable once AssignNullToNotNullAttribute
            return new StringSegment(this.Buffer, this.Offset + offset, length);
        }

        [PublicAPI, Pure]
        public StringSegment TakeSegment(int count)
        {
            this.EnsureNotNull();
            if (count < 0 || count > this.Length) throw new ArgumentOutOfRangeException(nameof(count));
            // ReSharper disable once AssignNullToNotNullAttribute
            return new StringSegment(this.Buffer, this.Offset, count);
        }
    }
}