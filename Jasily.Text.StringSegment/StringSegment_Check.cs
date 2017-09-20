using System;
using JetBrains.Annotations;
// ReSharper disable PureAttributeOnVoidMethod

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        /// <summary>
        /// Allow <see cref="Buffer"/> be <see langword="null"/>.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        [Pure]
        private void EnsureSegmentRange(int startIndex, int count)
        {
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (this.Buffer == null) return;

            if (startIndex > this.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + count > this.Length) throw new ArgumentOutOfRangeException(nameof(count));
        }

        /// <summary>
        /// Throw <see cref="InvalidOperationException"/> if <see cref="Buffer"/> is <see langword="null"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [Pure]
        internal void EnsureNotNull()
        {
            if (this.Buffer == null) throw new InvalidOperationException("The Buffer is null.");
        }
    }
}