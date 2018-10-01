using System;

namespace WDNUtils.Common
{
    /// <summary>
    /// Enum extensions
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Retrieves an array of the values of the constants in a specified enumeration
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <returns>An array that contains the values of the constants in enumType</returns>
        /// <exception cref="ArgumentException">T is not an System.Enum</exception>
        /// <exception cref="InvalidOperationException">The method is invoked by reflection in a reflection-only context, -or- T is a type from an assembly loaded in a reflection-only context</exception>
        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Determines whether one or more bit fields are set in an enum value
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="flags">The combinations of bit fields to compare with the value</param>
        /// <returns>true if one of the combination of bit fields are set in the specified value; otherwise, false</returns>
        /// <exception cref="ArgumentException">flag is a different type than the current instance</exception>
        public static bool HasAnyFlag(this Enum value, params Enum[] flags)
        {
            foreach (var item in flags)
            {
                if (value.HasFlag(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether any of the specified bits are set in an enum value
        /// </summary>
        /// <param name="value">An enumeration value</param>
        /// <param name="bitMask">The bits to compare with the value</param>
        /// <returns>true if one of the specified bits are set in the value; otherwise, false</returns>
        /// <exception cref="ArgumentException">flag is a different type than the current instance</exception>
        public static bool HasAnyBit(this Enum value, Enum bitMask)
        {
            if (!value.GetType().Equals(bitMask.GetType()))
                throw new InvalidOperationException();

            switch (value.GetTypeCode())
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return (Convert.ToInt64(value) & Convert.ToInt64(bitMask)) != 0;

                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return (Convert.ToUInt64(value) & Convert.ToUInt64(bitMask)) != 0;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Converts the specified nullable 8-bit unsigned integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(byte? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 8-bit signed integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(sbyte? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 16-bit signed integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(short? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 16-bit unsigned integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(ushort? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 32-bit signed integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(int? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 32-bit unsigned integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(uint? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 64-bit signed integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(long? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified nullable 8-bit unsigned integer to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <param name="nullValue">The enum to be returned if the value is null</param>
        /// <returns>The value converted to the enumeration; or nullValue if the value is null</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(ulong? value, T nullValue)
        {
            return (!value.HasValue) ? nullValue : ConvertEnum<T>(value.Value);
        }

        /// <summary>
        /// Converts the specified number to an enumeration member
        /// </summary>
        /// <typeparam name="T">The enumeration type to return</typeparam>
        /// <param name="value">The value to convert to an enumeration member</param>
        /// <returns>The value converted to the enumeration</returns>
        /// <exception cref="ArgumentException">T is not an enum type</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value is not defined in the T enumeration</exception>
        public static T ConvertEnum<T>(decimal value)
        {
            switch (Type.GetTypeCode(Enum.GetUnderlyingType(typeof(T))))
            {
                case TypeCode.SByte:
                    {
                        var val = checked((sbyte)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.Int16:
                    {
                        var val = checked((short)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.Int32:
                    {
                        var val = checked((int)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.Int64:
                    {
                        var val = checked((long)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.Byte:
                    {
                        var val = checked((byte)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.UInt16:
                    {
                        var val = checked((ushort)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.UInt32:
                    {
                        var val = checked((uint)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                case TypeCode.UInt64:
                    {
                        var val = checked((ulong)value);

                        if (!Enum.IsDefined(typeof(T), val))
                            throw new ArgumentOutOfRangeException(nameof(value));

                        return (T)Enum.ToObject(typeof(T), val);
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
