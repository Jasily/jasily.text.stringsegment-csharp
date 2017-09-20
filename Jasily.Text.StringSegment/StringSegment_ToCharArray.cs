using System;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public partial struct StringSegment
    {
        [PublicAPI, Pure, NotNull]
        public char[] ToCharArray()
        {
            this.EnsureNotNull();

            var array = new char[this.Length];
            for (var i = 0; i < this.Length; i++)
            {
                // ReSharper disable once PossibleNullReferenceException
                array[i] = this.Buffer[i + this.Offset];
            }
            return array;
        }
    }
}