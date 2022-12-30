using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector3 : IEquatable<Vector3>
{
	public static readonly Vector3 zero = new Vector3(0f, 0f, 0f);

	public const float Epsilon = 9.99999944E-11f;

	public float x;

	public float y;

	public float z;

	public float Magnitude => (float)System.Math.Sqrt(x * x + y * y + z * z);

	public float MagnitudeSqr => x * x + y * y + z * z;

	public Vector3 Normalized
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
				_ => throw new IndexOutOfRangeException("Invalid Vector3 index!"), 
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
				throw new IndexOutOfRangeException("Invalid Vector3 index!");
			}
		}
	}

	public Vector3(float value)
	{
		x = value;
		y = value;
		z = value;
	}

	public Vector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3(Vector3d vector)
	{
		x = (float)vector.x;
		y = (float)vector.y;
		z = (float)vector.z;
	}

	public static Vector3 operator +(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator -(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3 operator *(Vector3 a, float d)
	{
		return new Vector3(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3 operator *(float d, Vector3 a)
	{
		return new Vector3(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3 operator /(Vector3 a, float d)
	{
		return new Vector3(a.x / d, a.y / d, a.z / d);
	}

	public static Vector3 operator -(Vector3 a)
	{
		return new Vector3(0f - a.x, 0f - a.y, 0f - a.z);
	}

	public static bool operator ==(Vector3 lhs, Vector3 rhs)
	{
		return (lhs - rhs).MagnitudeSqr < 9.99999944E-11f;
	}

	public static bool operator !=(Vector3 lhs, Vector3 rhs)
	{
		return (lhs - rhs).MagnitudeSqr >= 9.99999944E-11f;
	}

	public static explicit operator Vector3(Vector3d v)
	{
		return new Vector3((float)v.x, (float)v.y, (float)v.z);
	}

	public static implicit operator Vector3(Vector3i v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	public void Set(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void Scale(ref Vector3 scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
	}

	public void Normalize()
	{
		float magnitude = Magnitude;
		if (magnitude > 9.99999944E-11f)
		{
			x /= magnitude;
			y /= magnitude;
			z /= magnitude;
		}
		else
		{
			x = (y = (z = 0f));
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
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector3 vector))
		{
			return false;
		}
		if (x == vector.x && y == vector.y)
		{
			return z == vector.z;
		}
		return false;
	}

	public bool Equals(Vector3 other)
	{
		if (x == other.x && y == other.y)
		{
			return z == other.z;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("({0}, {1}, {2})", x.ToString("F1", CultureInfo.InvariantCulture), y.ToString("F1", CultureInfo.InvariantCulture), z.ToString("F1", CultureInfo.InvariantCulture));
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format, CultureInfo.InvariantCulture)}, {y.ToString(format, CultureInfo.InvariantCulture)}, {z.ToString(format, CultureInfo.InvariantCulture)})";
	}

	public static float Dot(ref Vector3 lhs, ref Vector3 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static void Cross(ref Vector3 lhs, ref Vector3 rhs, out Vector3 result)
	{
		result = new Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
	}

	public static float Angle(ref Vector3 from, ref Vector3 to)
	{
		Vector3 lhs = from.Normalized;
		Vector3 rhs = to.Normalized;
		return (float)System.Math.Acos(MathHelper.Clamp(Dot(ref lhs, ref rhs), -1f, 1f)) * (180f / (float)System.Math.PI);
	}

	public static void Lerp(ref Vector3 a, ref Vector3 b, float t, out Vector3 result)
	{
		result = new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
	}

	public static void Scale(ref Vector3 a, ref Vector3 b, out Vector3 result)
	{
		result = new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static void Normalize(ref Vector3 value, out Vector3 result)
	{
		float magnitude = value.Magnitude;
		if (magnitude > 9.99999944E-11f)
		{
			result = new Vector3(value.x / magnitude, value.y / magnitude, value.z / magnitude);
		}
		else
		{
			result = zero;
		}
	}

	public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent)
	{
		normal.Normalize();
		Vector3 vector = normal * Dot(ref tangent, ref normal);
		tangent -= vector;
		tangent.Normalize();
	}
}
