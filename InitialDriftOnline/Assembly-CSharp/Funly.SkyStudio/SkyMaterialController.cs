using UnityEngine;

namespace Funly.SkyStudio;

public class SkyMaterialController
{
	[SerializeField]
	private Material _skyboxMaterial;

	[SerializeField]
	private Color _skyColor = ColorHelper.ColorWithHex(2892384u);

	[SerializeField]
	private Color _skyMiddleColor = Color.white;

	[SerializeField]
	private Color _horizonColor = ColorHelper.ColorWithHex(14928002u);

	[SerializeField]
	[Range(-1f, 1f)]
	private float _gradientFadeBegin;

	[SerializeField]
	[Range(0f, 2f)]
	private float _gradientFadeLength = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _skyMiddlePosition = 0.5f;

	[SerializeField]
	private Cubemap _backgroundCubemap;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _starFadeBegin = 0.067f;

	[SerializeField]
	[Range(0f, 2f)]
	private float _starFadeLength = 0.36f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _horizonDistanceScale = 0.7f;

	[SerializeField]
	private Texture _starBasicCubemap;

	[SerializeField]
	private float _starBasicTwinkleSpeed;

	[SerializeField]
	private float _starBasicTwinkleAmount;

	[SerializeField]
	private float _starBasicOpacity;

	[SerializeField]
	private Color _starBasicTintColor;

	[SerializeField]
	private float _starBasicExponent;

	[SerializeField]
	private float _starBasicIntensity;

	[SerializeField]
	private Texture _starLayer1Texture;

	[SerializeField]
	private Texture2D _starLayer1DataTexture;

	[SerializeField]
	private Color _starLayer1Color;

	[SerializeField]
	[Range(0f, 0.1f)]
	private float _starLayer1MaxRadius = 0.007f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _starLayer1TwinkleAmount = 0.7f;

	[SerializeField]
	[Range(0f, 10f)]
	private float _starLayer1TwinkleSpeed = 0.7f;

	[SerializeField]
	[Range(0f, 10f)]
	private float _starLayer1RotationSpeed = 0.7f;

	[SerializeField]
	[Range(0.0001f, 0.9999f)]
	private float _starLayer1EdgeFeathering = 0.2f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _starLayer1BloomFilterBoost;

	[SerializeField]
	private Vector4 _starLayer1SpriteDimensions = Vector4.zero;

	[SerializeField]
	private int _starLayer1SpriteItemCount = 1;

	[SerializeField]
	[Range(0f, 1f)]
	private float _starLayer1SpriteAnimationSpeed = 1f;

	[SerializeField]
	private Texture _starLayer2Texture;

	[SerializeField]
	private Texture2D _starLayer2DataTexture;

	[SerializeField]
	private Color _starLayer2Color;

	[SerializeField]
	[Range(0f, 0.1f)]
	private float _starLayer2MaxRadius = 0.007f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _starLayer2TwinkleAmount = 0.7f;

	[SerializeField]
	[Range(0f, 10f)]
	private float _starLayer2TwinkleSpeed = 0.7f;

	[SerializeField]
	[Range(0f, 10f)]
	private float _starLayer2RotationSpeed = 0.7f;

	[SerializeField]
	[Range(0.0001f, 0.9999f)]
	private float _starLayer2EdgeFeathering = 0.2f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _starLayer2BloomFilterBoost;

	[SerializeField]
	private Vector4 _starLayer2SpriteDimensions = Vector4.zero;

	[SerializeField]
	private int _starLayer2SpriteItemCount = 1;

	[SerializeField]
	[Range(0f, 1f)]
	private float _starLayer2SpriteAnimationSpeed = 1f;

	[SerializeField]
	private Texture _starLayer3Texture;

	[SerializeField]
	private Texture2D _starLayer3DataTexture;

	[SerializeField]
	private Color _starLayer3Color;

	[SerializeField]
	[Range(0f, 0.1f)]
	private float _starLayer3MaxRadius = 0.007f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _starLayer3TwinkleAmount = 0.7f;

	[SerializeField]
	[Range(0f, 10f)]
	private float _starLayer3TwinkleSpeed = 0.7f;

	[SerializeField]
	[Range(0f, 10f)]
	private float _starLayer3RotationSpeed = 0.7f;

	[SerializeField]
	[Range(0.0001f, 0.9999f)]
	private float _starLayer3EdgeFeathering = 0.2f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _starLayer3BloomFilterBoost;

	[SerializeField]
	private Vector4 _starLayer3SpriteDimensions = Vector4.zero;

	[SerializeField]
	private int _starLayer3SpriteItemCount = 1;

	[SerializeField]
	[Range(0f, 1f)]
	private float _starLayer3SpriteAnimationSpeed = 1f;

	[SerializeField]
	private Texture _moonTexture;

	[SerializeField]
	private float _moonRotationSpeed;

	[SerializeField]
	private Color _moonColor = Color.white;

	[SerializeField]
	private Vector3 _moonDirection = Vector3.right;

	[SerializeField]
	private Matrix4x4 _moonWorldToLocalMatrix = Matrix4x4.identity;

	[SerializeField]
	[Range(0f, 1f)]
	private float _moonSize = 0.1f;

	[SerializeField]
	[Range(0.0001f, 0.9999f)]
	private float _moonEdgeFeathering = 0.085f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _moonBloomFilterBoost = 1f;

