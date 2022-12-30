using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector4 : IEquatable<Vector4>
{
	public static readonly Vector4 zero = new Vector4(0f, 0f, 0f, 0f);

	public const float Epsilon = 9.99999944E-11f;

	public float x;

	public float y;

	public float z;

	public float w;

	public float Magnitude => (float)System.Math.Sqrt(x * x + y * y + z * z + w * w);

	public float MagnitudeSqr => x * x + y * y + z * z + w * w;

	public Vector4 Normalized
	{
		get
		{
			Normalize(ref this, out var result);
			return result;
		}
	}

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException("Invalid Vector4 index!"), 
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
				throw new IndexOutOfRangeException("Invalid Vector4 index!");
			}
		}
	}

	public Vector4(float value)
	{
		x = value;
		y = value;
		z = value;
		w = value;
	}

	public Vector4(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public static Vector4 operator +(Vector4 a, Vector4 b)
	{
		return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
	}

	public static Vector4 operator -(Vector4 a, Vector4 b)
	{
		return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
	}

	public static Vector4 operator *(Vector4 a, float d)
	{
		return new Vector4(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4 operator *(float d, Vector4 a)
	{
		return new Vector4(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4 operator /(Vector4 a, float d)
	{
		return new Vector4(a.x / d, a.y / d, a.z / d, a.w / d);
	}

	public static Vector4 operator -(Vector4 a)
	{
		return new Vector4(0f - a.x, 0f - a.y, 0f - a.z, 0f - a.w);
	}

	public static bool operator ==(Vector4 lhs, Vector4 rhs)
	{
		return (lhs - rhs).MagnitudeSqr < 9.99999944E-11f;
	}

	public static bool operator !=(Vector4 lhs, Vector4 rhs)
	{
		return (lhs - rhs).MagnitudeSqr >= 9.99999944E-11f;
	}

	public static explicit operator Vector4(Vector4d v)
	{
		return new Vector4((float)v.x, (float)v.y, (float)v.z, (float)v.w);
	}

	public static implicit operator Vector4(Vector4i v)
	{
		return new Vector4(v.x, v.y, v.z, v.w);
	}

	public void Set(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public void Scale(ref Vector4 scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
		w *= scale.w;
	}

	public void Normalize()
	{
		float magnitude = Magnitude;
		if (magnitude > 9.99999944E-11f)
		{
			x /= magnitude;
			y /= magnitude;
			z /= magnitude;
			w /= magnitude;
		}
		else
		{
			x = (y = (z = (w = 0f)));
		}
	}

	public void Clamp(float min, float max)
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
		if (!(other is Vector4 vector))
		{
			return false;
		}
		if (x == vector.x && y == vector.y && z == vector.z)
		{
			return w == vector.w;
		}
		return false;
	}

	public bool Equals(Vector4 other)
	{
		if (x == other.x && y == other.y && z == other.z)
		{
			return w == other.w;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("({0}, {1}, {2}, {3})", x.ToString("F1", CultureInfo.InvariantCulture), y.ToString("F1", CultureInfo.InvariantCulture), z.ToString("F1", CultureInfo.InvariantCulture), w.ToString("F1", CultureInfo.InvariantCulture));
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format, CultureInfo.InvariantCulture)}, {y.ToString(format, CultureInfo.InvariantCulture)}, {z.ToString(format, CultureInfo.InvariantCulture)}, {w.ToString(format, CultureInfo.InvariantCulture)})";
	}

	public static float Dot(ref Vector4 lhs, ref Vector4 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;
	}

	public static void Lerp(ref Vector4 a, ref Vector4 b, float t, out Vector4 result)
	{
		result = new Vector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
	}

	public static void Scale(ref Vector4 a, ref Vector4 b, out Vector4 result)
	{
		result = new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
	}

	public static void Normalize(ref Vector4 value, out Vector4 result)
	{
		float magnitude = value.Magnitude;
		if (magnitude > 9.99999944E-11f)
		{
			result = new Vector4(value.x / magnitude, value.y / magnitude, value.z / magnitude, value.w / magnitude);
		}
		else
		{
			result = zero;
		}
	}
}
