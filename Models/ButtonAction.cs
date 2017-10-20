using System;

namespace nothrow.smartbuttons
{
    internal class ButtonAction : IEquatable<ButtonAction>
    {
        public string Command { get; set; }
        public string Pwd { get; set; }

        public bool Equals(ButtonAction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Command, other.Command) && string.Equals(Pwd, other.Pwd);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ButtonAction)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Command != null ? Command.GetHashCode() : 0) * 397) ^ (Pwd != null ? Pwd.GetHashCode() : 0);
            }
        }
    }
}