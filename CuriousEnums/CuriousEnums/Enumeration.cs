using System;
using System.Collections.Generic;

namespace CuriousEnums {
    public abstract class TypeSafeEnumeration : IComparable<TypeSafeEnumeration>, IComparable<int>, IEquatable<TypeSafeEnumeration> {
        static int _nextOrdinal = 0;
        public int Ordinal { get; private set; }
        public String Display { get; private set; }

        protected TypeSafeEnumeration(string display) {
            Ordinal = _nextOrdinal++;
            Display = display;
        }

        public bool Equals(TypeSafeEnumeration other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Ordinal == other.Ordinal;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((TypeSafeEnumeration) obj);
        }

        public override int GetHashCode() {
            return Ordinal;
        }

        public static bool operator ==(TypeSafeEnumeration left, TypeSafeEnumeration right) {
            return Equals(left, right);
        }

        public static bool operator !=(TypeSafeEnumeration left, TypeSafeEnumeration right) {
            return !Equals(left, right);
        }

        public int CompareTo(TypeSafeEnumeration other) {
            return Ordinal.CompareTo(other.Ordinal);
        }

        public int CompareTo(int other) {
            return Ordinal.CompareTo(other);
        }
    }
    public abstract class TypeSafeEnumeration<TIdentifyingValueType> : IComparable<TypeSafeEnumeration<TIdentifyingValueType>>,
            IComparable<TIdentifyingValueType>, IEquatable<TypeSafeEnumeration<TIdentifyingValueType>>
            where TIdentifyingValueType : IComparable<TIdentifyingValueType> {

        readonly static IDictionary<TIdentifyingValueType, TypeSafeEnumeration<TIdentifyingValueType>> ValueMap = new Dictionary<TIdentifyingValueType, TypeSafeEnumeration<TIdentifyingValueType>>();
        readonly static IDictionary<String, TypeSafeEnumeration<TIdentifyingValueType>> DisplayMap = new Dictionary<String, TypeSafeEnumeration<TIdentifyingValueType>>();
        readonly string _toStringValue;

        public TIdentifyingValueType Value { get; private set; }
        public String Display { get; private set; }

        protected TypeSafeEnumeration(TIdentifyingValueType value, String display) {
            Value = value;
            Display = display;

            _toStringValue = String.Format("{0}({1})", display, value);

            ValueMap.Add(value, this);
            DisplayMap.Add(display, this);
        }

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : TypeSafeEnumeration<TIdentifyingValueType> {
            return ValueMap.Values as IEnumerable<TEnum>;
        }

        public static TypeSafeEnumeration<TIdentifyingValueType> FromValue(TIdentifyingValueType value) {
            return ValueMap[value];
        }
        public static TypeSafeEnumeration<TIdentifyingValueType> FromDisplayName(string display) {
            return DisplayMap[display];
        }

        public override string ToString() {
            return _toStringValue;
        }

        public bool Equals(TypeSafeEnumeration<TIdentifyingValueType> other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return EqualityComparer<TIdentifyingValueType>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((TypeSafeEnumeration<TIdentifyingValueType>)obj);
        }

        public override int GetHashCode() {
            return EqualityComparer<TIdentifyingValueType>.Default.GetHashCode(Value);
        }

        public static bool operator ==(TypeSafeEnumeration<TIdentifyingValueType> left, TypeSafeEnumeration<TIdentifyingValueType> right) {
            return Equals(left, right);
        }

        public static bool operator !=(TypeSafeEnumeration<TIdentifyingValueType> left, TypeSafeEnumeration<TIdentifyingValueType> right) {
            return !Equals(left, right);
        }

        public int CompareTo(TypeSafeEnumeration<TIdentifyingValueType> other) {
            return CompareTo(other.Value);
        }

        public int CompareTo(TIdentifyingValueType other) {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (Value == null) {
                return other == null ? 0 : 1;
            }
            // ReSharper restore CompareNonConstrainedGenericWithNull
            return Value.CompareTo(other);
        }
    }


}