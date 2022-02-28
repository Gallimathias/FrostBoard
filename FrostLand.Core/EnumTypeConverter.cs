using System;
using System.ComponentModel;
using System.Globalization;

namespace FrostLand.Core;

public class EnumTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(byte) 
            || sourceType == typeof(sbyte)
            || sourceType == typeof(short) 
            || sourceType == typeof(ushort) 
            || sourceType == typeof(int)
            || sourceType == typeof(uint)
            || sourceType == typeof(long)
            || sourceType == typeof(ulong)
            || sourceType.IsEnum;
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value; 
    }    
}
