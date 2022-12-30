using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Funly.SkyStudio;

public class NearbyStarRenderer : BaseStarDataRenderer
{
	private const int kMaxStars = 2000;

	private const int kStarPointTextureWidth = 2048;

	private const float kStarPaddingRadiusMultipler = 2.1f;

	private RenderTexture CreateRenderTexture(string name, int renderTextureSize, RenderTextureFormat format)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(renderTextureSize, renderTextureSize, 0, format, RenderTextureReadWrite.Linear);
		temporary.filterMode = FilterMode.Point;
		temporary.wrapMode = TextureWrapMode.Clamp;
		temporary.name = name;
		return temporary;
	}

	private Material GetNearbyStarMaterial(Vector4 randomSeed, int starCount)
	{
		Material material = new Material(new Material(Shader.Find("Hidden/Funly/Sky Studio/Computation/StarCalcNearby")));
		material.hideFlags = HideFlags.HideAndDontSave;
		material.SetFloat("_StarDensity", density);
		material.SetFloat("_NumStarPoints", starCount);
		material.SetVector("_RandomSeed", randomSeed);
		material.SetFloat("_TextureSize", 2048f);
		return material;
	}

	private void WriteDebugTexture(RenderTexture rt, string path)
	{
		Texture2D tex = ConvertToTexture2D(rt);
		File.WriteAllBytes(path, tex.EncodeToPNG());
	}

	private Texture2D GetStarListTexture(string starTexKey, out int validStarPixelCount)
	{
		Texture2D texture2D = new Texture2D(2048, 1, TextureFormat.RGBAFloat, mipChain: false, linear: true);
		texture2D.filterMode = FilterMode.Point;
		int num = 0;
		float num2 = maxRadius * 2.1f;
		List<Vector4> list = new List<Vector4>();
		bool flag = maxRadius > 0.0015f;
		for (int i = 0; i < 2000; i++)
		{
			Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
			if (flag)
			{
				bool flag2 = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (Vector3.Distance(onUnitSphere, list[j]) < num2)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
			}
			list.Add(onUnitSphere);
			texture2D.SetPixel(num, 0, new Color(onUnitSphere.x, onUnitSphere.y, onUnitSphere.z, 0f));
			num++;
		}
		texture2D.Apply();
		validStarPixelCount = num;
		return texture2D;
	}

	public override IEnumerator ComputeStarData()
	{
		SendProgress(0f);
		RenderTexture renderTexture = CreateRenderTexture("Nearby Star " + layerId, (int)imageSize, RenderTextureFormat.ARGB32);
		RenderTexture active = RenderTexture.active;
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(layerId.GetHashCode());
		Vector4 randomSeed = new Vector4(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
		int validStarPixelCount;
		Texture2D starListTexture = GetStarListTexture(layerId, out validStarPixelCount);
		int starCount = Math.Min(Mathf.FloorToInt(Mathf.Clamp01(density) * 2000f), validStarPixelCount);
		RenderTexture.active = renderTexture;
		Material nearbyStarMaterial = GetNearbyStarMaterial(randomSeed, starCount);
		Graphics.Blit(starListTexture, nearbyStarMaterial);
		Texture2D texture = ConvertToTexture2D(renderTexture);
		RenderTexture.active = active;
		renderTexture.Release();
		UnityEngine.Random.state = state;
		SendCompletion(texture, success: true);
		yield break;
	}

	private Texture2D ConvertToTexture2D(RenderTexture rt)
	{
		Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, mipChain: false);
		texture2D.name = layerId;
		texture2D.filterMode = FilterMode.Point;
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.ReadPixels(new Rect(0f, 0f, rt.width, rt.height), 0, 0, recalculateMipMaps: false);
		texture2D.Apply(updateMipmaps: false);
		return texture2D;
	}
}