	[SerializeField]
	private Vector4 _moonSpriteDimensions = Vector4.zero;

	[SerializeField]
	private int _moonSpriteItemCount = 1;

	[SerializeField]
	[Range(0f, 1f)]
	private float _moonSpriteAnimationSpeed = 1f;

	[SerializeField]
	private Texture _sunTexture;

	[SerializeField]
	private Color _sunColor = Color.white;

	[SerializeField]
	private float _sunRotationSpeed;

	[SerializeField]
	private Vector3 _sunDirection = Vector3.right;

	[SerializeField]
	private Matrix4x4 _sunWorldToLocalMatrix = Matrix4x4.identity;

	[SerializeField]
	[Range(0f, 1f)]
	private float _sunSize = 0.1f;

	[SerializeField]
	[Range(0.0001f, 0.9999f)]
	private float _sunEdgeFeathering = 0.085f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _sunBloomFilterBoost = 1f;

	[SerializeField]
	private Vector4 _sunSpriteDimensions = Vector4.zero;

	[SerializeField]
	private int _sunSpriteItemCount = 1;

	[SerializeField]
	[Range(0f, 1f)]
	private float _sunSpriteAnimationSpeed = 1f;

	[SerializeField]
	[Range(-1f, 1f)]
	private float _cloudBegin = 0.2f;

	private float _cloudTextureTiling;

	[SerializeField]
	private Color _cloudColor = Color.white;

	[SerializeField]
	private Texture _cloudTexture;

	[SerializeField]
	private Texture _artCloudCustomTexture;

	[SerializeField]
	private float _cloudDensity;

	[SerializeField]
	private float _cloudSpeed;

	[SerializeField]
	private float _cloudDirection;

	[SerializeField]
	private float _cloudHeight;

	[SerializeField]
	private Color _cloudColor1 = Color.white;

	[SerializeField]
	private Color _cloudColor2 = Color.white;

	[SerializeField]
	private float _cloudFadePosition;

	[SerializeField]
	private float _cloudFadeAmount = 0.5f;

	[SerializeField]
	private Texture _cloudCubemap;

	[SerializeField]
	private float _cloudCubemapRotationSpeed;

	[SerializeField]
	private Texture _cloudCubemapDoubleLayerCustomTexture;

	[SerializeField]
	private float _cloudCubemapDoubleLayerRotationSpeed;

	[SerializeField]
	private float _cloudCubemapDoubleLayerHeight;

	[SerializeField]
	private Color _cloudCubemapDoubleLayerTintColor = Color.white;

	[SerializeField]
	private Color _cloudCubemapTintColor = Color.white;

	[SerializeField]
	private float _cloudCubemapHeight;

	[SerializeField]
	private Texture _cloudCubemapNormalTexture;

	[SerializeField]
	private Color _cloudCubemapNormalLitColor = Color.white;

	[SerializeField]
	private Color _cloudCubemapNormalShadowColor = Color.gray;

	[SerializeField]
	private float _cloudCubemapNormalRotationSpeed;

	[SerializeField]
	private float _cloudCubemapNormalHeight;

	[SerializeField]
	private float _cloudCubemapNormalAmbientItensity;

	[SerializeField]
	private Texture _cloudCubemapNormalDoubleLayerCustomTexture;

	[SerializeField]
	private float _cloudCubemapNormalDoubleLayerRotationSpeed;

	[SerializeField]
	private float _cloudCubemapNormalDoubleLayerHeight;

	[SerializeField]
	private Color _cloudCubemapNormalDoubleLayerLitColor = Color.white;

	[SerializeField]
	private Color _cloudCubemapNormalDoubleLayerShadowColor = Color.gray;

	[SerializeField]
	private Vector3 _cloudCubemapNormalLightDirection = new Vector3(0f, 1f, 0f);

	[SerializeField]
	private Color _fogColor = Color.white;

	[SerializeField]
	private float _fogDensity = 0.12f;

	[SerializeField]
	private float _fogHeight = 0.12f;

	public Material SkyboxMaterial
	{
		get
		{
			return _skyboxMaterial;
		}
		set
		{
			_skyboxMaterial = value;
			RenderSettings.skybox = _skyboxMaterial;
		}
	}

	public Color SkyColor
	{
		get
		{
			return _skyColor;
		}
		set
		{
			_skyColor = value;
			SkyboxMaterial.SetColor("_GradientSkyUpperColor", _skyColor);
		}
	}

	public Color SkyMiddleColor
	{
		get
		{
			return _skyMiddleColor;
		}
		set
		{
			_skyMiddleColor = value;
			SkyboxMaterial.SetColor("_GradientSkyMiddleColor", _skyMiddleColor);
		}
	}

	public Color HorizonColor
	{
		get
		{
			return _horizonColor;
		}
		set
		{
			_horizonColor = value;
			SkyboxMaterial.SetColor("_GradientSkyLowerColor", _horizonColor);
		}
	}

	public float GradientFadeBegin
	{
		get
		{
			return _gradientFadeBegin;
		}
		set
		{
			_gradientFadeBegin = value;
			ApplyGradientValuesOnMaterial();
		}
	}

	public float GradientFadeLength
	{
		get
		{
			return _gradientFadeLength;
		}
		set
		{
			_gradientFadeLength = value;
			ApplyGradientValuesOnMaterial();
		}
	}

