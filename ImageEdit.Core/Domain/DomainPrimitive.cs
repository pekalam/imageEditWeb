using System;

namespace ImageEdit.Core.Domain
{
    public class DomainPrimitive<TVal, TSubcl> : IEquatable<TSubcl>, IComparable<TSubcl>
        where TSubcl : DomainPrimitive<TVal, TSubcl> where TVal : IComparable<TVal>, IConvertible
    {
        public TVal Value { get; }

        public DomainPrimitive(TVal value)
        {
            Value = value;
        }

        public bool Equals(TSubcl other)
        {
            if (other == null)
                return false;
            return Value.Equals(other.Value);
        }

        public int CompareTo(TSubcl other)
        {
            if (other == null)
                return 1;
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object obj) => (obj is TSubcl) && ((TSubcl)obj).Value.Equals(Value);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
    }
}