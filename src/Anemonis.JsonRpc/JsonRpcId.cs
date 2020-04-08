// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Globalization;

using Anemonis.JsonRpc.Resources;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message identifier.</summary>
    public readonly struct JsonRpcId : IEquatable<JsonRpcId>
    {
        private readonly string _valueString;
        private readonly double _valueNumber;
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
            _valueNumber = default;
            _valueString = value;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> is not finite.</exception>
        public JsonRpcId(double value)
        {
            if (!double.IsFinite(value))
            {
                throw new ArgumentException(Strings.GetString("id.float_not_finite"), nameof(value));
            }

            _type = JsonRpcIdType.Number;
            _valueString = default;
            _valueNumber = value;
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
                case JsonRpcIdType.Number:
                    {
                        return _valueNumber == other._valueNumber;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        internal string GetStringValue()
        {
            return _valueString;
        }

        internal double GetNumberValue()
        {
            return _valueNumber;
        }

        /// <inheritdoc />
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
                case double other:
                    {
                        return (_type == JsonRpcIdType.Number) && (_valueNumber == other);
                    }
                default:
                    {
                        return (_type == JsonRpcIdType.None) && (obj is null);
                    }
            }
        }

        /// <inheritdoc />
        public bool Equals(JsonRpcId other)
        {
            return Equals(in other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(_type);

            switch (_type)
            {
                case JsonRpcIdType.String:
                    {
                        hashCode.Add(_valueString);
                    }
                    break;
                case JsonRpcIdType.Number:
                    {
                        hashCode.Add(_valueNumber);
                    }
                    break;
            }

            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
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
                case JsonRpcIdType.Number:
                    {
                        return _valueNumber.ToString(provider);
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
        public static string ToString(in JsonRpcId value)
        {
            return value._valueString;
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="double" />.</summary>
        /// <param name="value">The identifier to get a <see cref="double" /> value from.</param>
        /// <returns>An underlying value of type <see cref="double" />.</returns>
        public static double ToDouble(in JsonRpcId value)
        {
            return value._valueNumber;
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