	public float SkyMiddlePosition
	{
		get
		{
			return _skyMiddlePosition;
		}
		set
		{
			_skyMiddlePosition = value;
			SkyboxMaterial.SetFloat("_GradientFadeMiddlePosition", _skyMiddlePosition);
		}
	}

	public Cubemap BackgroundCubemap
	{
		get
		{
			return _backgroundCubemap;
		}
		set
		{
			_backgroundCubemap = value;
			SkyboxMaterial.SetTexture("_MainTex", _backgroundCubemap);
		}
	}

	public float StarFadeBegin
	{
		get
		{
			return _starFadeBegin;
		}
		set
		{
			_starFadeBegin = value;
			ApplyStarFadeValuesOnMaterial();
		}
	}

	public float StarFadeLength
	{
		get
		{
			return _starFadeLength;
		}
		set
		{
			_starFadeLength = value;
			ApplyStarFadeValuesOnMaterial();
		}
	}

	public float HorizonDistanceScale
	{
		get
		{
			return _horizonDistanceScale;
		}
		set
		{
			_horizonDistanceScale = value;
			SkyboxMaterial.SetFloat("_HorizonScaleFactor", _horizonDistanceScale);
		}
	}

	public Texture StarBasicCubemap
	{
		get
		{
			return _starBasicCubemap;
		}
		set
		{
			_starBasicCubemap = value;
			SkyboxMaterial.SetTexture("_StarBasicCubemap", _starBasicCubemap);
		}
	}

	public float StarBasicTwinkleSpeed
	{
		get
		{
			return _starBasicTwinkleSpeed;
		}
		set
		{
			_starBasicTwinkleSpeed = value;
			SkyboxMaterial.SetFloat("_StarBasicTwinkleSpeed", _starBasicTwinkleSpeed);
		}
	}

	public float StarBasicTwinkleAmount
	{
		get
		{
			return _starBasicTwinkleAmount;
		}
		set
		{
			_starBasicTwinkleAmount = value;
			SkyboxMaterial.SetFloat("_StarBasicTwinkleAmount", _starBasicTwinkleAmount);
		}
	}

	public float StarBasicOpacity
	{
		get
		{
			return _starBasicOpacity;
		}
		set
		{
			_starBasicOpacity = value;
			SkyboxMaterial.SetFloat("_StarBasicOpacity", _starBasicOpacity);
		}
	}

	public Color StarBasicTintColor
	{
		get
		{
			return _starBasicTintColor;
		}
		set
		{
			_starBasicTintColor = value;
			SkyboxMaterial.SetColor("_StarBasicTintColor", _starBasicTintColor);
		}
	}

	public float StarBasicExponent
	{
		get
		{
			return _starBasicExponent;
		}
		set
		{
			_starBasicExponent = value;
			SkyboxMaterial.SetFloat("_StarBasicExponent", _starBasicExponent);
		}
	}

	public float StarBasicIntensity
	{
		get
		{
			return _starBasicIntensity;
		}
		set
		{
			_starBasicIntensity = value;
			SkyboxMaterial.SetFloat("_StarBasicHDRBoost", _starBasicIntensity);
		}
	}

	public Texture StarLayer1Texture
	{
		get
		{
			return _starLayer1Texture;
		}
		set
		{
			_starLayer1Texture = value;
			SkyboxMaterial.SetTexture("_StarLayer1Tex", _starLayer1Texture);
		}
	}

	public Texture2D StarLayer1DataTexture
	{
		get
		{
			return _starLayer1DataTexture;
		}
		set
		{
			_starLayer1DataTexture = value;
			SkyboxMaterial.SetTexture("_StarLayer1DataTex", value);
		}
	}

	public Color StarLayer1Color
	{
		get
		{
			return _starLayer1Color;
		}
		set
		{
			_starLayer1Color = value;
			SkyboxMaterial.SetColor("_StarLayer1Color", _starLayer1Color);
		}
	}

	public float StarLayer1MaxRadius
	{
		get
		{
			return _starLayer1MaxRadius;
		}
		set
		{
			_starLayer1MaxRadius = value;
			SkyboxMaterial.SetFloat("_StarLayer1MaxRadius", _starLayer1MaxRadius);
		}
	}

	public float StarLayer1TwinkleAmount
	{
		get
		{
			return _starLayer1TwinkleAmount;
		}
		set
		{
			_starLayer1TwinkleAmount = value;
			SkyboxMaterial.SetFloat("_StarLayer1TwinkleAmount", _starLayer1TwinkleAmount);
		}
	}

	public float StarLayer1TwinkleSpeed
	{
		get
		{
			return _starLayer1TwinkleSpeed;
		}
		set
		{
			_starLayer1TwinkleSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer1TwinkleSpeed", _starLayer1TwinkleSpeed);
		}
	}

	public float StarLayer1RotationSpeed
	{
		get
		{
			return _starLayer1RotationSpeed;
		}
		set
		{
			_starLayer1RotationSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer1RotationSpeed", _starLayer1RotationSpeed);
		}
	}

	public float StarLayer1EdgeFeathering
	{
		get
		{
			return _starLayer1EdgeFeathering;
		}
		set
		{
			_starLayer1EdgeFeathering = value;
			SkyboxMaterial.SetFloat("_StarLayer1EdgeFade", _starLayer1EdgeFeathering);
		}
	}

