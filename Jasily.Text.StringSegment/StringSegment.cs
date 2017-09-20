using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI]
        public static readonly StringSegment Empty = string.Empty;

        public StringSegment([CanBeNull] string buffer)
        {
            this.Buffer = buffer;
            this.Offset = 0;
            this.Length = buffer?.Length ?? 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringSegment([NotNull] string buffer, int offset)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            this.Buffer = buffer;
            this.Offset = offset;
            this.Length = buffer.Length - offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringSegment([NotNull] string buffer, int offset, int length)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0 || offset + length > buffer.Length) throw new ArgumentOutOfRangeException(nameof(length));

            this.Buffer = buffer;
            this.Offset = offset;
            this.Length = length;
        }

        [PublicAPI, CanBeNull]
        public string Buffer { get; }
        
        [PublicAPI]
        public int Offset { get; }

        [PublicAPI]
        public int Length { get; }

        [PublicAPI, CanBeNull]
        public string Value => this.Buffer?.Substring(this.Offset, this.Length);

        /// <summary>
        /// Get whether <see cref="Buffer"/> is <see langword="null"/>.
        /// </summary>
        [PublicAPI]
        public bool HasValue => this.Buffer != null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [PublicAPI]
        public char this[int index]
        {
            get
            {
                this.EnsureNotNull();

                if (index < 0 || index >= this.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                // ReSharper disable once PossibleNullReferenceException
                return this.Buffer[this.Offset + index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>return <see cref="string.Empty"/> if does not <see cref="HasValue"/>.</returns>
        [PublicAPI, Pure, NotNull]
        public override string ToString()
        {
            return this.Value ?? string.Empty;
        }

        private int LastIndex => this.Length == 0 ? 0 : this.Length - 1;
    }
}
