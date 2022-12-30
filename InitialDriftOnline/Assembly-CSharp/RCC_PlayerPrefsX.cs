using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_PlayerPrefsX
{
	private enum ArrayType
	{
		Float,
		Int32,
		Bool,
		String,
		Vector2,
		Vector3,
		Quaternion,
		Color
	}

	private static int endianDiff1;

	private static int endianDiff2;

	private static int idx;

	private static byte[] byteBlock;

	public static bool SetBool(string name, bool value)
	{
		try
		{
			PlayerPrefs.SetInt(name, value ? 1 : 0);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool GetBool(string name)
	{
		return PlayerPrefs.GetInt(name) == 1;
	}

	public static bool GetBool(string name, bool defaultValue)
	{
		return 1 == PlayerPrefs.GetInt(name, defaultValue ? 1 : 0);
	}

	public static long GetLong(string key, long defaultValue)
	{
		SplitLong(defaultValue, out var lowBits, out var highBits);
		lowBits = PlayerPrefs.GetInt(key + "_lowBits", lowBits);
		highBits = PlayerPrefs.GetInt(key + "_highBits", highBits);
		return (long)(((ulong)(uint)highBits << 32) | (uint)lowBits);
	}

	public static long GetLong(string key)
	{
		int @int = PlayerPrefs.GetInt(key + "_lowBits");
		return (long)(((ulong)(uint)PlayerPrefs.GetInt(key + "_highBits") << 32) | (uint)@int);
	}

	private static void SplitLong(long input, out int lowBits, out int highBits)
	{
		lowBits = (int)input;
		highBits = (int)(input >> 32);
	}

	public static void SetLong(string key, long value)
	{
		SplitLong(value, out var lowBits, out var highBits);
		PlayerPrefs.SetInt(key + "_lowBits", lowBits);
		PlayerPrefs.SetInt(key + "_highBits", highBits);
	}

	public static bool SetVector2(string key, Vector2 vector)
	{
		return SetFloatArray(key, new float[2] { vector.x, vector.y });
	}

	private static Vector2 GetVector2(string key)
	{
		float[] floatArray = GetFloatArray(key);
		if (floatArray.Length < 2)
		{
			return Vector2.zero;
		}
		return new Vector2(floatArray[0], floatArray[1]);
	}

	public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetVector2(key);
		}
		return defaultValue;
	}

	public static bool SetVector3(string key, Vector3 vector)
	{
		return SetFloatArray(key, new float[3] { vector.x, vector.y, vector.z });
	}

	public static Vector3 GetVector3(string key)
	{
		float[] floatArray = GetFloatArray(key);
		if (floatArray.Length < 3)
		{
			return Vector3.zero;
		}
		return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
	}

	public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetVector3(key);
		}
		return defaultValue;
	}

	public static bool SetQuaternion(string key, Quaternion vector)
	{
		return SetFloatArray(key, new float[4] { vector.x, vector.y, vector.z, vector.w });
	}

	public static Quaternion GetQuaternion(string key)
	{
		float[] floatArray = GetFloatArray(key);
		if (floatArray.Length < 4)
		{
			return Quaternion.identity;
		}
		return new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	}

	public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetQuaternion(key);
		}
		return defaultValue;
	}

	public static bool SetColor(string key, Color color)
	{
		return SetFloatArray(key, new float[4] { color.r, color.g, color.b, color.a });
	}

	public static Color GetColor(string key)
	{
		float[] floatArray = GetFloatArray(key);
		if (floatArray.Length < 4)
		{
			return new Color(0f, 0f, 0f, 0f);
		}
		return new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	}

	public static Color GetColor(string key, Color defaultValue)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetColor(key);
		}
		return defaultValue;
	}

	public static bool SetBoolArray(string key, bool[] boolArray)
	{
		byte[] array = new byte[(boolArray.Length + 7) / 8 + 5];
		array[0] = Convert.ToByte(ArrayType.Bool);
		new BitArray(boolArray).CopyTo(array, 5);
		Initialize();
		ConvertInt32ToBytes(boolArray.Length, array);
		return SaveBytes(key, array);
	}

	public static bool[] GetBoolArray(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
			if (array.Length < 5)
			{
				Debug.LogError("Corrupt preference file for " + key);
				return new bool[0];
			}
			if (array[0] != 2)
			{
				Debug.LogError(key + " is not a boolean array");
				return new bool[0];
			}
			Initialize();
			byte[] array2 = new byte[array.Length - 5];
			Array.Copy(array, 5, array2, 0, array2.Length);
			BitArray obj = new BitArray(array2)
			{
				Length = ConvertBytesToInt32(array)
			};
			bool[] array3 = new bool[obj.Count];
			obj.CopyTo(array3, 0);
			return array3;
		}
		return new bool[0];
	}

	public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetBoolArray(key);
		}
		bool[] array = new bool[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static bool SetStringArray(string key, string[] stringArray)
	{
		byte[] array = new byte[stringArray.Length + 1];
		array[0] = Convert.ToByte(ArrayType.String);
		Initialize();
		for (int i = 0; i < stringArray.Length; i++)
		{
			if (stringArray[i] == null)
			{
				Debug.LogError("Can't save null entries in the string array when setting " + key);
				return false;
			}
			if (stringArray[i].Length > 255)
			{
				Debug.LogError("Strings cannot be longer than 255 characters when setting " + key);
				return false;
			}
			array[idx++] = (byte)stringArray[i].Length;
		}
		try
		{
			PlayerPrefs.SetString(key, Convert.ToBase64String(array) + "|" + string.Join("", stringArray));
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static string[] GetStringArray(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			string @string = PlayerPrefs.GetString(key);
			int num = @string.IndexOf("|"[0]);
			if (num < 4)
			{
				Debug.LogError("Corrupt preference file for " + key);
				return new string[0];
			}
			byte[] array = Convert.FromBase64String(@string.Substring(0, num));
			if (array[0] != 3)
			{
				Debug.LogError(key + " is not a string array");
				return new string[0];
			}
			Initialize();
			int num2 = array.Length - 1;
			string[] array2 = new string[num2];
			int num3 = num + 1;
			for (int i = 0; i < num2; i++)
			{
				int num4 = array[idx++];
				if (num3 + num4 > @string.Length)
				{
					Debug.LogError("Corrupt preference file for " + key);
					return new string[0];
				}
				array2[i] = @string.Substring(num3, num4);
				num3 += num4;
			}
			return array2;
		}
		return new string[0];
	}

	public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetStringArray(key);
		}
		string[] array = new string[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static bool SetIntArray(string key, int[] intArray)
	{
		return SetValue(key, intArray, ArrayType.Int32, 1, ConvertFromInt);
	}

	public static bool SetFloatArray(string key, float[] floatArray)
	{
		return SetValue(key, floatArray, ArrayType.Float, 1, ConvertFromFloat);
	}

	public static bool SetVector2Array(string key, Vector2[] vector2Array)
	{
		return SetValue(key, vector2Array, ArrayType.Vector2, 2, ConvertFromVector2);
	}

	public static bool SetVector3Array(string key, Vector3[] vector3Array)
	{
		return SetValue(key, vector3Array, ArrayType.Vector3, 3, ConvertFromVector3);
	}

	public static bool SetQuaternionArray(string key, Quaternion[] quaternionArray)
	{
		return SetValue(key, quaternionArray, ArrayType.Quaternion, 4, ConvertFromQuaternion);
	}

	public static bool SetColorArray(string key, Color[] colorArray)
	{
		return SetValue(key, colorArray, ArrayType.Color, 4, ConvertFromColor);
	}

	private static bool SetValue<T>(string key, T array, ArrayType arrayType, int vectorNumber, Action<T, byte[], int> convert) where T : IList
	{
		byte[] array2 = new byte[4 * array.Count * vectorNumber + 1];
		array2[0] = Convert.ToByte(arrayType);
		Initialize();
		for (int i = 0; i < array.Count; i++)
		{
			convert(array, array2, i);
		}
		return SaveBytes(key, array2);
	}

	private static void ConvertFromInt(int[] array, byte[] bytes, int i)
	{
		ConvertInt32ToBytes(array[i], bytes);
	}

	private static void ConvertFromFloat(float[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i], bytes);
	}

	private static void ConvertFromVector2(Vector2[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].x, bytes);
		ConvertFloatToBytes(array[i].y, bytes);
	}

	private static void ConvertFromVector3(Vector3[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].x, bytes);
		ConvertFloatToBytes(array[i].y, bytes);
		ConvertFloatToBytes(array[i].z, bytes);
	}

	private static void ConvertFromQuaternion(Quaternion[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].x, bytes);
		ConvertFloatToBytes(array[i].y, bytes);
		ConvertFloatToBytes(array[i].z, bytes);
		ConvertFloatToBytes(array[i].w, bytes);
	}

	private static void ConvertFromColor(Color[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].r, bytes);
		ConvertFloatToBytes(array[i].g, bytes);
		ConvertFloatToBytes(array[i].b, bytes);
		ConvertFloatToBytes(array[i].a, bytes);
	}

	public static int[] GetIntArray(string key)
	{
		List<int> list = new List<int>();
		GetValue(key, list, ArrayType.Int32, 1, ConvertToInt);
		return list.ToArray();
	}

	public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetIntArray(key);
		}
		int[] array = new int[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static float[] GetFloatArray(string key)
	{
		List<float> list = new List<float>();
		GetValue(key, list, ArrayType.Float, 1, ConvertToFloat);
		return list.ToArray();
	}

	public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetFloatArray(key);
		}
		float[] array = new float[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static Vector2[] GetVector2Array(string key)
	{
		List<Vector2> list = new List<Vector2>();
		GetValue(key, list, ArrayType.Vector2, 2, ConvertToVector2);
		return list.ToArray();
	}

	public static Vector2[] GetVector2Array(string key, Vector2 defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetVector2Array(key);
		}
		Vector2[] array = new Vector2[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static Vector3[] GetVector3Array(string key)
	{
		List<Vector3> list = new List<Vector3>();
		GetValue(key, list, ArrayType.Vector3, 3, ConvertToVector3);
		return list.ToArray();
	}

	public static Vector3[] GetVector3Array(string key, Vector3 defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetVector3Array(key);
		}
		Vector3[] array = new Vector3[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static Quaternion[] GetQuaternionArray(string key)
	{
		List<Quaternion> list = new List<Quaternion>();
		GetValue(key, list, ArrayType.Quaternion, 4, ConvertToQuaternion);
		return list.ToArray();
	}

	public static Quaternion[] GetQuaternionArray(string key, Quaternion defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetQuaternionArray(key);
		}
		Quaternion[] array = new Quaternion[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	public static Color[] GetColorArray(string key)
	{
		List<Color> list = new List<Color>();
		GetValue(key, list, ArrayType.Color, 4, ConvertToColor);
		return list.ToArray();
	}

	public static Color[] GetColorArray(string key, Color defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return GetColorArray(key);
		}
		Color[] array = new Color[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	private static void GetValue<T>(string key, T list, ArrayType arrayType, int vectorNumber, Action<T, byte[]> convert) where T : IList
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return;
		}
		byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
		if ((array.Length - 1) % (vectorNumber * 4) != 0)
		{
			Debug.LogError("Corrupt preference file for " + key);
			return;
		}
		if ((ArrayType)array[0] != arrayType)
		{
			Debug.LogError(key + " is not a " + arrayType.ToString() + " array");
			return;
		}
		Initialize();
		int num = (array.Length - 1) / (vectorNumber * 4);
		for (int i = 0; i < num; i++)
		{
			convert(list, array);
		}
	}

	private static void ConvertToInt(List<int> list, byte[] bytes)
	{
		list.Add(ConvertBytesToInt32(bytes));
	}

	private static void ConvertToFloat(List<float> list, byte[] bytes)
	{
		list.Add(ConvertBytesToFloat(bytes));
	}

	private static void ConvertToVector2(List<Vector2> list, byte[] bytes)
	{
		list.Add(new Vector2(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToVector3(List<Vector3> list, byte[] bytes)
	{
		list.Add(new Vector3(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
	{
		list.Add(new Quaternion(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToColor(List<Color> list, byte[] bytes)
	{
		list.Add(new Color(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	public static void ShowArrayType(string key)
	{
		byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
		if (array.Length != 0)
		{
			ArrayType arrayType = (ArrayType)array[0];
			Debug.Log(key + " is a " + arrayType.ToString() + " array");
		}
	}

	private static void Initialize()
	{
		if (BitConverter.IsLittleEndian)
		{
			endianDiff1 = 0;
			endianDiff2 = 0;
		}
		else
		{
			endianDiff1 = 3;
			endianDiff2 = 1;
		}
		if (byteBlock == null)
		{
			byteBlock = new byte[4];
		}
		idx = 1;
	}

	private static bool SaveBytes(string key, byte[] bytes)
	{
		try
		{
			PlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
		}
		catch
		{
			return false;
		}
		return true;
	}

	private static void ConvertFloatToBytes(float f, byte[] bytes)
	{
		byteBlock = BitConverter.GetBytes(f);
		ConvertTo4Bytes(bytes);
	}

	private static float ConvertBytesToFloat(byte[] bytes)
	{
		ConvertFrom4Bytes(bytes);
		return BitConverter.ToSingle(byteBlock, 0);
	}

	private static void ConvertInt32ToBytes(int i, byte[] bytes)
	{
		byteBlock = BitConverter.GetBytes(i);
		ConvertTo4Bytes(bytes);
	}

	private static int ConvertBytesToInt32(byte[] bytes)
	{
		ConvertFrom4Bytes(bytes);
		return BitConverter.ToInt32(byteBlock, 0);
	}

	private static void ConvertTo4Bytes(byte[] bytes)
	{
		bytes[idx] = byteBlock[endianDiff1];
		bytes[idx + 1] = byteBlock[1 + endianDiff2];
		bytes[idx + 2] = byteBlock[2 - endianDiff2];
		bytes[idx + 3] = byteBlock[3 - endianDiff1];
		idx += 4;
	}

	private static void ConvertFrom4Bytes(byte[] bytes)
	{
		byteBlock[endianDiff1] = bytes[idx];
		byteBlock[1 + endianDiff2] = bytes[idx + 1];
		byteBlock[2 - endianDiff2] = bytes[idx + 2];
		byteBlock[3 - endianDiff1] = bytes[idx + 3];
		idx += 4;
	}
}
