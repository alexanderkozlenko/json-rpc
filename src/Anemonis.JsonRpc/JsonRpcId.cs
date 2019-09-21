// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Globalization;
using System.Runtime.InteropServices;

using Anemonis.JsonRpc.Resources;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message identifier.</summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct JsonRpcId : IEquatable<JsonRpcId>
    {
        [FieldOffset(0x00)]
        private readonly string _valueString;

        [FieldOffset(0x08)]
        private readonly long _valueInteger;

        [FieldOffset(0x08)]
        private readonly double _valueFloat;

        [FieldOffset(0x10)]
        private readonly JsonRpcIdType _type;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public JsonRpcId(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _type = JsonRpcIdType.String;
            _valueInteger = default;
            _valueFloat = default;
            _valueString = value;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        public JsonRpcId(long value)
        {
            _type = JsonRpcIdType.Integer;
            _valueString = default;
            _valueFloat = default;
            _valueInteger = value;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> is <see cref="double.NaN" />, or <see cref="double.NegativeInfinity" />, or <see cref="double.PositiveInfinity" />.</exception>
        public JsonRpcId(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException(Strings.GetString("id.invalid_float"), nameof(value));
            }

            _type = JsonRpcIdType.Float;
            _valueString = default;
            _valueInteger = default;
            _valueFloat = value;
        }

        private bool Equals(in JsonRpcId other)
        {
            if (_type != other._type)
            {
                return false;
            }

            switch (_type)
            {
                case JsonRpcIdType.String:
                    {
                        return string.Equals(_valueString, other._valueString, StringComparison.Ordinal);
                    }
                case JsonRpcIdType.Integer:
                case JsonRpcIdType.Float:
                    {
                        return _valueInteger == other._valueInteger;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        internal string UnsafeAsString()
        {
            return _valueString;
        }

        internal long UnsafeAsInteger()
        {
            return _valueInteger;
        }

        internal double UnsafeAsFloat()
        {
            return _valueFloat;
        }

        /// <summary>Indicates whether the current <see cref="JsonRpcId" /> is equal to the specified object.</summary>
        /// <param name="obj">The object to compare with the current <see cref="JsonRpcId" />.</param>
        /// <returns><see langword="true" /> if the current <see cref="JsonRpcId" /> is equal to the specified object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case JsonRpcId other:
                    {
                        return Equals(in other);
                    }
                case string other:
                    {
                        return (_type == JsonRpcIdType.String) && string.Equals(_valueString, other, StringComparison.Ordinal);
                    }
                case long other:
                    {
                        return (_type == JsonRpcIdType.Integer) && (_valueInteger == other);
                    }
                case double other:
                    {
                        return (_type == JsonRpcIdType.Float) && (_valueFloat == other);
                    }
                default:
                    {
                        return (_type == JsonRpcIdType.None) && (obj == null);
                    }
            }
        }

        /// <summary>Indicates whether the current <see cref="JsonRpcId" /> is equal to another <see cref="JsonRpcId" />.</summary>
        /// <param name="other">A <see cref="JsonRpcId" /> to compare with the current <see cref="JsonRpcId" />.</param>
        /// <returns><see langword="true" /> if the current <see cref="JsonRpcId" /> is equal to the other <see cref="JsonRpcId" />; otherwise, <see langword="false" />.</returns>
        public bool Equals(JsonRpcId other)
        {
            return Equals(in other);
        }

        /// <summary>Returns the hash code for the current <see cref="JsonRpcId" />.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            // FNV-1a

            unchecked
            {
                var hashCode = HashCode.FNV_OFFSET_BASIS_32;

                hashCode ^= _type.GetHashCode();
                hashCode *= HashCode.FNV_PRIME_32;

                switch (_type)
                {
                    case JsonRpcIdType.String:
                        {
                            hashCode ^= _valueString.GetHashCode();
                            hashCode *= HashCode.FNV_PRIME_32;
                        }
                        break;
                    case JsonRpcIdType.Integer:
                    case JsonRpcIdType.Float:
                        {
                            hashCode ^= _valueInteger.GetHashCode();
                            hashCode *= HashCode.FNV_PRIME_32;
                        }
                        break;
                }

                return hashCode;
            }
        }

        /// <summary>Converts the current <see cref="JsonRpcId" /> to its equivalent string representation.</summary>
        /// <returns>The string representation of the current <see cref="JsonRpcId" />.</returns>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the current <see cref="JsonRpcId" /> to its equivalent string representation.</summary>
        /// <param name="provider">An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current <see cref="JsonRpcId" />.</returns>
        public string ToString(IFormatProvider provider)
        {
            switch (_type)
            {
                case JsonRpcIdType.String:
                    {
                        return _valueString;
                    }
                case JsonRpcIdType.Integer:
                    {
                        return _valueInteger.ToString(provider);
                    }
                case JsonRpcIdType.Float:
                    {
                        return _valueFloat.ToString(provider);
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        /// <summary>Performs an implicit conversion from <see cref="string" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        /// <returns>A new instance of the <see cref="JsonRpcId" /> structure.</returns>
        public static JsonRpcId FromString(string value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="ulong" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        /// <returns>A new instance of the <see cref="JsonRpcId" /> structure.</returns>
        public static JsonRpcId FromInt64(long value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="double" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        /// <returns>A new instance of the <see cref="JsonRpcId" /> structure.</returns>
        public static JsonRpcId FromDouble(double value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="string" />.</summary>
        /// <param name="value">The identifier to get a <see cref="string" /> value from.</param>
        /// <returns>An underlying value of type <see cref="string" />.</returns>
        /// <exception cref="InvalidCastException">The underlying value is not of type <see cref="string" />.</exception>
        public static string ToString(in JsonRpcId value)
        {
            if (value._type != JsonRpcIdType.String)
            {
                throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("id.invalid_cast"), typeof(JsonRpcId), typeof(string)));
            }

            return value._valueString;
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="long" />.</summary>
        /// <param name="value">The identifier to get a <see cref="long" /> value from.</param>
        /// <returns>An underlying value of type <see cref="long" />.</returns>
        /// <exception cref="InvalidCastException">The underlying value is not of type <see cref="long" />.</exception>
        public static long ToInt64(in JsonRpcId value)
        {
            if (value._type != JsonRpcIdType.Integer)
            {
                throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("id.invalid_cast"), typeof(JsonRpcId), typeof(long)));
            }

            return value._valueInteger;
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="double" />.</summary>
        /// <param name="value">The identifier to get a <see cref="double" /> value from.</param>
        /// <returns>An underlying value of type <see cref="double" />.</returns>
        /// <exception cref="InvalidCastException">The underlying value is not of type <see cref="double" />.</exception>
        public static double ToDouble(in JsonRpcId value)
        {
            if (value._type != JsonRpcIdType.Float)
            {
                throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("id.invalid_cast"), typeof(JsonRpcId), typeof(double)));
            }

            return value._valueFloat;
        }

        /// <summary>Indicates whether the left <see cref="JsonRpcId" /> is equal to the right <see cref="JsonRpcId" />.</summary>
        /// <param name="obj1">The left <see cref="JsonRpcId" /> operand.</param>
        /// <param name="obj2">The right <see cref="JsonRpcId" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="JsonRpcId" /> is equal to the right <see cref="JsonRpcId" />; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(in JsonRpcId obj1, in JsonRpcId obj2)
        {
            return obj1.Equals(in obj2);
        }

        /// <summary>Indicates whether the left <see cref="JsonRpcId" /> is not equal to the right <see cref="JsonRpcId" />.</summary>
        /// <param name="obj1">The left <see cref="JsonRpcId" /> operand.</param>
        /// <param name="obj2">The right <see cref="JsonRpcId" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="JsonRpcId" /> is not equal to the right <see cref="JsonRpcId" />; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(in JsonRpcId obj1, in JsonRpcId obj2)
        {
            return !obj1.Equals(in obj2);
        }

        /// <summary>Performs an implicit conversion from <see cref="string" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        public static implicit operator JsonRpcId(string value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="ulong" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        public static implicit operator JsonRpcId(long value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="double" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        public static implicit operator JsonRpcId(double value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="string" />.</summary>
        /// <param name="value">The identifier to get a <see cref="string" /> value from.</param>
        public static explicit operator string(in JsonRpcId value)
        {
            return ToString(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="long" />.</summary>
        /// <param name="value">The identifier to get a <see cref="long" /> value from.</param>
        public static explicit operator long(in JsonRpcId value)
        {
            return ToInt64(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="double" />.</summary>
        /// <param name="value">The identifier to get a <see cref="double" /> value from.</param>
        public static explicit operator double(in JsonRpcId value)
        {
            return ToDouble(value);
        }

        /// <summary>Gets the JSON-RPC message identifier type.</summary>
        public JsonRpcIdType Type
        {
            get => _type;
        }
    }
}
