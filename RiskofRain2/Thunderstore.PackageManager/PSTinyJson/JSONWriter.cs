using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

// from propersave

namespace Thunderstore.PackageManager.PSTinyJson
{
    // Token: 0x02000003 RID: 3
    public static class JSONWriter
    {
        // Token: 0x06000009 RID: 9 RVA: 0x00002AB2 File Offset: 0x00000CB2
        public static string ToJson(this object item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            JSONWriter.AppendValue(stringBuilder, item);
            return stringBuilder.ToString();
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002AC8 File Offset: 0x00000CC8
        private static void AppendValue(StringBuilder stringBuilder, object item)
        {
            if (item == null)
            {
                stringBuilder.Append("null");
                return;
            }
            Type type = item.GetType();
            if (type == typeof(string))
            {
                stringBuilder.Append('"');
                string text = (string)item;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] < ' ' || text[i] == '"' || text[i] == '\\')
                    {
                        stringBuilder.Append('\\');
                        int num = "\"\\\n\r\t\b\f".IndexOf(text[i]);
                        if (num >= 0)
                        {
                            stringBuilder.Append("\"\\nrtbf"[num]);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("u{0:X4}", (uint)text[i]);
                        }
                    }
                    else
                    {
                        stringBuilder.Append(text[i]);
                    }
                }
                stringBuilder.Append('"');
                return;
            }
            if (type == typeof(byte) || type == typeof(int) || type == typeof(long) || type == typeof(uint) || type == typeof(ulong))
            {
                stringBuilder.Append(item.ToString());
                return;
            }
            if (type == typeof(float))
            {
                stringBuilder.Append(((float)item).ToString(CultureInfo.InvariantCulture));
                return;
            }
            if (type == typeof(double))
            {
                stringBuilder.Append(((double)item).ToString(CultureInfo.InvariantCulture));
                return;
            }
            if (type == typeof(bool))
            {
                stringBuilder.Append(((bool)item) ? "true" : "false");
                return;
            }
            if (type.IsEnum)
            {
                stringBuilder.Append('"');
                stringBuilder.Append(item.ToString());
                stringBuilder.Append('"');
                return;
            }
            if (item is IList)
            {
                stringBuilder.Append('[');
                bool flag = true;
                IList list = item as IList;
                for (int j = 0; j < list.Count; j++)
                {
                    if (flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        stringBuilder.Append(',');
                    }
                    JSONWriter.AppendValue(stringBuilder, list[j]);
                }
                stringBuilder.Append(']');
                return;
            }
            if (!type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
            {
                stringBuilder.Append('{');
                bool flag2 = true;
                FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int k = 0; k < fields.Length; k++)
                {
                    if (!fields[k].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    {
                        object value = fields[k].GetValue(item);
                        if (value != null)
                        {
                            if (flag2)
                            {
                                flag2 = false;
                            }
                            else
                            {
                                stringBuilder.Append(',');
                            }
                            stringBuilder.Append('"');
                            stringBuilder.Append(JSONWriter.GetMemberName(fields[k]));
                            stringBuilder.Append("\":");
                            JSONWriter.AppendValue(stringBuilder, value);
                        }
                    }
                }
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                for (int l = 0; l < properties.Length; l++)
                {
                    if (properties[l].CanRead && !properties[l].IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    {
                        object value2 = properties[l].GetValue(item, null);
                        if (value2 != null)
                        {
                            if (flag2)
                            {
                                flag2 = false;
                            }
                            else
                            {
                                stringBuilder.Append(',');
                            }
                            stringBuilder.Append('"');
                            stringBuilder.Append(JSONWriter.GetMemberName(properties[l]));
                            stringBuilder.Append("\":");
                            JSONWriter.AppendValue(stringBuilder, value2);
                        }
                    }
                }
                stringBuilder.Append('}');
                return;
            }
            if (type.GetGenericArguments()[0] != typeof(string))
            {
                stringBuilder.Append("{}");
                return;
            }
            stringBuilder.Append('{');
            IDictionary dictionary = item as IDictionary;
            bool flag3 = true;
            foreach (object obj in dictionary.Keys)
            {
                if (flag3)
                {
                    flag3 = false;
                }
                else
                {
                    stringBuilder.Append(',');
                }
                stringBuilder.Append('"');
                stringBuilder.Append((string)obj);
                stringBuilder.Append("\":");
                JSONWriter.AppendValue(stringBuilder, dictionary[obj]);
            }
            stringBuilder.Append('}');
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002F4C File Offset: 0x0000114C
        private static string GetMemberName(MemberInfo member)
        {
            if (member.IsDefined(typeof(DataMemberAttribute), true))
            {
                DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                {
                    return dataMemberAttribute.Name;
                }
            }
            return member.Name;
        }
    }
}
