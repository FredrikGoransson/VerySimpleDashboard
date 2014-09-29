using System;
using System.Globalization;

namespace VerySimpleDashboard.Data
{
    public static class DataTypeParser
    {
        public static bool TestValue(object value, DataType dataType, System.Globalization.CultureInfo cultureInfo)
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
    }
}