using System;
using UnityEngine;

namespace UniStorm.Utility;

public class GenerateNoise
{
	public static string baseFolder = "Assets/UniStorm Weather System/Resources/";

	private static ComputeShader _noiseCompute = null;

	private static Texture2D _baseNoiseTexturePrecomputed = null;

	private static RenderTexture _baseNoiseTexture = null;

	private static Texture3D _detailNoiseTexture;

	private static Texture2D _curlNoiseTexture;

	public static ComputeShader noiseCompute
	{
		get
		{
			if (_noiseCompute == null)
			{
				_noiseCompute = Resources.Load<ComputeShader>("Clouds/noiseGeneration");
			}
			return _noiseCompute;
		}
	}

	public static Texture baseNoiseTexture
	{
		get
		{
			if (SystemInfo.supportsComputeShaders)
			{
				if (_baseNoiseTexture == null)
				{
					GenerateBaseCloudNoise();
				}
				return _baseNoiseTexture;
			}
			if (_baseNoiseTexturePrecomputed == null)
			{
				GenerateBaseCloudNoise();
			}
			return _baseNoiseTexturePrecomputed;
		}
	}

	public static Texture3D detailNoiseTexture
	{
		get
		{
			if (_detailNoiseTexture == null)
			{
				GenerateCloudDetailNoise();
			}
			return _detailNoiseTexture;
		}
	}

	public static Texture2D curlNoiseTexture
	{
		get
		{
			if (_curlNoiseTexture == null)
			{
				GenerateCloudCurlNoise();
			}
			return _curlNoiseTexture;
		}
	}

	public static void GenerateBaseCloudNoise()
	{
		if (SystemInfo.supportsComputeShaders)
		{
			Texture2D texture2D = Resources.Load<Texture2D>("Clouds/baseNoise");
			int width = texture2D.width;
			_baseNoiseTexture = new RenderTexture(width, width, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			_baseNoiseTexture.name = "baseNoise";
			_baseNoiseTexture.enableRandomWrite = true;
			_baseNoiseTexture.wrapMode = TextureWrapMode.Repeat;
			_baseNoiseTexture.filterMode = FilterMode.Trilinear;
			_baseNoiseTexture.Create();
			int kernelIndex = noiseCompute.FindKernel("CSBaseNoiseMain");
			noiseCompute.SetFloat("_TextureDim", width);
			noiseCompute.SetInt("_Seed", DateTime.Now.Millisecond);
			noiseCompute.SetTexture(kernelIndex, "_CPUBase", texture2D);
			noiseCompute.SetTexture(kernelIndex, "_Base", _baseNoiseTexture);
			noiseCompute.Dispatch(kernelIndex, width / 8, width / 8, 1);
		}
		else
		{
			_baseNoiseTexturePrecomputed = Resources.Load<Texture2D>("Clouds/baseNoisePrecomputed");
		}
	}

	public static void GenerateCloudDetailNoise()
	{
		_detailNoiseTexture = Resources.Load<Texture3D>("Clouds/detailNoise");
	}

	public static void GenerateCloudCurlNoise()
	{
		_curlNoiseTexture = Resources.Load<Texture2D>("Clouds/curlNoise");
	}
}
