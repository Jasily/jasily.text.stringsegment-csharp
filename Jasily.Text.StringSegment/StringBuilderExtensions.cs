using System;
using System.Text;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public static class StringBuilderExtensions
    {
        [PublicAPI]
        public static StringBuilder Append([NotNull] this StringBuilder builder, StringSegment segment)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            segment.EnsureNotNull();
            builder.Append(segment.Buffer, segment.Offset, segment.Length);
            return builder;
        }
    }
}