	public float StarLayer1BloomFilterBoost
	{
		get
		{
			return _starLayer1BloomFilterBoost;
		}
		set
		{
			_starLayer1BloomFilterBoost = value;
			SkyboxMaterial.SetFloat("_StarLayer1HDRBoost", _starLayer1BloomFilterBoost);
		}
	}

	public int StarLayer1SpriteItemCount
	{
		get
		{
			return _starLayer1SpriteItemCount;
		}
		set
		{
			_starLayer1SpriteItemCount = value;
			SkyboxMaterial.SetInt("_StarLayer1SpriteItemCount", _starLayer1SpriteItemCount);
		}
	}

	public float StarLayer1SpriteAnimationSpeed
	{
		get
		{
			return _starLayer1SpriteAnimationSpeed;
		}
		set
		{
			_starLayer1SpriteAnimationSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer1SpriteAnimationSpeed", _starLayer1SpriteAnimationSpeed);
		}
	}

	public Texture StarLayer2Texture
	{
		get
		{
			return _starLayer2Texture;
		}
		set
		{
			_starLayer2Texture = value;
			SkyboxMaterial.SetTexture("_StarLayer2Tex", _starLayer2Texture);
		}
	}

	public Texture2D StarLayer2DataTexture
	{
		get
		{
			return _starLayer2DataTexture;
		}
		set
		{
			_starLayer2DataTexture = value;
			SkyboxMaterial.SetTexture("_StarLayer2DataTex", value);
		}
	}

	public Color StarLayer2Color
	{
		get
		{
			return _starLayer2Color;
		}
		set
		{
			_starLayer2Color = value;
			SkyboxMaterial.SetColor("_StarLayer2Color", _starLayer2Color);
		}
	}

	public float StarLayer2MaxRadius
	{
		get
		{
			return _starLayer2MaxRadius;
		}
		set
		{
			_starLayer2MaxRadius = value;
			SkyboxMaterial.SetFloat("_StarLayer2MaxRadius", _starLayer2MaxRadius);
		}
	}

	public float StarLayer2TwinkleAmount
	{
		get
		{
			return _starLayer2TwinkleAmount;
		}
		set
		{
			_starLayer2TwinkleAmount = value;
			SkyboxMaterial.SetFloat("_StarLayer2TwinkleAmount", _starLayer2TwinkleAmount);
		}
	}

	public float StarLayer2TwinkleSpeed
	{
		get
		{
			return _starLayer2TwinkleSpeed;
		}
		set
		{
			_starLayer2TwinkleSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer2TwinkleSpeed", _starLayer2TwinkleSpeed);
		}
	}

	public float StarLayer2RotationSpeed
	{
		get
		{
			return _starLayer2RotationSpeed;
		}
		set
		{
			_starLayer2RotationSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer2RotationSpeed", _starLayer2RotationSpeed);
		}
	}

	public float StarLayer2EdgeFeathering
	{
		get
		{
			return _starLayer2EdgeFeathering;
		}
		set
		{
			_starLayer2EdgeFeathering = value;
			SkyboxMaterial.SetFloat("_StarLayer2EdgeFade", _starLayer2EdgeFeathering);
		}
	}

	public float StarLayer2BloomFilterBoost
	{
		get
		{
			return _starLayer2BloomFilterBoost;
		}
		set
		{
			_starLayer2BloomFilterBoost = value;
			SkyboxMaterial.SetFloat("_StarLayer2HDRBoost", _starLayer2BloomFilterBoost);
		}
	}

	public int StarLayer2SpriteItemCount
	{
		get
		{
			return _starLayer2SpriteItemCount;
		}
		set
		{
			_starLayer2SpriteItemCount = value;
			SkyboxMaterial.SetInt("_StarLayer2SpriteItemCount", _starLayer2SpriteItemCount);
		}
	}

	public float StarLayer2SpriteAnimationSpeed
	{
		get
		{
			return _starLayer2SpriteAnimationSpeed;
		}
		set
		{
			_starLayer2SpriteAnimationSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer2SpriteAnimationSpeed", _starLayer2SpriteAnimationSpeed);
		}
	}

	public Texture StarLayer3Texture
	{
		get
		{
			return _starLayer3Texture;
		}
		set
		{
			_starLayer3Texture = value;
			SkyboxMaterial.SetTexture("_StarLayer3Tex", _starLayer3Texture);
		}
	}

	public Texture2D StarLayer3DataTexture
	{
		get
		{
			return _starLayer3DataTexture;
		}
		set
		{
			_starLayer3DataTexture = value;
			SkyboxMaterial.SetTexture("_StarLayer3DataTex", value);
		}
	}

	public Color StarLayer3Color
	{
		get
		{
			return _starLayer3Color;
		}
		set
		{
			_starLayer3Color = value;
			SkyboxMaterial.SetColor("_StarLayer3Color", _starLayer3Color);
		}
	}

	public float StarLayer3MaxRadius
	{
		get
		{
			return _starLayer3MaxRadius;
		}
		set
		{
			_starLayer3MaxRadius = value;
			SkyboxMaterial.SetFloat("_StarLayer3MaxRadius", _starLayer3MaxRadius);
		}
	}

	public float StarLayer3TwinkleAmount
	{
		get
		{
			return _starLayer3TwinkleAmount;
		}
		set
		{
			_starLayer3TwinkleAmount = value;
			SkyboxMaterial.SetFloat("_StarLayer3TwinkleAmount", _starLayer3TwinkleAmount);
		}
	}

