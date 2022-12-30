using UnityEngine;

namespace UniStorm.Utility;

public class PNG
{
	public int octaves = 4;

	private static int[] perm = new int[257]
	{
		151, 160, 137, 91, 90, 15, 131, 13, 201, 95,
		96, 53, 194, 233, 7, 225, 140, 36, 103, 30,
		69, 142, 8, 99, 37, 240, 21, 10, 23, 190,
		6, 148, 247, 120, 234, 75, 0, 26, 197, 62,
		94, 252, 219, 203, 117, 35, 11, 32, 57, 177,
		33, 88, 237, 149, 56, 87, 174, 20, 125, 136,
		171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
		27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
		60, 211, 133, 230, 220, 105, 92, 41, 55, 46,
		245, 40, 244, 102, 143, 54, 65, 25, 63, 161,
		1, 216, 80, 73, 209, 76, 132, 187, 208, 89,
		18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
		164, 100, 109, 198, 173, 186, 3, 64, 52, 217,
		226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
		255, 82, 85, 212, 207, 206, 59, 227, 47, 16,
		58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
		119, 248, 152, 2, 44, 154, 163, 70, 221, 153,
		101, 155, 167, 43, 172, 9, 129, 22, 39, 253,
		19, 98, 108, 110, 79, 113, 224, 232, 178, 185,
		112, 104, 218, 246, 97, 228, 251, 34, 242, 193,
		238, 210, 144, 12, 191, 179, 162, 241, 81, 51,
		145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
		181, 199, 106, 157, 184, 84, 204, 176, 115, 121,
		50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
		222, 114, 67, 29, 24, 72, 243, 141, 128, 195,
		78, 66, 215, 61, 156, 180, 151
	};

	private static float Fade(float t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	private static float Lerp(float t, float a, float b)
	{
		return a + t * (b - a);
	}

	private static float Grad(int hash, float x, float y, float z)
	{
		int num = hash & 0xF;
		float num2 = ((num < 8) ? x : y);
		float num3 = ((num < 4) ? y : ((num == 12 || num == 14) ? x : z));
		return (((num & 1) == 0) ? num2 : (0f - num2)) + (((num & 2) == 0) ? num3 : (0f - num3));
	}

	private static int wrap(int n, int period)
	{
		n++;
		if (period <= 0)
		{
			return n;
		}
		return n % period;
	}

	public static float Noise(Vector3 pos, int period)
	{
		pos *= (float)period;
		float x = pos.x;
		float y = pos.y;
		float z = pos.z;
		int num = Mathf.FloorToInt(x) & 0xFF;
		int num2 = Mathf.FloorToInt(y) & 0xFF;
		int num3 = Mathf.FloorToInt(z) & 0xFF;
		x -= Mathf.Floor(x);
		y -= Mathf.Floor(y);
		z -= Mathf.Floor(z);
		float t = Fade(x);
		float t2 = Fade(y);
		float t3 = Fade(z);
		int num4 = (perm[wrap(num, period)] + num2) & 0xFF;
		int num5 = (perm[wrap(num + 1, period)] + num2) & 0xFF;
		int num6 = (perm[wrap(num4, period)] + num3) & 0xFF;
		int num7 = (perm[wrap(num5, period)] + num3) & 0xFF;
		int num8 = (perm[wrap(num4 + 1, period)] + num3) & 0xFF;
		int num9 = (perm[wrap(num5 + 1, period)] + num3) & 0xFF;
		return (Lerp(t3, Lerp(t2, Lerp(t, Grad(perm[num6], x, y, z), Grad(perm[num7], x - 1f, y, z)), Lerp(t, Grad(perm[num8], x, y - 1f, z), Grad(perm[num9], x - 1f, y - 1f, z))), Lerp(t2, Lerp(t, Grad(perm[num6 + 1], x, y, z - 1f), Grad(perm[num7 + 1], x - 1f, y, z - 1f)), Lerp(t, Grad(perm[num8 + 1], x, y - 1f, z - 1f), Grad(perm[num9 + 1], x - 1f, y - 1f, z - 1f)))) + 1f) / 2f;
	}

	public static float OctaveNoise(Vector3 pos, int period, int octaves, float persistence = 0.5f)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0.5f;
		float num4 = 1f;
		for (int i = 0; i < octaves; i++)
		{
			num += num3;
			num2 += (Noise(pos, Mathf.RoundToInt(num4 * (float)period)) * 2f - 1f) * num3;
			num3 *= persistence;
			num4 *= 2f;
		}
		if (octaves == 0)
		{
			return 0f;
		}
		return (num2 / num + 1f) / 2f;
	}
}
