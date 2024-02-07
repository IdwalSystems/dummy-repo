using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Reflection;

namespace MSNK.Models.Helper
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName()!;
        }

        public static int GetDisplayCode(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }
}