	public float StarLayer3TwinkleSpeed
	{
		get
		{
			return _starLayer3TwinkleSpeed;
		}
		set
		{
			_starLayer3TwinkleSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer3TwinkleSpeed", _starLayer3TwinkleSpeed);
		}
	}

	public float StarLayer3RotationSpeed
	{
		get
		{
			return _starLayer3RotationSpeed;
		}
		set
		{
			_starLayer3RotationSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer3RotationSpeed", _starLayer3RotationSpeed);
		}
	}

	public float StarLayer3EdgeFeathering
	{
		get
		{
			return _starLayer3EdgeFeathering;
		}
		set
		{
			_starLayer3EdgeFeathering = value;
			SkyboxMaterial.SetFloat("_StarLayer3EdgeFade", _starLayer3EdgeFeathering);
		}
	}

	public float StarLayer3BloomFilterBoost
	{
		get
		{
			return _starLayer3BloomFilterBoost;
		}
		set
		{
			_starLayer3BloomFilterBoost = value;
			SkyboxMaterial.SetFloat("_StarLayer3HDRBoost", _starLayer3BloomFilterBoost);
		}
	}

	public int StarLayer3SpriteItemCount
	{
		get
		{
			return _starLayer3SpriteItemCount;
		}
		set
		{
			_starLayer3SpriteItemCount = value;
			SkyboxMaterial.SetInt("_StarLayer3SpriteItemCount", _starLayer3SpriteItemCount);
		}
	}

	public float StarLayer3SpriteAnimationSpeed
	{
		get
		{
			return _starLayer3SpriteAnimationSpeed;
		}
		set
		{
			_starLayer3SpriteAnimationSpeed = value;
			SkyboxMaterial.SetFloat("_StarLayer3SpriteAnimationSpeed", _starLayer3SpriteAnimationSpeed);
		}
	}

	public Texture MoonTexture
	{
		get
		{
			return _moonTexture;
		}
		set
		{
			_moonTexture = value;
			SkyboxMaterial.SetTexture("_MoonTex", _moonTexture);
		}
	}

	public float MoonRotationSpeed
	{
		get
		{
			return _moonRotationSpeed;
		}
		set
		{
			_moonRotationSpeed = value;
			SkyboxMaterial.SetFloat("_MoonRotationSpeed", _moonRotationSpeed);
		}
	}

	public Color MoonColor
	{
		get
		{
			return _moonColor;
		}
		set
		{
			_moonColor = value;
			SkyboxMaterial.SetColor("_MoonColor", _moonColor);
		}
	}

	public Vector3 MoonDirection
	{
		get
		{
			return _moonDirection;
		}
		set
		{
			_moonDirection = value.normalized;
			SkyboxMaterial.SetVector("_MoonPosition", _moonDirection);
		}
	}

	public Matrix4x4 MoonWorldToLocalMatrix
	{
		get
		{
			return _moonWorldToLocalMatrix;
		}
		set
		{
			_moonWorldToLocalMatrix = value;
			SkyboxMaterial.SetMatrix("_MoonWorldToLocalMat", _moonWorldToLocalMatrix);
		}
	}

	public float MoonSize
	{
		get
		{
			return _moonSize;
		}
		set
		{
			_moonSize = value;
			SkyboxMaterial.SetFloat("_MoonRadius", _moonSize);
		}
	}

	public float MoonEdgeFeathering
	{
		get
		{
			return _moonEdgeFeathering;
		}
		set
		{
			_moonEdgeFeathering = value;
			SkyboxMaterial.SetFloat("_MoonEdgeFade", _moonEdgeFeathering);
		}
	}

	public float MoonBloomFilterBoost
	{
		get
		{
			return _moonBloomFilterBoost;
		}
		set
		{
			_moonBloomFilterBoost = value;
			SkyboxMaterial.SetFloat("_MoonHDRBoost", _moonBloomFilterBoost);
		}
	}

	public int MoonSpriteItemCount
	{
		get
		{
			return _moonSpriteItemCount;
		}
		set
		{
			_moonSpriteItemCount = value;
			SkyboxMaterial.SetInt("_MoonSpriteItemCount", _moonSpriteItemCount);
		}
	}

	public float MoonSpriteAnimationSpeed
	{
		get
		{
			return _moonSpriteAnimationSpeed;
		}
		set
		{
			_moonSpriteAnimationSpeed = value;
			SkyboxMaterial.SetFloat("_MoonSpriteAnimationSpeed", _moonSpriteAnimationSpeed);
		}
	}

	public Texture SunTexture
	{
		get
		{
			return _sunTexture;
		}
		set
		{
			_sunTexture = value;
			SkyboxMaterial.SetTexture("_SunTex", _sunTexture);
		}
	}

	public Color SunColor
	{
		get
		{
			return _sunColor;
		}
		set
		{
			_sunColor = value;
			SkyboxMaterial.SetColor("_SunColor", _sunColor);
		}
	}

	public float SunRotationSpeed
	{
		get
		{
			return _sunRotationSpeed;
		}
		set
		{
			_sunRotationSpeed = value;
			SkyboxMaterial.SetFloat("_SunRotationSpeed", _sunRotationSpeed);
		}
	}

	public Vector3 SunDirection
	{
		get
		{
			return _sunDirection;
		}
		set
		{
			_sunDirection = value.normalized;
			SkyboxMaterial.SetVector("_SunPosition", _sunDirection);
		}
	}

