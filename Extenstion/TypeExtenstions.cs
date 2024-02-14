namespace FilterPagingEfCore.Extenstion
{
    public static class TypeExtensions
    {
        public static bool IsNotNull(this object @object)
        {
            return !(@object == null);
        }
        public static bool IsNumericType(this object obj)
        {
            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsGuidType(this Type type)
        {
            if (typeof(Guid) == type)
                return true;
            else
                return false;
        }

        public static bool IsDateType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DateTime:
                    return true;
                default:
                    return false;
            }
        }

        public static object ToNumber(this string value, Type type)
        {
            if (value == null) return null;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                    return byte.Parse(value);
                case TypeCode.SByte:
                    return sbyte.Parse(value);

                case TypeCode.UInt16:
                    return ushort.Parse(value);

                case TypeCode.UInt32:
                    return uint.Parse(value);

                case TypeCode.UInt64:
                    return ulong.Parse(value);

                case TypeCode.Int16:
                    return short.Parse(value);

                case TypeCode.Int32:
                    return int.Parse(value);

                case TypeCode.Int64:
                    return long.Parse(value);

                case TypeCode.Decimal:
                    return decimal.Parse(value);

                case TypeCode.Double:
                    return double.Parse(value);

                case TypeCode.Single:
                    return float.Parse(value);

                default:
                    return int.Parse(value);
            }
        }

        public static bool IsSimpleTypeAndNotBooleanAndNotNullable(this Type type)
        {
            var result = (type.IsPrimitive || type.Equals(typeof(string)) || type.Equals(typeof(Guid))) && !type.Equals(typeof(bool));
            return result;
        }

        public static bool IsValidDateTime(this DateTime input)
        {
            return !input.Equals(default);
        }
        public static bool IsValidDateTime(this DateTime? input)
        {
            return input.IsNotNull() && input.IsValidDateTime();
        }
        public static bool IsValidGuid(this Guid input)
        {
            return !(input == default);
        }
        public static bool IsValidGuid(this Guid? input)
        {
            return input.IsNotNull() && input.Value.IsValidGuid();
        }

        public static bool IsValidLong(this long input)
        {
            return !(input == default);
        }
        public static bool IsValidLong(this long? input)
        {
            return input.IsNotNull() && input.Value.IsValidLong();
        }

        public static bool IsValidInt(this int input)
        {
            return !(input == default);
        }
        public static bool IsValidInt(this int? input)
        {
            return input.IsNotNull() && input.Value.IsValidInt();
        }

        public static bool IsGreaterThanZero<T>(this T number) where T : struct, IComparable
        {
            return number.CompareTo(default(T)) > 0;
        }

        public static bool IsGreaterThanZero<T>(this T? number) where T : struct, IComparable
        {
            return number.HasValue && number.Value.IsGreaterThanZero();
        }


    }
}