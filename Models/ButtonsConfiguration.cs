using System;
using System.Collections.Generic;
using System.Linq;

namespace nothrow.smartbuttons
{
    internal class ButtonsConfiguration : IEquatable<ButtonsConfiguration>
    {
        public List<ButtonConfiguration> Buttons { get; set; } = new List<ButtonConfiguration>();

        public bool Equals(ButtonsConfiguration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Buttons.SequenceEqual(other.Buttons);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ButtonsConfiguration)obj);
        }

        public override int GetHashCode()
        {
            return Buttons != null ? Buttons.GetHashCode() : 0;
        }
    }
}