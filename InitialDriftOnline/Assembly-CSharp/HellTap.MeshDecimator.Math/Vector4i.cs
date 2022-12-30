using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector4i : IEquatable<Vector4i>
{
	public static readonly Vector4i zero = new Vector4i(0, 0, 0, 0);

	public int x;

	public int y;

	public int z;

	public int w;

	public int Magnitude => (int)System.Math.Sqrt(x * x + y * y + z * z + w * w);

	public int MagnitudeSqr => x * x + y * y + z * z + w * w;

	public int this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException("Invalid Vector4i index!"), 
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
			case 3:
				w = value;
				break;
			default:
				throw new IndexOutOfRangeException("Invalid Vector4i index!");
			}
		}
	}

	public Vector4i(int value)
	{
		x = value;
		y = value;
		z = value;
		w = value;
	}

	public Vector4i(int x, int y, int z, int w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public static Vector4i operator +(Vector4i a, Vector4i b)
	{
		return new Vector4i(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
	}

	public static Vector4i operator -(Vector4i a, Vector4i b)
	{
		return new Vector4i(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
	}

	public static Vector4i operator *(Vector4i a, int d)
	{
		return new Vector4i(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4i operator *(int d, Vector4i a)
	{
		return new Vector4i(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4i operator /(Vector4i a, int d)
	{
		return new Vector4i(a.x / d, a.y / d, a.z / d, a.w / d);
	}

	public static Vector4i operator -(Vector4i a)
	{
		return new Vector4i(-a.x, -a.y, -a.z, -a.w);
	}

	public static bool operator ==(Vector4i lhs, Vector4i rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z)
		{
			return lhs.w == rhs.w;
		}
		return false;
	}

	public static bool operator !=(Vector4i lhs, Vector4i rhs)
	{
		if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z)
		{
			return lhs.w != rhs.w;
		}
		return true;
	}

	public static explicit operator Vector4i(Vector4 v)
	{
		return new Vector4i((int)v.x, (int)v.y, (int)v.z, (int)v.w);
	}

	public static explicit operator Vector4i(Vector4d v)
	{
		return new Vector4i((int)v.x, (int)v.y, (int)v.z, (int)v.w);
	}

	public void Set(int x, int y, int z, int w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public void Scale(ref Vector4i scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
		w *= scale.w;
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
		if (w < min)
		{
			w = min;
		}
		else if (w > max)
		{
			w = max;
		}
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector4i vector4i))
		{
			return false;
		}
		if (x == vector4i.x && y == vector4i.y && z == vector4i.z)
		{
			return w == vector4i.w;
		}
		return false;
	}

	public bool Equals(Vector4i other)
	{
		if (x == other.x && y == other.y && z == other.z)
		{
			return w == other.w;
		}
		return false;
	}

	public override string ToString()
	{
		return $"({x.ToString(CultureInfo.InvariantCulture)}, {y.ToString(CultureInfo.InvariantCulture)}, {z.ToString(CultureInfo.InvariantCulture)}, {w.ToString(CultureInfo.InvariantCulture)})";
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format, CultureInfo.InvariantCulture)}, {y.ToString(format, CultureInfo.InvariantCulture)}, {z.ToString(format, CultureInfo.InvariantCulture)}, {w.ToString(format, CultureInfo.InvariantCulture)})";
	}

	public static void Scale(ref Vector4i a, ref Vector4i b, out Vector4i result)
	{
		result = new Vector4i(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
	}
}
