using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math;

public struct Vector3d : IEquatable<Vector3d>
{
	public static readonly Vector3d zero = new Vector3d(0.0, 0.0, 0.0);

	public const double Epsilon = double.Epsilon;

	public double x;

	public double y;

	public double z;

	public double Magnitude => System.Math.Sqrt(x * x + y * y + z * z);

	public double MagnitudeSqr => x * x + y * y + z * z;

	public Vector3d Normalized
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
				_ => throw new IndexOutOfRangeException("Invalid Vector3d index!"), 
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
				throw new IndexOutOfRangeException("Invalid Vector3d index!");
			}
		}
	}

	public Vector3d(double value)
	{
		x = value;
		y = value;
		z = value;
	}

	public Vector3d(double x, double y, double z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3d(Vector3 vector)
	{
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}

	public static Vector3d operator +(Vector3d a, Vector3d b)
	{
		return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3d operator -(Vector3d a, Vector3d b)
	{
		return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3d operator *(Vector3d a, double d)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3d operator *(double d, Vector3d a)
	{
		return new Vector3d(a.x * d, a.y * d, a.z * d);
	}

	public static Vector3d operator /(Vector3d a, double d)
	{
		return new Vector3d(a.x / d, a.y / d, a.z / d);
	}

	public static Vector3d operator -(Vector3d a)
	{
		return new Vector3d(0.0 - a.x, 0.0 - a.y, 0.0 - a.z);
	}

	public static bool operator ==(Vector3d lhs, Vector3d rhs)
	{
		return (lhs - rhs).MagnitudeSqr < double.Epsilon;
	}

	public static bool operator !=(Vector3d lhs, Vector3d rhs)
	{
		return (lhs - rhs).MagnitudeSqr >= double.Epsilon;
	}

	public static implicit operator Vector3d(Vector3 v)
	{
		return new Vector3d(v.x, v.y, v.z);
	}

	public static implicit operator Vector3d(Vector3i v)
	{
		return new Vector3d(v.x, v.y, v.z);
	}

	public void Set(double x, double y, double z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void Scale(ref Vector3d scale)
	{
		x *= scale.x;
		y *= scale.y;
		z *= scale.z;
	}

	public void Normalize()
	{
		double magnitude = Magnitude;
		if (magnitude > double.Epsilon)
		{
			x /= magnitude;
			y /= magnitude;
			z /= magnitude;
		}
		else
		{
			x = (y = (z = 0.0));
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
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector3d vector3d))
		{
			return false;
		}
		if (x == vector3d.x && y == vector3d.y)
		{
			return z == vector3d.z;
		}
		return false;
	}

	public bool Equals(Vector3d other)
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

	public static double Dot(ref Vector3d lhs, ref Vector3d rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static void Cross(ref Vector3d lhs, ref Vector3d rhs, out Vector3d result)
	{
		result = new Vector3d(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
	}

	public static double Angle(ref Vector3d from, ref Vector3d to)
	{
		Vector3d lhs = from.Normalized;
		Vector3d rhs = to.Normalized;
		return System.Math.Acos(MathHelper.Clamp(Dot(ref lhs, ref rhs), -1.0, 1.0)) * (180.0 / System.Math.PI);
	}

	public static void Lerp(ref Vector3d a, ref Vector3d b, double t, out Vector3d result)
	{
		result = new Vector3d(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
	}

	public static void Scale(ref Vector3d a, ref Vector3d b, out Vector3d result)
	{
		result = new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static void Normalize(ref Vector3d value, out Vector3d result)
	{
		double magnitude = value.Magnitude;
		if (magnitude > double.Epsilon)
		{
			result = new Vector3d(value.x / magnitude, value.y / magnitude, value.z / magnitude);
		}
		else
		{
			result = zero;
		}
	}
}
