using System;

namespace HellTap.MeshDecimator.Math;

public static class MathHelper
{
	public const float PI = (float)System.Math.PI;

	public const double PId = System.Math.PI;

	public const float Deg2Rad = (float)System.Math.PI / 180f;

	public const double Deg2Radd = System.Math.PI / 180.0;

	public const float Rad2Deg = 180f / (float)System.Math.PI;

	public const double Rad2Degd = 180.0 / System.Math.PI;

	public static int Min(int val1, int val2)
	{
		if (val1 >= val2)
		{
			return val2;
		}
		return val1;
	}

	public static int Min(int val1, int val2, int val3)
	{
		if (val1 >= val2)
		{
			if (val2 >= val3)
			{
				return val3;
			}
			return val2;
		}
		if (val1 >= val3)
		{
			return val3;
		}
		return val1;
	}

	public static float Min(float val1, float val2)
	{
		if (!(val1 < val2))
		{
			return val2;
		}
		return val1;
	}

	public static float Min(float val1, float val2, float val3)
	{
		if (!(val1 < val2))
		{
			if (!(val2 < val3))
			{
				return val3;
			}
			return val2;
		}
		if (!(val1 < val3))
		{
			return val3;
		}
		return val1;
	}

	public static double Min(double val1, double val2)
	{
		if (!(val1 < val2))
		{
			return val2;
		}
		return val1;
	}

	public static double Min(double val1, double val2, double val3)
	{
		if (!(val1 < val2))
		{
			if (!(val2 < val3))
			{
				return val3;
			}
			return val2;
		}
		if (!(val1 < val3))
		{
			return val3;
		}
		return val1;
	}

	public static int Max(int val1, int val2)
	{
		if (val1 <= val2)
		{
			return val2;
		}
		return val1;
	}

	public static int Max(int val1, int val2, int val3)
	{
		if (val1 <= val2)
		{
			if (val2 <= val3)
			{
				return val3;
			}
			return val2;
		}
		if (val1 <= val3)
		{
			return val3;
		}
		return val1;
	}

	public static float Max(float val1, float val2)
	{
		if (!(val1 > val2))
		{
			return val2;
		}
		return val1;
	}

	public static float Max(float val1, float val2, float val3)
	{
		if (!(val1 > val2))
		{
			if (!(val2 > val3))
			{
				return val3;
			}
			return val2;
		}
		if (!(val1 > val3))
		{
			return val3;
		}
		return val1;
	}

	public static double Max(double val1, double val2)
	{
		if (!(val1 > val2))
		{
			return val2;
		}
		return val1;
	}

	public static double Max(double val1, double val2, double val3)
	{
		if (!(val1 > val2))
		{
			if (!(val2 > val3))
			{
				return val3;
			}
			return val2;
		}
		if (!(val1 > val3))
		{
			return val3;
		}
		return val1;
	}

	public static float Clamp(float value, float min, float max)
	{
		if (!(value >= min))
		{
			return min;
		}
		if (!(value <= max))
		{
			return max;
		}
		return value;
	}

	public static double Clamp(double value, double min, double max)
	{
		if (!(value >= min))
		{
			return min;
		}
		if (!(value <= max))
		{
			return max;
		}
		return value;
	}

	public static float Clamp01(float value)
	{
		if (!(value > 0f))
		{
			return 0f;
		}
		if (!(value < 1f))
		{
			return 1f;
		}
		return value;
	}

	public static double Clamp01(double value)
	{
		if (!(value > 0.0))
		{
			return 0.0;
		}
		if (!(value < 1.0))
		{
			return 1.0;
		}
		return value;
	}

	public static float TriangleArea(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
	{
		Vector3 from = p1 - p0;
		Vector3 to = p2 - p0;
		return from.Magnitude * ((float)System.Math.Sin(Vector3.Angle(ref from, ref to) * ((float)System.Math.PI / 180f)) * to.Magnitude) * 0.5f;
	}

	public static double TriangleArea(ref Vector3d p0, ref Vector3d p1, ref Vector3d p2)
	{
		Vector3d from = p1 - p0;
		Vector3d to = p2 - p0;
		return from.Magnitude * (System.Math.Sin(Vector3d.Angle(ref from, ref to) * (System.Math.PI / 180.0)) * to.Magnitude) * 0.5;
	}
}
