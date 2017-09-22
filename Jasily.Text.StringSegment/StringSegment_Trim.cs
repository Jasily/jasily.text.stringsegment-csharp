using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI, Pure]
        public StringSegment Trim() => this.TrimStart().TrimEnd();

        /// <summary>
        /// Removes all leading whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="StringSegment"/>.</returns>
        [PublicAPI, Pure]
        public StringSegment TrimStart()
        {
            if (this.Buffer == null) return this;

            var trimmedStart = this.Offset;
            while (trimmedStart < this.Offset + this.Length && char.IsWhiteSpace(this.Buffer, trimmedStart))
            {
                trimmedStart++;
            }

            return new StringSegment(this.Buffer, trimmedStart, this.Offset + this.Length - trimmedStart);
        }

        /// <summary>
        /// Removes all trailing whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="StringSegment"/>.</returns>
        [PublicAPI, Pure]
        public StringSegment TrimEnd()
        {
            if (this.Buffer == null) return this;

            var trimmedEnd = this.Offset + this.Length - 1;
            while (trimmedEnd >= this.Offset && char.IsWhiteSpace(this.Buffer, trimmedEnd))
            {
                trimmedEnd--;
            }

            return new StringSegment(this.Buffer, this.Offset, trimmedEnd - this.Offset + 1);
        }
    }
}