	public Matrix4x4 SunWorldToLocalMatrix
	{
		get
		{
			return _sunWorldToLocalMatrix;
		}
		set
		{
			_sunWorldToLocalMatrix = value;
			SkyboxMaterial.SetMatrix("_SunWorldToLocalMat", _sunWorldToLocalMatrix);
		}
	}

	public float SunSize
	{
		get
		{
			return _sunSize;
		}
		set
		{
			_sunSize = value;
			SkyboxMaterial.SetFloat("_SunRadius", _sunSize);
		}
	}

	public float SunEdgeFeathering
	{
		get
		{
			return _sunEdgeFeathering;
		}
		set
		{
			_sunEdgeFeathering = value;
			SkyboxMaterial.SetFloat("_SunEdgeFade", _sunEdgeFeathering);
		}
	}

	public float SunBloomFilterBoost
	{
		get
		{
			return _sunBloomFilterBoost;
		}
		set
		{
			_sunBloomFilterBoost = value;
			SkyboxMaterial.SetFloat("_SunHDRBoost", _sunBloomFilterBoost);
		}
	}

	public int SunSpriteItemCount
	{
		get
		{
			return _sunSpriteItemCount;
		}
		set
		{
			_sunSpriteItemCount = value;
			SkyboxMaterial.SetInt("_SunSpriteItemCount", _sunSpriteItemCount);
		}
	}

	public float SunSpriteAnimationSpeed
	{
		get
		{
			return _sunSpriteAnimationSpeed;
		}
		set
		{
			_sunSpriteAnimationSpeed = value;
			SkyboxMaterial.SetFloat("_SunSpriteAnimationSpeed", _sunSpriteAnimationSpeed);
		}
	}

	public float CloudBegin
	{
		get
		{
			return _cloudBegin;
		}
		set
		{
			_cloudBegin = value;
			SkyboxMaterial.SetFloat("_CloudBegin", _cloudBegin);
		}
	}

	public float CloudTextureTiling
	{
		get
		{
			return _cloudTextureTiling;
		}
		set
		{
			_cloudTextureTiling = value;
			SkyboxMaterial.SetFloat("_CloudTextureTiling", _cloudTextureTiling);
		}
	}

	public Color CloudColor
	{
		get
		{
			return _cloudColor;
		}
		set
		{
			_cloudColor = value;
			SkyboxMaterial.SetColor("_CloudColor", _cloudColor);
		}
	}

	public Texture CloudTexture
	{
		get
		{
			if (!(_cloudTexture != null))
			{
				return Texture2D.blackTexture;
			}
			return _cloudTexture;
		}
		set
		{
			_cloudTexture = value;
			SkyboxMaterial.SetTexture("_CloudNoiseTexture", _cloudTexture);
		}
	}

	public Texture ArtCloudCustomTexture
	{
		get
		{
			if (!(_artCloudCustomTexture != null))
			{
				return Texture2D.blackTexture;
			}
			return _artCloudCustomTexture;
		}
		set
		{
			_artCloudCustomTexture = value;
			SkyboxMaterial.SetTexture("_ArtCloudCustomTexture", _artCloudCustomTexture);
		}
	}

	public float CloudDensity
	{
		get
		{
			return _cloudDensity;
		}
		set
		{
			_cloudDensity = value;
			SkyboxMaterial.SetFloat("_CloudDensity", _cloudDensity);
		}
	}

	public float CloudSpeed
	{
		get
		{
			return _cloudSpeed;
		}
		set
		{
			_cloudSpeed = value;
			SkyboxMaterial.SetFloat("_CloudSpeed", _cloudSpeed);
		}
	}

	public float CloudDirection
	{
		get
		{
			return _cloudDirection;
		}
		set
		{
			_cloudDirection = value;
			SkyboxMaterial.SetFloat("_CloudDirection", _cloudDirection);
		}
	}

	public float CloudHeight
	{
		get
		{
			return _cloudHeight;
		}
		set
		{
			_cloudHeight = value;
			SkyboxMaterial.SetFloat("_CloudHeight", _cloudHeight);
		}
	}

	public Color CloudColor1
	{
		get
		{
			return _cloudColor1;
		}
		set
		{
			_cloudColor1 = value;
			SkyboxMaterial.SetColor("_CloudColor1", _cloudColor1);
		}
	}

	public Color CloudColor2
	{
		get
		{
			return _cloudColor2;
		}
		set
		{
			_cloudColor2 = value;
			SkyboxMaterial.SetColor("_CloudColor2", _cloudColor2);
		}
	}

	public float CloudFadePosition
	{
		get
		{
			return _cloudFadePosition;
		}
		set
		{
			_cloudFadePosition = value;
			SkyboxMaterial.SetFloat("_CloudFadePosition", _cloudFadePosition);
		}
	}

	public float CloudFadeAmount
	{
		get
		{
			return _cloudFadeAmount;
		}
		set
		{
			_cloudFadeAmount = value;
			SkyboxMaterial.SetFloat("_CloudFadeAmount", _cloudFadeAmount);
		}
	}

	public Texture CloudCubemap
	{
		get
		{
			return _cloudCubemap;
		}
		set
		{
			_cloudCubemap = value;
			SkyboxMaterial.SetTexture("_CloudCubemapTexture", _cloudCubemap);
		}
	}

