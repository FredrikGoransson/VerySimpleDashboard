using System;
using System.Globalization;

namespace VerySimpleDashboard.Data
{
    public static class DataTypeParser
    {
        public static object ParseValue(object value, DataType dataType, CultureInfo cultureInfo)
        {
            switch (dataType)
            {
                case (DataType.Integer):
                    return ParseIntegerValue(value, cultureInfo);

                case (DataType.Double):
                    return ParseDoubleValue(value, cultureInfo);

                case (DataType.Boolean):
                    return ParseBooleanValue(value, cultureInfo);

                case (DataType.DateTime):
                    return ParseDateTimeValue(value, cultureInfo);

                case (DataType.String):
                    return value as string;

            }
            throw new NotSupportedException(string.Format("Could not parse the value, the datatype {0} is not supported", dataType));
        }

        public static bool TestValue(object value, DataType dataType, CultureInfo cultureInfo)
        {
            switch (dataType)
            {
                case (DataType.Integer):
                    return TestIntegerValue(value, cultureInfo);

                case (DataType.Double):
                    return TestDoubleValue(value, cultureInfo);

                case (DataType.Boolean):
                    return TestBooleanValue(value, cultureInfo);

                case (DataType.DateTime):
                    return TestDateTimeValue(value, cultureInfo);

                case (DataType.String):
                    return value != null;

            }
            return false;
        }

        public static bool TestIntegerValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return false;

            var result = -1;
            return int.TryParse(value.ToString(), 
                NumberStyles.Integer | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite, 
                cultureInfo, 
                out result);
        }

        public static bool TestDoubleValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return false;

            var result = -1.0;
            return double.TryParse(value.ToString(), 
                NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite,
                cultureInfo,
                out result);
        }

        public static bool TestBooleanValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return false;

            var result = false;
            return bool.TryParse(value.ToString(), out result);
        }

        public static bool TestDateTimeValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return false;

            var result = DateTime.MinValue;
            return DateTime.TryParse(value.ToString(), out result);
        }

        public static int? ParseIntegerValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return null;
            return int.Parse(value.ToString(),
                NumberStyles.Integer | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite,
                cultureInfo);
        }

        public static double? ParseDoubleValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return null;
            return double.Parse(value.ToString(),
                NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite,
                cultureInfo);
        }

        public static bool? ParseBooleanValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return null;
            return bool.Parse(value.ToString());
        }

        public static DateTime? ParseDateTimeValue(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return null;
            return DateTime.Parse(value.ToString());
        }
    }
}