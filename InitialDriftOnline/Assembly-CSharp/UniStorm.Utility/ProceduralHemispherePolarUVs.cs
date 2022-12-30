using System;
using UnityEngine;

namespace UniStorm.Utility;

public class ProceduralHemispherePolarUVs : MonoBehaviour
{
	private static Mesh _hemisphere;

	private static Mesh _hemisphereInv;

	public static Mesh hemisphere
	{
		get
		{
			if (_hemisphere == null)
			{
				CreateProceduralHemisphereWithUVs();
			}
			return _hemisphere;
		}
	}

	public static Mesh hemisphereInv
	{
		get
		{
			if (_hemisphereInv == null)
			{
				CreateProceduralHemisphereWithUVs();
			}
			return _hemisphereInv;
		}
	}

	private static void CreateProceduralHemisphereWithUVs()
	{
		_hemisphere = new Mesh
		{
			name = "procedurally_created_hemisphere"
		};
		_hemisphere.Clear();
		_hemisphereInv = new Mesh
		{
			name = "procedurally_created_inverted_hemisphere"
		};
		_hemisphereInv.Clear();
		int num = 48;
		int num2 = 32;
		num = UniStormSystem.Instance.CloudDomeTrisCountX;
		num2 = UniStormSystem.Instance.CloudDomeTrisCountY;
		Vector3[] array = new Vector3[(num + 1) * (num2 + 1) + 1];
		Vector2[] array2 = new Vector2[array.Length];
		float num3 = (float)Math.PI;
		float num4 = num3 * 2f;
		array[0] = Vector3.up;
		array2[0] = new Vector2(0.5f, 0.5f);
		for (int i = 0; i < num2 + 1; i++)
		{
			float f = num3 / 1.98f * (float)(i + 1) / (float)(num2 + 1);
			float num5 = num3 / 2f * (float)(i + 1) / (float)(num2 + 1);
			float num6 = Mathf.Sin(f);
			float y = Mathf.Cos(f);
			for (int j = 0; j <= num; j++)
			{
				float f2 = num4 * (float)((j != num) ? j : 0) / (float)num;
				float num7 = Mathf.Sin(f2);
				float num8 = Mathf.Cos(f2);
				array[j + i * (num + 1) + 1] = new Vector3(num6 * num8, y, num6 * num7);
				array2[j + i * (num + 1) + 1] = array2[0] + new Vector2(num8, num7) * (num5 / num3);
			}
		}
		Vector3[] array3 = new Vector3[array.Length];
		for (int k = 0; k < array.Length; k++)
		{
			array3[k] = -array[k].normalized;
		}
		Vector3[] array4 = new Vector3[array.Length];
		for (int l = 0; l < array.Length; l++)
		{
			array4[l] = array[l].normalized;
		}
		int num9 = array.Length * 2 * 3;
		int[] array5 = new int[num9];
		int[] array6 = new int[num9];
		int num10 = 0;
		int num11 = 0;
		for (int m = 0; m < num; m++)
		{
			array5[num10++] = 0;
			array5[num10++] = m + 1;
			array5[num10++] = m + 2;
			array6[num11++] = m + 2;
			array6[num11++] = m + 1;
			array6[num11++] = 0;
		}
		for (int n = 0; n < num2; n++)
		{
			for (int num12 = 0; num12 < num; num12++)
			{
				int num13 = num12 + n * (num + 1) + 1;
				int num14 = num13 + num + 1;
				array5[num10++] = num14 + 1;
				array5[num10++] = num13 + 1;
				array5[num10++] = num13;
				array5[num10++] = num14;
				array5[num10++] = num14 + 1;
				array5[num10++] = num13;
				array6[num11++] = num13;
				array6[num11++] = num13 + 1;
				array6[num11++] = num14 + 1;
				array6[num11++] = num13;
				array6[num11++] = num14 + 1;
				array6[num11++] = num14;
			}
		}
		_hemisphere.vertices = array;
		_hemisphere.normals = array3;
		_hemisphere.uv = array2;
		_hemisphere.triangles = array5;
		_hemisphere.RecalculateBounds();
		_hemisphereInv.vertices = array;
		_hemisphereInv.normals = array4;
		_hemisphereInv.uv = array2;
		_hemisphereInv.triangles = array6;
		_hemisphereInv.RecalculateBounds();
	}
}