	public float CloudCubemapRotationSpeed
	{
		get
		{
			return _cloudCubemapRotationSpeed;
		}
		set
		{
			_cloudCubemapRotationSpeed = value;
			SkyboxMaterial.SetFloat("_CloudCubemapRotationSpeed", _cloudCubemapRotationSpeed);
		}
	}

	public Texture CloudCubemapDoubleLayerCustomTexture
	{
		get
		{
			return _cloudCubemapDoubleLayerCustomTexture;
		}
		set
		{
			_cloudCubemapDoubleLayerCustomTexture = value;
			SkyboxMaterial.SetTexture("_CloudCubemapDoubleTexture", _cloudCubemapDoubleLayerCustomTexture);
		}
	}

	public float CloudCubemapDoubleLayerRotationSpeed
	{
		get
		{
			return _cloudCubemapDoubleLayerRotationSpeed;
		}
		set
		{
			_cloudCubemapDoubleLayerRotationSpeed = value;
			SkyboxMaterial.SetFloat("_CloudCubemapDoubleLayerRotationSpeed", _cloudCubemapDoubleLayerRotationSpeed);
		}
	}

	public float CloudCubemapDoubleLayerHeight
	{
		get
		{
			return _cloudCubemapDoubleLayerHeight;
		}
		set
		{
			_cloudCubemapDoubleLayerHeight = value;
			SkyboxMaterial.SetFloat("_CloudCubemapDoubleLayerHeight", _cloudCubemapDoubleLayerHeight);
		}
	}

	public Color CloudCubemapDoubleLayerTintColor
	{
		get
		{
			return _cloudCubemapDoubleLayerTintColor;
		}
		set
		{
			_cloudCubemapDoubleLayerTintColor = value;
			SkyboxMaterial.SetColor("_CloudCubemapDoubleLayerTintColor", _cloudCubemapDoubleLayerTintColor);
		}
	}

	public Color CloudCubemapTintColor
	{
		get
		{
			return _cloudCubemapTintColor;
		}
		set
		{
			_cloudCubemapTintColor = value;
			SkyboxMaterial.SetColor("_CloudCubemapTintColor", _cloudCubemapTintColor);
		}
	}

	public float CloudCubemapHeight
	{
		get
		{
			return _cloudCubemapHeight;
		}
		set
		{
			_cloudCubemapHeight = value;
			SkyboxMaterial.SetFloat("_CloudCubemapHeight", _cloudCubemapHeight);
		}
	}

	public Texture CloudCubemapNormalTexture
	{
		get
		{
			return _cloudCubemap;
		}
		set
		{
			_cloudCubemapNormalTexture = value;
			SkyboxMaterial.SetTexture("_CloudCubemapNormalTexture", _cloudCubemapNormalTexture);
		}
	}

	public Color CloudCubemapNormalLitColor
	{
		get
		{
			return _cloudCubemapNormalLitColor;
		}
		set
		{
			_cloudCubemapNormalLitColor = value;
			SkyboxMaterial.SetColor("_CloudCubemapNormalLitColor", _cloudCubemapNormalLitColor);
		}
	}

	public Color CloudCubemapNormalShadowColor
	{
		get
		{
			return _cloudCubemapNormalShadowColor;
		}
		set
		{
			_cloudCubemapNormalShadowColor = value;
			SkyboxMaterial.SetColor("_CloudCubemapNormalShadowColor", _cloudCubemapNormalShadowColor);
		}
	}

	public float CloudCubemapNormalRotationSpeed
	{
		get
		{
			return _cloudCubemapNormalRotationSpeed;
		}
		set
		{
			_cloudCubemapNormalRotationSpeed = value;
			SkyboxMaterial.SetFloat("_CloudCubemapNormalRotationSpeed", _cloudCubemapNormalRotationSpeed);
		}
	}

	public float CloudCubemapNormalHeight
	{
		get
		{
			return _cloudCubemapNormalHeight;
		}
		set
		{
			_cloudCubemapNormalHeight = value;
			SkyboxMaterial.SetFloat("_CloudCubemapNormalHeight", _cloudCubemapNormalHeight);
		}
	}

	public float CloudCubemapNormalAmbientIntensity
	{
		get
		{
			return _cloudCubemapNormalAmbientItensity;
		}
		set
		{
			_cloudCubemapNormalAmbientItensity = value;
			SkyboxMaterial.SetFloat("_CloudCubemapNormalAmbientIntensity", _cloudCubemapNormalAmbientItensity);
		}
	}

	public Texture CloudCubemapNormalDoubleLayerCustomTexture
	{
		get
		{
			return _cloudCubemapNormalDoubleLayerCustomTexture;
		}
		set
		{
			_cloudCubemapNormalDoubleLayerCustomTexture = value;
			SkyboxMaterial.SetTexture("_CloudCubemapNormalDoubleTexture", _cloudCubemapNormalDoubleLayerCustomTexture);
		}
	}

	public float CloudCubemapNormalDoubleLayerRotationSpeed
	{
		get
		{
			return _cloudCubemapNormalDoubleLayerRotationSpeed;
		}
		set
		{
			_cloudCubemapNormalDoubleLayerRotationSpeed = value;
			SkyboxMaterial.SetFloat("_CloudCubemapNormalDoubleLayerRotationSpeed", _cloudCubemapNormalDoubleLayerRotationSpeed);
		}
	}

