using System;
using System.Linq;
using System.Reflection;

// from propersave

namespace Thunderstore.PackageManager.PSTinyJson
{
    // Token: 0x02000018 RID: 24
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal sealed class DiscoverObjectTypeAttribute : Attribute
    {
        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000095 RID: 149 RVA: 0x00005606 File Offset: 0x00003806
        private string MemberName { get; }

        // Token: 0x06000096 RID: 150 RVA: 0x0000560E File Offset: 0x0000380E
        public DiscoverObjectTypeAttribute(string memberName)
        {
            this.MemberName = memberName;
        }

        // Token: 0x06000097 RID: 151 RVA: 0x00005620 File Offset: 0x00003820
        public Type GetObjectType(object instance)
        {
            if (this.MemberName == null)
            {
                return null;
            }
            MemberInfo memberInfo;
            if (instance == null)
            {
                memberInfo = null;
            }
            else
            {
                Type type = instance.GetType();
                if (type == null)
                {
                    memberInfo = null;
                }
                else
                {
                    MemberInfo[] member = type.GetMember(this.MemberName);
                    memberInfo = ((member != null) ? member.FirstOrDefault<MemberInfo>() : null);
                }
            }
            MemberInfo memberInfo2 = memberInfo;
            FieldInfo fieldInfo = memberInfo2 as FieldInfo;
            string text;
            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = memberInfo2 as PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new NotSupportedException();
                }
                text = propertyInfo.GetValue(instance) as string;
            }
            else
            {
                text = fieldInfo.GetValue(instance) as string;
            }
            return Type.GetType(text, false);
        }
    }
}
