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
    // Token: 0x02000002 RID: 2
    public static class JSONParser
    {
        // Token: 0x04000001 RID: 1
        [ThreadStatic]
        private static Stack<List<string>> splitArrayPool;

        // Token: 0x04000002 RID: 2
        [ThreadStatic]
        private static StringBuilder stringBuilder;

        // Token: 0x04000003 RID: 3
        [ThreadStatic]
        private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache;

        // Token: 0x04000004 RID: 4
        [ThreadStatic]
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache;

        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static T FromJson<T>(this string json)
        {
            return (T)((object)json.FromJson(typeof(T)));
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
        public static object FromJson(this string json, Type type)
        {
            if (JSONParser.propertyInfoCache == null)
            {
                JSONParser.propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            }
            if (JSONParser.fieldInfoCache == null)
            {
                JSONParser.fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
            }
            if (JSONParser.stringBuilder == null)
            {
                JSONParser.stringBuilder = new StringBuilder();
            }
            if (JSONParser.splitArrayPool == null)
            {
                JSONParser.splitArrayPool = new Stack<List<string>>();
            }
            JSONParser.stringBuilder.Length = 0;
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"')
                {
                    i = JSONParser.AppendUntilStringEnd(true, i, json);
                }
                else if (!char.IsWhiteSpace(c))
                {
                    JSONParser.stringBuilder.Append(c);
                }
            }
            return JSONParser.ParseValue(type, JSONParser.stringBuilder.ToString());
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002114 File Offset: 0x00000314
        private static int AppendUntilStringEnd(bool appendEscapeCharacter, int startIdx, string json)
        {
            JSONParser.stringBuilder.Append(json[startIdx]);
            for (int i = startIdx + 1; i < json.Length; i++)
            {
                if (json[i] == '\\')
                {
                    if (appendEscapeCharacter)
                    {
                        JSONParser.stringBuilder.Append(json[i]);
                    }
                    JSONParser.stringBuilder.Append(json[i + 1]);
                    i++;
                }
                else
                {
                    if (json[i] == '"')
                    {
                        JSONParser.stringBuilder.Append(json[i]);
                        return i;
                    }
                    JSONParser.stringBuilder.Append(json[i]);
                }
            }
            return json.Length - 1;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000021BC File Offset: 0x000003BC
        private static List<string> Split(string json)
        {
            List<string> list = ((JSONParser.splitArrayPool.Count > 0) ? JSONParser.splitArrayPool.Pop() : new List<string>());
            list.Clear();
            if (json.Length == 2)
            {
                return list;
            }
            int num = 0;
            JSONParser.stringBuilder.Length = 0;
            int i = 1;
            while (i < json.Length - 1)
            {
                char c = json[i];
                if (c > ':')
                {
                    if (c <= ']')
                    {
                        if (c != '[')
                        {
                            if (c != ']')
                            {
                                goto IL_B6;
                            }
                            goto IL_85;
                        }
                    }
                    else if (c != '{')
                    {
                        if (c != '}')
                        {
                            goto IL_B6;
                        }
                        goto IL_85;
                    }
                    num++;
                    goto IL_B6;
                IL_85:
                    num--;
                    goto IL_B6;
                }
                if (c != '"')
                {
                    if (c != ',' && c != ':')
                    {
                        goto IL_B6;
                    }
                    if (num != 0)
                    {
                        goto IL_B6;
                    }
                    list.Add(JSONParser.stringBuilder.ToString());
                    JSONParser.stringBuilder.Length = 0;
                }
                else
                {
                    i = JSONParser.AppendUntilStringEnd(true, i, json);
                }
            IL_C8:
                i++;
                continue;
            IL_B6:
                JSONParser.stringBuilder.Append(json[i]);
                goto IL_C8;
            }
            list.Add(JSONParser.stringBuilder.ToString());
            return list;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000022B4 File Offset: 0x000004B4
        internal static object ParseValue(Type type, string json)
        {
            if (type == typeof(string))
            {
                if (json.Length <= 2)
                {
                    return string.Empty;
                }
                StringBuilder stringBuilder = new StringBuilder(json.Length);
                int i = 1;
                while (i < json.Length - 1)
                {
                    if (json[i] != '\\' || i + 1 >= json.Length - 1)
                    {
                        goto IL_C6;
                    }
                    int num = "\"\\nrtbf/".IndexOf(json[i + 1]);
                    if (num >= 0)
                    {
                        stringBuilder.Append("\"\\\n\r\t\b\f/"[num]);
                        i++;
                    }
                    else
                    {
                        if (json[i + 1] != 'u' || i + 5 >= json.Length - 1)
                        {
                            goto IL_C6;
                        }
                        if (!uint.TryParse(json.Substring(i + 2, 4), NumberStyles.AllowHexSpecifier, null, out uint num2))
                        {
                            goto IL_C6;
                        }
                        stringBuilder.Append((char)num2);
                        i += 5;
                    }
                IL_D4:
                    i++;
                    continue;
                IL_C6:
                    stringBuilder.Append(json[i]);
                    goto IL_D4;
                }
                return stringBuilder.ToString();
            }
            else
            {
                if (type.IsPrimitive)
                {
                    return Convert.ChangeType(json, type, CultureInfo.InvariantCulture);
                }
                if (type == typeof(decimal))
                {
                    decimal.TryParse(json, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal num3);
                    return num3;
                }
                if (json == "null")
                {
                    return null;
                }
                if (type.IsEnum)
                {
                    if (json[0] == '"')
                    {
                        json = json.Substring(1, json.Length - 2);
                    }
                    try
                    {
                        return Enum.Parse(type, json, false);
                    }
                    catch
                    {
                        return 0;
                    }
                }
                if (type.IsArray)
                {
                    Type elementType = type.GetElementType();
                    if (json[0] != '[' || json[json.Length - 1] != ']')
                    {
                        return null;
                    }
                    List<string> list = JSONParser.Split(json);
                    Array array = Array.CreateInstance(elementType, list.Count);
                    for (int j = 0; j < list.Count; j++)
                    {
                        array.SetValue(JSONParser.ParseValue(elementType, list[j]), j);
                    }
                    JSONParser.splitArrayPool.Push(list);
                    return array;
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type type2 = type.GetGenericArguments()[0];
                    if (json[0] != '[' || json[json.Length - 1] != ']')
                    {
                        return null;
                    }
                    List<string> list2 = JSONParser.Split(json);
                    IList list3 = (IList)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { list2.Count });
                    for (int k = 0; k < list2.Count; k++)
                    {
                        list3.Add(JSONParser.ParseValue(type2, list2[k]));
                    }
                    JSONParser.splitArrayPool.Push(list2);
                    return list3;
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    Type[] genericArguments = type.GetGenericArguments();
                    Type type3 = genericArguments[0];
                    Type type4 = genericArguments[1];
                    if (type3 != typeof(string))
                    {
                        return null;
                    }
                    if (json[0] != '{' || json[json.Length - 1] != '}')
                    {
                        return null;
                    }
                    List<string> list4 = JSONParser.Split(json);
                    if (list4.Count % 2 != 0)
                    {
                        return null;
                    }
                    IDictionary dictionary = (IDictionary)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { list4.Count / 2 });
                    for (int l = 0; l < list4.Count; l += 2)
                    {
                        if (list4[l].Length > 2)
                        {
                            string text = list4[l].Substring(1, list4[l].Length - 2);
                            object obj = JSONParser.ParseValue(type4, list4[l + 1]);
                            dictionary.Add(text, obj);
                        }
                    }
                    return dictionary;
                }
                else
                {
                    if (type == typeof(object))
                    {
                        return JSONParser.ParseAnonymousValue(json);
                    }
                    if (json[0] == '{' && json[json.Length - 1] == '}')
                    {
                        return JSONParser.ParseObject(type, json);
                    }
                    return null;
                }
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002708 File Offset: 0x00000908
        private static object ParseAnonymousValue(string json)
        {
            if (json.Length == 0)
            {
                return null;
            }
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                List<string> list = JSONParser.Split(json);
                if (list.Count % 2 != 0)
                {
                    return null;
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>(list.Count / 2);
                for (int i = 0; i < list.Count; i += 2)
                {
                    dictionary.Add(list[i].Substring(1, list[i].Length - 2), JSONParser.ParseAnonymousValue(list[i + 1]));
                }
                return dictionary;
            }
            else
            {
                if (json[0] == '[' && json[json.Length - 1] == ']')
                {
                    List<string> list2 = JSONParser.Split(json);
                    List<object> list3 = new List<object>(list2.Count);
                    for (int j = 0; j < list2.Count; j++)
                    {
                        list3.Add(JSONParser.ParseAnonymousValue(list2[j]));
                    }
                    return list3;
                }
                if (json[0] == '"' && json[json.Length - 1] == '"')
                {
                    return json.Substring(1, json.Length - 2).Replace("\\", string.Empty);
                }
                if (char.IsDigit(json[0]) || json[0] == '-')
                {
                    if (json.Contains("."))
                    {
                        double.TryParse(json, NumberStyles.Float, CultureInfo.InvariantCulture, out double num);
                        return num;
                    }
                    int.TryParse(json, out int num2);
                    return num2;
                }
                else
                {
                    if (json == "true")
                    {
                        return true;
                    }
                    if (json == "false")
                    {
                        return false;
                    }
                    return null;
                }
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000028B4 File Offset: 0x00000AB4
        private static Dictionary<string, T> CreateMemberNameDictionary<T>(T[] members) where T : MemberInfo
        {
            Dictionary<string, T> dictionary = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            foreach (T t in members)
            {
                if (!t.IsDefined(typeof(IgnoreDataMemberAttribute), true))
                {
                    string text = t.Name;
                    if (t.IsDefined(typeof(DataMemberAttribute), true))
                    {
                        DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(t, typeof(DataMemberAttribute), true);
                        if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
                        {
                            text = dataMemberAttribute.Name;
                        }
                    }
                    dictionary.Add(text, t);
                }
            }
            return dictionary;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002960 File Offset: 0x00000B60
        private static object ParseObject(Type type, string json)
        {
            object uninitializedObject = FormatterServices.GetUninitializedObject(type);
            List<string> list = JSONParser.Split(json);
            if (list.Count % 2 != 0)
            {
                return uninitializedObject;
            }
            if (!JSONParser.fieldInfoCache.TryGetValue(type, out Dictionary<string, FieldInfo> dictionary))
            {
                dictionary = JSONParser.CreateMemberNameDictionary<FieldInfo>(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                JSONParser.fieldInfoCache.Add(type, dictionary);
            }
            if (!JSONParser.propertyInfoCache.TryGetValue(type, out Dictionary<string, PropertyInfo> dictionary2))
            {
                dictionary2 = JSONParser.CreateMemberNameDictionary<PropertyInfo>(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                JSONParser.propertyInfoCache.Add(type, dictionary2);
            }
            for (int i = 0; i < list.Count; i += 2)
            {
                if (list[i].Length > 2)
                {
                    string text = list[i].Substring(1, list[i].Length - 2);
                    string text2 = list[i + 1];
                    if (dictionary.TryGetValue(text, out FieldInfo fieldInfo))
                    {
                        DiscoverObjectTypeAttribute customAttribute = fieldInfo.GetCustomAttribute<DiscoverObjectTypeAttribute>();
                        fieldInfo.SetValue(uninitializedObject, JSONParser.ParseValue(((customAttribute != null) ? customAttribute.GetObjectType(uninitializedObject) : null) ?? fieldInfo.FieldType, text2));
                    }
                    else if (dictionary2.TryGetValue(text, out PropertyInfo propertyInfo))
                    {
                        DiscoverObjectTypeAttribute customAttribute2 = propertyInfo.GetCustomAttribute<DiscoverObjectTypeAttribute>();
                        propertyInfo.SetValue(uninitializedObject, JSONParser.ParseValue(((customAttribute2 != null) ? customAttribute2.GetObjectType(uninitializedObject) : null) ?? propertyInfo.PropertyType, text2), null);
                    }
                }
            }
            return uninitializedObject;
        }
    }
}
