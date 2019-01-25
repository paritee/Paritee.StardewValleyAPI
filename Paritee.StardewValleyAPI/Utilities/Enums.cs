using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Paritee.StardewValleyAPI.Utilities
{
    public class Enums
    {
        public static string GetValue(Enum e)
        {
            Type enumType = e.GetType();
            MemberInfo[] memInfo = enumType.GetMember(e.ToString());
            EnumMemberAttribute attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();

            if (attr == null)
                return e.ToString();

            return attr.Value;
        }
    }
}