	public float CloudCubemapNormalDoubleLayerHeight
	{
		get
		{
			return _cloudCubemapDoubleLayerHeight;
		}
		set
		{
			_cloudCubemapNormalDoubleLayerHeight = value;
			SkyboxMaterial.SetFloat("_CloudCubemapNormalDoubleLayerHeight", _cloudCubemapNormalDoubleLayerHeight);
		}
	}

	public Color CloudCubemapNormalDoubleLayerLitColor
	{
		get
		{
			return _cloudCubemapNormalDoubleLayerLitColor;
		}
		set
		{
			_cloudCubemapNormalDoubleLayerLitColor = value;
			SkyboxMaterial.SetColor("_CloudCubemapNormalDoubleLitColor", _cloudCubemapNormalDoubleLayerLitColor);
		}
	}

	public Color CloudCubemapNormalDoubleLayerShadowColor
	{
		get
		{
			return _cloudCubemapNormalDoubleLayerShadowColor;
		}
		set
		{
			_cloudCubemapNormalDoubleLayerShadowColor = value;
			SkyboxMaterial.SetColor("_CloudCubemapNormalDoubleShadowColor", _cloudCubemapNormalDoubleLayerShadowColor);
		}
	}

	public Vector3 CloudCubemapNormalLightDirection
	{
		get
		{
			return _cloudCubemapNormalLightDirection;
		}
		set
		{
			_cloudCubemapNormalLightDirection = value;
			SkyboxMaterial.SetVector("_CloudCubemapNormalToLight", _cloudCubemapNormalLightDirection);
		}
	}

	public Color FogColor
	{
		get
		{
			return _fogColor;
		}
		set
		{
			_fogColor = value;
			SkyboxMaterial.SetColor("_HorizonFogColor", _fogColor);
		}
	}

	public float FogDensity
	{
		get
		{
			return _fogDensity;
		}
		set
		{
			_fogDensity = value;
			SkyboxMaterial.SetFloat("_HorizonFogDensity", _fogDensity);
		}
	}

	public float FogHeight
	{
		get
		{
			return _fogHeight;
		}
		set
		{
			_fogHeight = value;
			SkyboxMaterial.SetFloat("_HorizonFogLength", _fogHeight);
		}
	}

	public void SetStarLayer1SpriteDimensions(int columns, int rows)
	{
		_starLayer1SpriteDimensions.x = columns;
		_starLayer1SpriteDimensions.y = rows;
		SkyboxMaterial.SetVector("_StarLayer1SpriteDimensions", _starLayer1SpriteDimensions);
	}

	public Vector2 GetStarLayer1SpriteDimensions()
	{
		return new Vector2(_starLayer1SpriteDimensions.x, _starLayer1SpriteDimensions.y);
	}

	public void SetStarLayer2SpriteDimensions(int columns, int rows)
	{
		_starLayer2SpriteDimensions.x = columns;
		_starLayer2SpriteDimensions.y = rows;
		SkyboxMaterial.SetVector("_StarLayer2SpriteDimensions", _starLayer2SpriteDimensions);
	}

	public Vector2 GetStarLayer2SpriteDimensions()
	{
		return new Vector2(_starLayer2SpriteDimensions.x, _starLayer2SpriteDimensions.y);
	}

	public void SetStarLayer3SpriteDimensions(int columns, int rows)
	{
		_starLayer3SpriteDimensions.x = columns;
		_starLayer3SpriteDimensions.y = rows;
		SkyboxMaterial.SetVector("_StarLayer3SpriteDimensions", _starLayer3SpriteDimensions);
	}

	public Vector2 GetStarLayer3SpriteDimensions()
	{
		return new Vector2(_starLayer3SpriteDimensions.x, _starLayer3SpriteDimensions.y);
	}

	public void SetMoonSpriteDimensions(int columns, int rows)
	{
		_moonSpriteDimensions.x = columns;
		_moonSpriteDimensions.y = rows;
		SkyboxMaterial.SetVector("_MoonSpriteDimensions", _moonSpriteDimensions);
	}

	public Vector2 GetMoonSpriteDimensions()
	{
		return new Vector2(_moonSpriteDimensions.x, _moonSpriteDimensions.y);
	}

	public void SetSunSpriteDimensions(int columns, int rows)
	{
		_sunSpriteDimensions.x = columns;
		_sunSpriteDimensions.y = rows;
		SkyboxMaterial.SetVector("_SunSpriteDimensions", _sunSpriteDimensions);
	}

	public Vector2 GetSunSpriteDimensions()
	{
		return new Vector2(_sunSpriteDimensions.x, _sunSpriteDimensions.y);
	}

	private void ApplyGradientValuesOnMaterial()
	{
		float value = Mathf.Clamp(_gradientFadeBegin + _gradientFadeLength, -1f, 1f);
		SkyboxMaterial.SetFloat("_GradientFadeBegin", _gradientFadeBegin);
		SkyboxMaterial.SetFloat("_GradientFadeEnd", value);
	}

	private void ApplyStarFadeValuesOnMaterial()
	{
		float value = Mathf.Clamp(_starFadeBegin + _starFadeLength, -1f, 1f);
		SkyboxMaterial.SetFloat("_StarFadeBegin", _starFadeBegin);
		SkyboxMaterial.SetFloat("_StarFadeEnd", value);
	}
}
