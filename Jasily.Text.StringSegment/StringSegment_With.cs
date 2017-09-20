using System;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        public bool StartsWith([NotNull] string text, StringComparison comparisonType)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var textLength = text.Length;
            if (!this.HasValue || this.Length < textLength)
            {
                return false;
            }

            return string.Compare(this.Buffer, this.Offset, text, 0, textLength, comparisonType) == 0;
        }

        public bool EndsWith([NotNull] string text, StringComparison comparisonType)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var textLength = text.Length;
            if (!this.HasValue || this.Length < textLength)
            {
                return false;
            }

            return string.Compare(this.Buffer, this.Offset + this.Length - textLength, text, 0, textLength, comparisonType) == 0;
        }
    }
}