using UnityEngine;

namespace AsImpL;

public class ModelUtil
{
	public enum MtlBlendMode
	{
		OPAQUE,
		CUTOUT,
		FADE,
		TRANSPARENT
	}

	public static void SetupMaterialWithBlendMode(Material mtl, MtlBlendMode mode)
	{
		switch (mode)
		{
		case MtlBlendMode.OPAQUE:
			mtl.SetOverrideTag("RenderType", "Opaque");
			mtl.SetFloat("_Mode", 0f);
			mtl.SetInt("_SrcBlend", 1);
			mtl.SetInt("_DstBlend", 0);
			mtl.SetInt("_ZWrite", 1);
			mtl.DisableKeyword("_ALPHATEST_ON");
			mtl.DisableKeyword("_ALPHABLEND_ON");
			mtl.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			mtl.renderQueue = -1;
			break;
		case MtlBlendMode.CUTOUT:
			mtl.SetOverrideTag("RenderType", "TransparentCutout");
			mtl.SetFloat("_Mode", 1f);
			mtl.SetFloat("_Mode", 1f);
			mtl.SetInt("_SrcBlend", 1);
			mtl.SetInt("_DstBlend", 0);
			mtl.SetInt("_ZWrite", 1);
			mtl.EnableKeyword("_ALPHATEST_ON");
			mtl.DisableKeyword("_ALPHABLEND_ON");
			mtl.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			mtl.renderQueue = 2450;
			break;
		case MtlBlendMode.FADE:
			mtl.SetOverrideTag("RenderType", "Transparent");
			mtl.SetFloat("_Mode", 2f);
			mtl.SetInt("_SrcBlend", 5);
			mtl.SetInt("_DstBlend", 10);
			mtl.SetInt("_ZWrite", 0);
			mtl.DisableKeyword("_ALPHATEST_ON");
			mtl.EnableKeyword("_ALPHABLEND_ON");
			mtl.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			mtl.renderQueue = 3000;
			break;
		case MtlBlendMode.TRANSPARENT:
			mtl.SetOverrideTag("RenderType", "Transparent");
			mtl.SetFloat("_Mode", 3f);
			mtl.SetInt("_SrcBlend", 1);
			mtl.SetInt("_DstBlend", 10);
			mtl.SetInt("_ZWrite", 0);
			mtl.DisableKeyword("_ALPHATEST_ON");
			mtl.DisableKeyword("_ALPHABLEND_ON");
			mtl.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			mtl.renderQueue = 3000;
			break;
		}
	}

	public static bool ScanTransparentPixels(Texture2D texture, ref MtlBlendMode mode)
	{
		if (texture != null && (texture.format == TextureFormat.ARGB32 || texture.format == TextureFormat.RGBA32 || texture.format == TextureFormat.DXT5 || texture.format == TextureFormat.ARGB4444 || texture.format == TextureFormat.BGRA32 || texture.format == TextureFormat.DXT5Crunched))
		{
			bool noDoubt = false;
			int num = 1;
			for (int i = 0; i < texture.width; i += num)
			{
				if (noDoubt)
				{
					break;
				}
				for (int j = 0; j < texture.height; j += num)
				{
					if (noDoubt)
					{
						break;
					}
					DetectMtlBlendFadeOrCutout(texture.GetPixel(i, j).a, ref mode, ref noDoubt);
					if (noDoubt)
					{
						if (mode != MtlBlendMode.FADE)
						{
							return mode == MtlBlendMode.CUTOUT;
						}
						return true;
					}
				}
			}
		}
		if (mode != MtlBlendMode.FADE)
		{
			return mode == MtlBlendMode.CUTOUT;
		}
		return true;
	}

	public static void DetectMtlBlendFadeOrCutout(float alpha, ref MtlBlendMode mode, ref bool noDoubt)
	{
		if (!noDoubt && alpha < 1f)
		{
			if (alpha == 0f)
			{
				mode = MtlBlendMode.CUTOUT;
			}
			else if (mode != MtlBlendMode.FADE)
			{
				mode = MtlBlendMode.FADE;
				noDoubt = true;
			}
		}
	}

	public static Texture2D HeightToNormalMap(Texture2D bumpMap, float amount = 1f)
	{
		int height = bumpMap.height;
		int width = bumpMap.width;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: true);
		Color black = Color.black;
		for (int i = 0; i < bumpMap.height; i++)
		{
			for (int j = 0; j < bumpMap.width; j++)
			{
				Vector3 zero = Vector3.zero;
				float grayscale = bumpMap.GetPixel(WrapInt(j - 1, width), i).grayscale;
				float grayscale2 = bumpMap.GetPixel(j, i).grayscale;
				float grayscale3 = bumpMap.GetPixel(WrapInt(j + 1, width), i).grayscale;
				float num = grayscale2 - grayscale;
				float num2 = grayscale3 - grayscale2;
				zero.x = (0f - (num2 + num)) / 255f;
				grayscale = bumpMap.GetPixel(j, WrapInt(i - 1, height)).grayscale;
				grayscale2 = bumpMap.GetPixel(j, i).grayscale;
				float grayscale4 = bumpMap.GetPixel(j, WrapInt(i + 1, height)).grayscale;
				num = grayscale2 - grayscale;
				num2 = grayscale4 - grayscale2;
				zero.y = 0f - (num2 + num);
				if (amount != 1f)
				{
					zero *= amount;
				}
				zero.z = Mathf.Sqrt(1f - (zero.x * zero.x + zero.y * zero.y));
				zero *= 0.5f;
				black.r = Mathf.Clamp01(zero.x + 0.5f);
				black.g = Mathf.Clamp01(zero.y + 0.5f);
				black.b = Mathf.Clamp01(zero.z + 0.5f);
				black.a = black.r;
				texture2D.SetPixel(j, i, black);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private static int WrapInt(int pos, int boundary)
	{
		if (pos < 0)
		{
			pos = boundary + pos;
		}
		else if (pos >= boundary)
		{
			pos -= boundary;
		}
		return pos;
	}
}
