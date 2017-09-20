namespace Jasily.Text
{
    public partial struct StringSegment
    {
        public StringSegment Trim()
        {
            return this.TrimStart().TrimEnd();
        }

        /// <summary>
        /// Removes all leading whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="StringSegment"/>.</returns>
        public StringSegment TrimStart()
        {
            var trimmedStart = this.Offset;
            while (trimmedStart < this.Offset + this.Length)
            {
                if (!char.IsWhiteSpace(this.Buffer, trimmedStart))
                {
                    break;
                }

                trimmedStart++;
            }

            return new StringSegment(this.Buffer, trimmedStart, this.Offset + this.Length - trimmedStart);
        }

        /// <summary>
        /// Removes all trailing whitespaces.
        /// </summary>
        /// <returns>The trimmed <see cref="StringSegment"/>.</returns>
        public StringSegment TrimEnd()
        {
            var trimmedEnd = this.Offset + this.Length - 1;
            while (trimmedEnd >= this.Offset)
            {
                if (!char.IsWhiteSpace(this.Buffer, trimmedEnd))
                {
                    break;
                }

                trimmedEnd--;
            }

            return new StringSegment(this.Buffer, this.Offset, trimmedEnd - this.Offset + 1);
        }
    }
}