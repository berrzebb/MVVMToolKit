using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace MVVMToolKit.Converters
{
    /// <summary>
    /// Enum Type을 변환합니다.
    /// </summary>
    public class EnumTypeConverter : EnumConverter
    {
        /// <summary>
        ///  Enum Type 변환 생성자
        /// </summary>
        /// <param name="enumType">변환할 Enum Type</param>
        public EnumTypeConverter(Type enumType)
            : base(enumType)
        {
        }
        /// <inheritdoc/>

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(string);
        }
        /// <inheritdoc/>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value == null) return base.ConvertTo(context, culture, value, destinationType);
            FieldInfo? fi = EnumType.GetField(Enum.GetName(this.EnumType, value)!);
            if (fi != null && Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)) is DescriptionAttribute description)
            {
                return description.Description;
            }


            return $"{value}({(int)value})";
        }
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
        /// <inheritdoc/>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
        {
            object? result = null;
            if (value is string stringValue)
            {
                foreach (FieldInfo fi in EnumType.GetFields())
                {
                    if (Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)) is not DescriptionAttribute
                        description) continue;
                    if (stringValue == description.Description && Enum.TryParse(EnumType, fi.Name, out result))
                    {
                        return result;
                    }
                }

                if (Enum.TryParse(EnumType, stringValue, out result))
                {
                    return result;
                }
            }

            return result;
        }
    }
}
