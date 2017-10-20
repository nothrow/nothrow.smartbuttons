using System;

namespace nothrow.smartbuttons
{
    internal class ButtonConfiguration : IEquatable<ButtonConfiguration>
    {
        public string Caption { get; set; }
        public ButtonAction Action { get; set; }

        public bool Equals(ButtonConfiguration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Caption, other.Caption) && Equals(Action, other.Action);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ButtonConfiguration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Caption != null ? Caption.GetHashCode() : 0) * 397) ^
                       (Action != null ? Action.GetHashCode() : 0);
            }
        }
    }
}