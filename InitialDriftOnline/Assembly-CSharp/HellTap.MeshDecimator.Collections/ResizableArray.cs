using System;

namespace HellTap.MeshDecimator.Collections;

internal sealed class ResizableArray<T>
{
	private T[] items;

	private int length;

	private static T[] emptyArr = new T[0];

	public int Length => length;

	public T[] Data => items;

	public T this[int index]
	{
		get
		{
			return items[index];
		}
		set
		{
			items[index] = value;
		}
	}

	public ResizableArray(int capacity)
		: this(capacity, 0)
	{
	}

	public ResizableArray(int capacity, int length)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (length < 0 || length > capacity)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		if (capacity > 0)
		{
			items = new T[capacity];
		}
		else
		{
			items = emptyArr;
		}
		this.length = length;
	}

	private void IncreaseCapacity(int capacity)
	{
		T[] destinationArray = new T[capacity];
		Array.Copy(items, 0, destinationArray, 0, System.Math.Min(length, capacity));
		items = destinationArray;
	}

	public void Clear()
	{
		Array.Clear(items, 0, length);
		length = 0;
	}

	public void Resize(int length, bool trimExess = false)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		if (length > items.Length)
		{
			IncreaseCapacity(length);
		}
		else
		{
			_ = this.length;
		}
		this.length = length;
		if (trimExess)
		{
			TrimExcess();
		}
	}

	public void TrimExcess()
	{
		if (items.Length != length)
		{
			T[] destinationArray = new T[length];
			Array.Copy(items, 0, destinationArray, 0, length);
			items = destinationArray;
		}
	}

	public void Add(T item)
	{
		if (length >= items.Length)
		{
			IncreaseCapacity(items.Length << 1);
		}
		items[length++] = item;
	}
}
