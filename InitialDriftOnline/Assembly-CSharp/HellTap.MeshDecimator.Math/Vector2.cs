using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector2 : IEquatable<Vector2>
{
	public static readonly Vector2 zero = new Vector2(0f, 0f);

	public const float Epsilon = 9.99999944E-11f;

	public float x;

	public float y;

	public float Magnitude => (float)System.Math.Sqrt(x * x + y * y);

	public float MagnitudeSqr => x * x + y * y;

	public Vector2 Normalized
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
				_ => throw new IndexOutOfRangeException("Invalid Vector2 index!"), 
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
			default:
				throw new IndexOutOfRangeException("Invalid Vector2 index!");
			}
		}
	}

	public Vector2(float value)
	{
		x = value;
		y = value;
	}

	public Vector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public static Vector2 operator +(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x + b.x, a.y + b.y);
	}

	public static Vector2 operator -(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x - b.x, a.y - b.y);
	}

	public static Vector2 operator *(Vector2 a, float d)
	{
		return new Vector2(a.x * d, a.y * d);
	}

	public static Vector2 operator *(float d, Vector2 a)
	{
		return new Vector2(a.x * d, a.y * d);
	}

	public static Vector2 operator /(Vector2 a, float d)
	{
		return new Vector2(a.x / d, a.y / d);
	}

	public static Vector2 operator -(Vector2 a)
	{
		return new Vector2(0f - a.x, 0f - a.y);
	}

	public static bool operator ==(Vector2 lhs, Vector2 rhs)
	{
		return (lhs - rhs).MagnitudeSqr < 9.99999944E-11f;
	}

	public static bool operator !=(Vector2 lhs, Vector2 rhs)
	{
		return (lhs - rhs).MagnitudeSqr >= 9.99999944E-11f;
	}

	public static explicit operator Vector2(Vector2d v)
	{
		return new Vector2((float)v.x, (float)v.y);
	}

	public static implicit operator Vector2(Vector2i v)
	{
		return new Vector2(v.x, v.y);
	}

	public void Set(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public void Scale(ref Vector2 scale)
	{
		x *= scale.x;
		y *= scale.y;
	}

	public void Normalize()
	{
		float magnitude = Magnitude;
		if (magnitude > 9.99999944E-11f)
		{
			x /= magnitude;
			y /= magnitude;
		}
		else
		{
			x = (y = 0f);
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
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector2 vector))
		{
			return false;
		}
		if (x == vector.x)
		{
			return y == vector.y;
		}
		return false;
	}

	public bool Equals(Vector2 other)
	{
		if (x == other.x)
		{
			return y == other.y;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("({0}, {1})", x.ToString("F1", CultureInfo.InvariantCulture), y.ToString("F1", CultureInfo.InvariantCulture));
	}

	public string ToString(string format)
	{
		return $"({x.ToString(format, CultureInfo.InvariantCulture)}, {y.ToString(format, CultureInfo.InvariantCulture)})";
	}

	public static float Dot(ref Vector2 lhs, ref Vector2 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	public static void Lerp(ref Vector2 a, ref Vector2 b, float t, out Vector2 result)
	{
		result = new Vector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
	}

	public static void Scale(ref Vector2 a, ref Vector2 b, out Vector2 result)
	{
		result = new Vector2(a.x * b.x, a.y * b.y);
	}

	public static void Normalize(ref Vector2 value, out Vector2 result)
	{
		float magnitude = value.Magnitude;
		if (magnitude > 9.99999944E-11f)
		{
			result = new Vector2(value.x / magnitude, value.y / magnitude);
		}
		else
		{
			result = zero;
		}
	}
}
