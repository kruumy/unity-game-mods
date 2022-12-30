using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector4d : IEquatable<Vector4d>
{
	public static readonly Vector4d zero = new Vector4d(0.0, 0.0, 0.0, 0.0);

	public const double Epsilon = double.Epsilon;

	public double x;

	public double y;

	public double z;

	public double w;

	public double Magnitude => System.Math.Sqrt(x * x + y * y + z * z + w * w);

	public double MagnitudeSqr => x * x + y * y + z * z + w * w;

	public Vector4d Normalized
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
				2 => z, 
				3 => w, 
				_ => throw new IndexOutOfRangeException("Invalid Vector4d index!"), 
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
				throw new IndexOutOfRangeException("Invalid Vector4d index!");
			}
		}
	}

	public Vector4d(double value)
	{
		x = value;
		y = value;
		z = value;
		w = value;
	}

	public Vector4d(double x, double y, double z, double w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public static Vector4d operator +(Vector4d a, Vector4d b)
	{
		return new Vector4d(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
	}

	public static Vector4d operator -(Vector4d a, Vector4d b)
	{
		return new Vector4d(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
	}

	public static Vector4d operator *(Vector4d a, double d)
	{
		return new Vector4d(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4d operator *(double d, Vector4d a)
	{
		return new Vector4d(a.x * d, a.y * d, a.z * d, a.w * d);
	}

	public static Vector4d operator /(Vector4d a, double d)
	{
		return new Vector4d(a.x / d, a.y / d, a.z / d, a.w / d);
	}

	public static Vector4d operator -(Vector4d a)
	{
		return new Vector4d(0.0 - a.x, 0.0 - a.y, 0.0 - a.z, 0.0 - a.w);
	}

	public static bool operator ==(Vector4d lhs, Vector4d rhs)
	{
		return (lhs - rhs).MagnitudeSqr < double.Epsilon;
	}

	public static bool operator !=(Vector4d lhs, Vector4d rhs)
	{
		return (lhs - rhs).MagnitudeSqr >= double.Epsilon;
	}

	public static implicit operator Vector4d(Vector4 v)
	{
		return new Vector4d(v.x, v.y, v.z, v.w);
	}

	public static implicit operator Vector4d(Vector4i v)
	{
		return new Vector4d(v.x, v.y, v.z, v.w);
	}

	public void Set(double x, double y, double z, double w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public void Scale(ref Vector4d scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
		w *= scale.w;
	}

	public void Normalize()
	{
		double magnitude = Magnitude;
		if (magnitude > double.Epsilon)
		{
			x /= magnitude;
			y /= magnitude;
			z /= magnitude;
			w /= magnitude;
		}
		else
		{
			x = (y = (z = (w = 0.0)));
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
		if (!(other is Vector4d vector4d))
		{
			return false;
		}
		if (x == vector4d.x && y == vector4d.y && z == vector4d.z)
		{
			return w == vector4d.w;
		}
		return false;
	}

	public bool Equals(Vector4d other)
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

	public static double Dot(ref Vector4d lhs, ref Vector4d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;
	}

	public static void Lerp(ref Vector4d a, ref Vector4d b, double t, out Vector4d result)
	{
		result = new Vector4d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
	}

	public static void Scale(ref Vector4d a, ref Vector4d b, out Vector4d result)
	{
		result = new Vector4d(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
	}

	public static void Normalize(ref Vector4d value, out Vector4d result)
	{
		double magnitude = value.Magnitude;
		if (magnitude > double.Epsilon)
		{
			result = new Vector4d(value.x / magnitude, value.y / magnitude, value.z / magnitude, value.w / magnitude);
		}
		else
		{
			result = zero;
		}
	}
}
