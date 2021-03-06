﻿using System;
using System.Reflection;

namespace WarcraftGuild.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumCodeAttribute : Attribute
    {
        public EnumCodeAttribute(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }

    public static class EnumExtensions
    {
        public static string GetCode<TEnum>(this TEnum value) where TEnum : Enum
        {
            Type type = typeof(TEnum);
            var field = type.GetField(value.ToString());
            var attr = field.GetCustomAttribute(typeof(EnumCodeAttribute)) as EnumCodeAttribute;

            return attr?.Code ?? value.ToString();
        }
    }
}
