using UnityEngine;

namespace UniStorm.Utility;

public class WNG
{
	public static float brightness = 1f;

	public static float contrast = 1f;

	public static int octaves = 4;

	private static int wrap(int n, int period)
	{
		if (n < 0)
		{
			return period + n;
		}
		return n % period;
	}

	public static float Noise(Vector3 pos, int period, int seed)
	{
		pos *= (float)period;
		int num = Mathf.FloorToInt(pos.x);
		int num2 = Mathf.FloorToInt(pos.y);
		int num3 = Mathf.FloorToInt(pos.z);
		Vector3 vector = new Vector3(num, num2, num3);
		float num4 = float.MaxValue;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -1; k <= 1; k++)
				{
					Vector3 vector2 = vector + new Vector3(i, j, k);
					Random.InitState((wrap((int)vector2.x, period) + wrap((int)vector2.y, period) * 131 + wrap((int)vector2.z, period) * 17161) % int.MaxValue + seed);
					num4 = Mathf.Min(num4, Vector3.Distance(b: new Vector3(Random.value + vector2.x, Random.value + vector2.y, Random.value + vector2.z), a: pos));
				}
			}
		}
		return 1f - num4;
	}

	public static float OctaveNoise(Vector3 pos, int octaves, int period, int seed = 0, float persistence = 0.5f)
	{
		float num = 0f;
		float num2 = 0.5f;
		float num3 = 1f;
		float num4 = 0f;
		for (int i = 0; i < octaves; i++)
		{
			num4 += num2;
			num += Noise(pos, Mathf.RoundToInt(num3 * (float)period), seed) * num2;
			num2 *= persistence;
			num3 /= persistence;
		}
		if (octaves == 0)
		{
			return 0f;
		}
		return num / num4;
	}
}
