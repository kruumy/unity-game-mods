using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector2d : IEquatable<Vector2d>
{
	public static readonly Vector2d zero = new Vector2d(0.0, 0.0);

	public const double Epsilon = double.Epsilon;

	public double x;

	public double y;

	public double Magnitude => System.Math.Sqrt(x * x + y * y);

	public double MagnitudeSqr => x * x + y * y;

	public Vector2d Normalized
	{
		get
		{
			Normalize(ref this, out var result);
			return result;
		}
	}

	public double this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => y, 
				_ => throw new IndexOutOfRangeException("Invalid Vector2d index!"), 
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
				throw new IndexOutOfRangeException("Invalid Vector2d index!");
			}
		}
	}

	public Vector2d(double value)
	{
		x = value;
		y = value;
	}

	public Vector2d(double x, double y)
	{
		this.x = x;
		this.y = y;
	}

	public static Vector2d operator +(Vector2d a, Vector2d b)
	{
		return new Vector2d(a.x + b.x, a.y + b.y);
	}

	public static Vector2d operator -(Vector2d a, Vector2d b)
	{
		return new Vector2d(a.x - b.x, a.y - b.y);
	}

	public static Vector2d operator *(Vector2d a, double d)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	public static Vector2d operator *(double d, Vector2d a)
	{
		return new Vector2d(a.x * d, a.y * d);
	}

	public static Vector2d operator /(Vector2d a, double d)
	{
		return new Vector2d(a.x / d, a.y / d);
	}

	public static Vector2d operator -(Vector2d a)
	{
		return new Vector2d(0.0 - a.x, 0.0 - a.y);
	}

	public static bool operator ==(Vector2d lhs, Vector2d rhs)
	{
		return (lhs - rhs).MagnitudeSqr < double.Epsilon;
	}

	public static bool operator !=(Vector2d lhs, Vector2d rhs)
	{
		return (lhs - rhs).MagnitudeSqr >= double.Epsilon;
	}

	public static implicit operator Vector2d(Vector2 v)
	{
		return new Vector2d(v.x, v.y);
	}

	public static implicit operator Vector2d(Vector2i v)
	{
		return new Vector2d(v.x, v.y);
	}

	public void Set(double x, double y)
	{
		this.x = x;
		this.y = y;
	}

	public void Scale(ref Vector2d scale)
	{
		x *= scale.x;
		y *= scale.y;
	}

	public void Normalize()
	{
		double magnitude = Magnitude;
		if (magnitude > double.Epsilon)
		{
			x /= magnitude;
			y /= magnitude;
		}
		else
		{
			x = (y = 0.0);
		}
	}

	public void Clamp(double min, double max)
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
		if (!(other is Vector2d vector2d))
		{
			return false;
		}
		if (x == vector2d.x)
		{
			return y == vector2d.y;
		}
		return false;
	}

	public bool Equals(Vector2d other)
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

	public static double Dot(ref Vector2d lhs, ref Vector2d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	public static void Lerp(ref Vector2d a, ref Vector2d b, double t, out Vector2d result)
	{
		result = new Vector2d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
	}

	public static void Scale(ref Vector2d a, ref Vector2d b, out Vector2d result)
	{
		result = new Vector2d(a.x * b.x, a.y * b.y);
	}

	public static void Normalize(ref Vector2d value, out Vector2d result)
	{
		double magnitude = value.Magnitude;
		if (magnitude > double.Epsilon)
		{
			result = new Vector2d(value.x / magnitude, value.y / magnitude);
		}
		else
		{
			result = zero;
		}
	}
}
