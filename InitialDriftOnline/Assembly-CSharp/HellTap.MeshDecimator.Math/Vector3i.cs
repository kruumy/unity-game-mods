using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector3i : IEquatable<Vector3i>
{
	public static readonly Vector3i zero = new Vector3i(0, 0, 0);

	public int x;

	public int y;

	public int z;

	public int Magnitude => (int)System.Math.Sqrt(x * x + y * y + z * z);

	public int MagnitudeSqr => x * x + y * y + z * z;

	public int this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				_ => throw new IndexOutOfRangeException("Invalid Vector3i index!"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				x = value;
				break;
			case 1:
				y = value;
				break;
			case 2:
				z = value;
				break;
			default:
				throw new IndexOutOfRangeException("Invalid Vector3i index!");
			}
		}
	}

	public Vector3i(int value)
	{
		x = value;
		y = value;
		z = value;
	}

	public Vector3i(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static Vector3i operator +(Vector3i a, Vector3i b)
	{
		return new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3i operator -(Vector3i a, Vector3i b)
	{
		return new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3i operator *(Vector3i a, int d)
	{
		return new Vector3i(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3i operator *(int d, Vector3i a)
	{
		return new Vector3i(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3i operator /(Vector3i a, int d)
	{
		return new Vector3i(a.x / d, a.y / d, a.z / d);
	}

	public static Vector3i operator -(Vector3i a)
	{
		return new Vector3i(-a.x, -a.y, -a.z);
	}

	public static bool operator ==(Vector3i lhs, Vector3i rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y)
		{
			return lhs.z == rhs.z;
		}
		return false;
	}

	public static bool operator !=(Vector3i lhs, Vector3i rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y)
		{
			return lhs.z != rhs.z;
		}
		return true;
	}

	public static implicit operator Vector3i(Vector3 v)
	{
		return new Vector3i((int)v.x, (int)v.y, (int)v.z);
	}

	public static explicit operator Vector3i(Vector3d v)
	{
		return new Vector3i((int)v.x, (int)v.y, (int)v.z);
	}

	public void Set(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void Scale(ref Vector3i scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
	}

	public void Clamp(int min, int max)
	{
		if (x < min)
		{
			x = min;
		}
		else if (x > max)
		{
			x = max;
		}
		if (y < min)
		{
			y = min;
		}
		else if (y > max)
		{
			y = max;
		}
		if (z < min)
		{
			z = min;
		}
		else if (z > max)
		{
			z = max;
		}
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector3i vector3i))
		{
			return false;
		}
		if (x == vector3i.x && y == vector3i.y)
		{
			return z == vector3i.z;
		}
		return false;
	}

	public bool Equals(Vector3i other)
	{
		if (x == other.x && y == other.y)
		{
			return z == other.z;
		}
		return false;
	}

	public override string ToString()
	{
		return $"({x.ToString(CultureInfo.InvariantCulture)}, {y.ToString(CultureInfo.InvariantCulture)}, {z.ToString(CultureInfo.InvariantCulture)})";
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format, CultureInfo.InvariantCulture)}, {y.ToString(format, CultureInfo.InvariantCulture)}, {z.ToString(format, CultureInfo.InvariantCulture)})";
	}

	public static void Scale(ref Vector3i a, ref Vector3i b, out Vector3i result)
	{
		result = new Vector3i(a.x * b.x, a.y * b.y, a.z * b.z);
	}
}
