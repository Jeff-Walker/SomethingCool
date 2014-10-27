using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CuriousEnums {
    public class AbstractEnumeration<TIdentifyingValueType> : IComparable<AbstractEnumeration<TIdentifyingValueType>>, IComparable<TIdentifyingValueType>, IEquatable<AbstractEnumeration<TIdentifyingValueType>>
            where TIdentifyingValueType : IComparable<TIdentifyingValueType> {
        readonly bool? _isNamed;
        readonly String _toStringValue;

        public TIdentifyingValueType Value { get; private set; }
        public String Display { get; private set; }

        protected AbstractEnumeration(TIdentifyingValueType value) {
            if (_isNamed.HasValue && _isNamed.Value) {
                throw new ArgumentException("All members must be named, or no members may be named");
            }

            Value = value;
            
            _isNamed = false;
            _toStringValue = String.Format("{0}", value);
        }

        protected AbstractEnumeration(TIdentifyingValueType value, String display) {
            if (_isNamed.HasValue && !_isNamed.Value) {
                throw new ArgumentException("All members must be named, or no members may be named");
            }

            Value = value;
            Display = display;

            _isNamed = true;
            _toStringValue = String.Format("{0}({1})", display, value);
        }

        public override string ToString() {
            return _toStringValue;
        }

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : AbstractEnumeration<TIdentifyingValueType> {
            var type = typeof (TEnum);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<TEnum>();
        }


        // http://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
        /*
         * public static T FromValue<T>(int value) where T : Enumeration, new()
    {
        var matchingItem = parse<T, int>(value, "value", item => item.Value == value);
        return matchingItem;
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
    { 
        var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
        return matchingItem;
    }

    private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
            throw new ApplicationException(message);
        }

        return matchingItem;
    }
         * 
         * */

        public bool Equals(AbstractEnumeration<TIdentifyingValueType> other) {
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
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((AbstractEnumeration<TIdentifyingValueType>) obj);
        }

        public override int GetHashCode() {
            return EqualityComparer<TIdentifyingValueType>.Default.GetHashCode(Value);
        }

        public static bool operator ==(AbstractEnumeration<TIdentifyingValueType> left, AbstractEnumeration<TIdentifyingValueType> right) {
            return Equals(left, right);
        }

        public static bool operator !=(AbstractEnumeration<TIdentifyingValueType> left, AbstractEnumeration<TIdentifyingValueType> right) {
            return !Equals(left, right);
        }

        public int CompareTo(AbstractEnumeration<TIdentifyingValueType> other) {
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