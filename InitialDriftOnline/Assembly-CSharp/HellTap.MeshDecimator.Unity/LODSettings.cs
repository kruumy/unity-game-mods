using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace HellTap.MeshDecimator.Unity;

[Serializable]
public struct LODSettings
{
	private const string _UNTAGGED = "Untagged";

	[Header("LOD Distance")]
	[Range(0.01f, 100f)]
	[Tooltip("At what distance should this LOD be shown? 100 is used for the best quality mesh.")]
	public float lodDistancePercentage;

	[Header("Decimation")]
	[Range(0.01f, 1f)]
	[Tooltip("When decimating, a value of 0 will reduce mesh complexity as much as possible. 1 will preserve it.")]
	public float quality;

	[HideInInspector]
	[Tooltip("Combining Meshes should always be false in MeshKit.")]
	public bool combineMeshes;

	[Header("Renderers")]
	[Tooltip("The Skin Quality setting used in the Renderer.")]
	public SkinQuality skinQuality;

	[Tooltip("The Recieve Shadows setting used in the Renderer.")]
	public bool receiveShadows;

	[Tooltip("The Shadow Casting setting used in the Renderer.")]
	public ShadowCastingMode shadowCasting;

	[Tooltip("The Motion Vectors setting used in the Renderer.")]
	public MotionVectorGenerationMode motionVectors;

	[Tooltip("The Skinned Motion Vectors setting used in the Renderer.")]
	public bool skinnedMotionVectors;

	[Tooltip("The Light Probe Usage setting found in the Renderer.")]
	public LightProbeUsage lightProbeUsage;

	[Tooltip("The Reflection Probe Usage setting found in the Renderer.")]
	public ReflectionProbeUsage reflectionProbeUsage;

	[Header("GameObject")]
	[Tooltip("The tag to use on the GameObject.")]
	public string tag;

	[Tooltip("The layer to use on the GameObject.")]
	public int layer;

	public LODSettings(float quality, float lodDistancePercentage = 0.8f)
	{
		this.quality = quality;
		this.lodDistancePercentage = lodDistancePercentage;
		combineMeshes = false;
		skinQuality = SkinQuality.Auto;
		receiveShadows = true;
		shadowCasting = ShadowCastingMode.On;
		motionVectors = MotionVectorGenerationMode.Object;
		skinnedMotionVectors = true;
		lightProbeUsage = LightProbeUsage.BlendProbes;
		reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
		tag = "Untagged";
		layer = 0;
	}

	public LODSettings(float quality, float lodDistancePercentage, SkinQuality skinQuality)
	{
		this.quality = quality;
		this.lodDistancePercentage = lodDistancePercentage;
		combineMeshes = false;
		this.skinQuality = skinQuality;
		receiveShadows = true;
		shadowCasting = ShadowCastingMode.On;
		motionVectors = MotionVectorGenerationMode.Object;
		skinnedMotionVectors = true;
		lightProbeUsage = LightProbeUsage.BlendProbes;
		reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
		tag = "Untagged";
		layer = 0;
	}

	public LODSettings(float quality, float lodDistancePercentage, SkinQuality skinQuality, bool receiveShadows, ShadowCastingMode shadowCasting)
	{
		this.quality = quality;
		this.lodDistancePercentage = lodDistancePercentage;
		combineMeshes = false;
		this.skinQuality = skinQuality;
		this.receiveShadows = receiveShadows;
		this.shadowCasting = shadowCasting;
		motionVectors = MotionVectorGenerationMode.Object;
		skinnedMotionVectors = true;
		lightProbeUsage = LightProbeUsage.BlendProbes;
		reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
		tag = "Untagged";
		layer = 0;
	}

	public LODSettings(float quality, float lodDistancePercentage, SkinQuality skinQuality, bool receiveShadows, ShadowCastingMode shadowCasting, MotionVectorGenerationMode motionVectors, bool skinnedMotionVectors)
	{
		this.quality = quality;
		this.lodDistancePercentage = lodDistancePercentage;
		combineMeshes = false;
		this.skinQuality = skinQuality;
		this.receiveShadows = receiveShadows;
		this.shadowCasting = shadowCasting;
		this.motionVectors = motionVectors;
		this.skinnedMotionVectors = skinnedMotionVectors;
		lightProbeUsage = LightProbeUsage.BlendProbes;
		reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
		tag = "Untagged";
		layer = 0;
	}

	public LODSettings(float quality, float lodDistancePercentage, SkinQuality skinQuality, bool receiveShadows, ShadowCastingMode shadowCasting, MotionVectorGenerationMode motionVectors, bool skinnedMotionVectors, LightProbeUsage lightProbeUsage, ReflectionProbeUsage reflectionProbeUsage)
	{
		this.quality = quality;
		this.lodDistancePercentage = lodDistancePercentage;
		combineMeshes = false;
		this.skinQuality = skinQuality;
		this.receiveShadows = receiveShadows;
		this.shadowCasting = shadowCasting;
		this.motionVectors = motionVectors;
		this.skinnedMotionVectors = skinnedMotionVectors;
		this.lightProbeUsage = lightProbeUsage;
		this.reflectionProbeUsage = reflectionProbeUsage;
		tag = "Untagged";
		layer = 0;
	}

	public LODSettings(float quality, float lodDistancePercentage, SkinQuality skinQuality, bool receiveShadows, ShadowCastingMode shadowCasting, MotionVectorGenerationMode motionVectors, bool skinnedMotionVectors, LightProbeUsage lightProbeUsage, ReflectionProbeUsage reflectionProbeUsage, string tag, int layer)
	{
		this.quality = quality;
		this.lodDistancePercentage = lodDistancePercentage;
		combineMeshes = false;
		this.skinQuality = skinQuality;
		this.receiveShadows = receiveShadows;
		this.shadowCasting = shadowCasting;
		this.motionVectors = motionVectors;
		this.skinnedMotionVectors = skinnedMotionVectors;
		this.lightProbeUsage = lightProbeUsage;
		this.reflectionProbeUsage = reflectionProbeUsage;
		this.tag = tag;
		this.layer = layer;
	}
}
