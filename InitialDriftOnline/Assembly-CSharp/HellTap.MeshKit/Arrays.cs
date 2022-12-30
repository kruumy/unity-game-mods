using System;
using System.Collections;
using UnityEngine;

namespace HellTap.MeshKit;

public static class Arrays
{
	public static bool AddItem<T>(ref T[] _arr, T item)
	{
		if (_arr != null)
		{
			Array.Resize(ref _arr, _arr.Length + 1);
			_arr[_arr.Length - 1] = item;
			return true;
		}
		Debug.LogWarning("ARRAYS - AddItem(): Array cannot be null. Skipping.");
		return false;
	}

	public static void AddItemFastest<T>(ref T[] _arr, T item)
	{
		Array.Resize(ref _arr, _arr.Length + 1);
		_arr[_arr.Length - 1] = item;
	}

	public static bool AddItemIfNotPresent<T>(ref T[] _arr, T item)
	{
		if (_arr != null)
		{
			if (ItemExistsAtIndex(ref _arr, ref item) == -1)
			{
				Array.Resize(ref _arr, _arr.Length + 1);
				_arr[_arr.Length - 1] = item;
				return true;
			}
		}
		else if (_arr == null)
		{
			Debug.LogWarning("ARRAYS - AddItemIfNotPresent(): Array cannot be null. Skipping.");
		}
		return false;
	}

	public static bool RemoveItem<T>(ref T[] _arr, ref T item, bool onlyRemoveFirstInstance = false)
	{
		if (_arr != null)
		{
			bool flag = false;
			while (ItemExistsAtIndex(ref _arr, ref item) != -1)
			{
				int num = ItemExistsAtIndex(ref _arr, ref item);
				T[] array = new T[_arr.Length - 1];
				for (int i = 0; i < array.Length; i++)
				{
					if (i < num)
					{
						array[i] = _arr[i];
					}
					else if (_arr.Length > 1)
					{
						array[i] = _arr[i + 1];
					}
				}
				_arr = array;
				flag = true;
				if (onlyRemoveFirstInstance)
				{
					return true;
				}
			}
			if (flag)
			{
				return true;
			}
		}
		else
		{
			Debug.LogWarning("ARRAYS - RemoveItem(): Array cannot be null. Skipping.");
		}
		return false;
	}

	public static bool RemoveFirstItem<T>(ref T[] _arr)
	{
		T[] array = new T[_arr.Length - 1];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _arr[i + 1];
		}
		_arr = array;
		return true;
	}

	public static bool RemoveItemAtIndex<T>(ref T[] _arr, int index)
	{
		if (_arr != null)
		{
			if (index >= 0 && index < _arr.Length)
			{
				T[] array = new T[_arr.Length - 1];
				for (int i = 0; i < array.Length; i++)
				{
					if (i < index)
					{
						array[i] = _arr[i];
					}
					else if (_arr.Length > 1)
					{
						array[i] = _arr[i + 1];
					}
				}
				_arr = array;
				return true;
			}
			Debug.LogWarning("ARRAYS - RemoveItemAtIndex(): Index is out of range. Skipping.");
		}
		else if (_arr == null)
		{
			Debug.LogWarning("ARRAYS - RemoveItemAtIndex(): Array cannot be null. Skipping.");
		}
		return false;
	}

	public static int ItemExistsAtIndex<T>(ref T[] _arr, ref T item)
	{
		if (_arr != null)
		{
			for (int i = 0; i < _arr.Length; i++)
			{
				if (_arr[i].Equals(item))
				{
					return i;
				}
			}
		}
		else
		{
			Debug.LogWarning("ARRAYS - ItemExistsAtIndex(): Array cannot be null. Returning -1.");
		}
		return -1;
	}

	public static bool ItemExists<T>(T[] _arr, T item)
	{
		if (_arr != null)
		{
			for (int i = 0; i < _arr.Length; i++)
			{
				if (_arr[i].Equals(item))
				{
					return true;
				}
			}
		}
		else
		{
			Debug.LogWarning("ARRAYS - ItemExistsAtIndex(): Array cannot be null. Returning false.");
		}
		return false;
	}

	public static T[] Concat<T>(this T[] a, T[] b)
	{
		if (a == null)
		{
			throw new ArgumentNullException("x");
		}
		if (b == null)
		{
			throw new ArgumentNullException("b");
		}
		int destinationIndex = a.Length;
		Array.Resize(ref a, a.Length + b.Length);
		Array.Copy(b, 0, a, destinationIndex, b.Length);
		return a;
	}

	public static T[] Combine<T>(T[] a, T[] b)
	{
		T[] array = new T[a.Length + b.Length];
		a.CopyTo(array, 0);
		b.CopyTo(array, a.Length);
		return array;
	}

	public static bool Clear<T>(ref T[] arr)
	{
		if (arr != null)
		{
			arr = new T[0];
			return true;
		}
		Debug.LogWarning("ARRAYS - Cannot clear an array that is null. Returning null.");
		return false;
	}

	public static bool Shift<T>(ref T[] _arr, int id, bool moveUp)
	{
		if (_arr != null && id < _arr.Length && ((moveUp && id > 0) || (!moveUp && id < _arr.Length - 1)))
		{
			T val = _arr[id];
			ArrayList arrayList = new ArrayList(_arr);
			arrayList.RemoveAt(id);
			ArrayList arrayList2 = new ArrayList();
			arrayList2.Clear();
			for (int i = 0; i < arrayList.Count; i++)
			{
				if (i == id - 1 && moveUp)
				{
					arrayList2.Add(val);
				}
				arrayList2.Add(arrayList[i]);
				if (i == id && !moveUp)
				{
					arrayList2.Add(val);
				}
			}
			T[] array = (_arr = arrayList2.ToArray(typeof(T)) as T[]);
			return true;
		}
		return false;
	}
}
