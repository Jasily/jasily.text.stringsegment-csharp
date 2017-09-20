using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI, Pure]
        public static bool IsNullOrEmpty(StringSegment value)
        {
            return value.Length == 0;
        }

        [PublicAPI, Pure]
        public static bool IsNullOrWhiteSpace(StringSegment value)
        {
            if (value.Length == 0) return true;

            var length = value.Offset + value.Length;
            for (var i = value.Offset; i < length; i++)
            {
                if (!char.IsWhiteSpace(value.Buffer, i))
                {
                    return false;
                }
            }
            return true;
        }
    }
}