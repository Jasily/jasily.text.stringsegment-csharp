using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI, Pure]
        public static implicit operator StringSegment(string value)
        {
            return new StringSegment(value);
        }
    }
}