using System;
using UnityEngine;

namespace UniStorm.Effects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UniStormAtmosphericFog : UniStormPostEffectsBase
{
	public enum DitheringControl
	{
		Enabled,
		Disabled
	}

	[HideInInspector]
	public Texture2D NoiseTexture;

	[HideInInspector]
	public DitheringControl Dither;

	[HideInInspector]
	public Light SunSource;

	[HideInInspector]
	public Light MoonSource;

	[HideInInspector]
	public bool distanceFog = true;

	public bool useRadialDistance;

	[HideInInspector]
	public bool heightFog;

	[HideInInspector]
	public float height = 1f;

	[HideInInspector]
	public float heightDensity = 2f;

	public float startDistance;

	[HideInInspector]
	public Shader fogShader;

	public Material fogMaterial;

	[HideInInspector]
	public Color SunColor = new Color(1f, 0.63529f, 0f);

	[HideInInspector]
	public Color MoonColor = new Color(1f, 0.63529f, 0f);

	[HideInInspector]
	public Color TopColor;

	[HideInInspector]
	public Color BottomColor;

	[Range(0f, 1f)]
	public float BlendHeight = 0.03f;

	[HideInInspector]
	[Range(0f, 1f)]
	public float FogGradientHeight = 0.5f;

	[HideInInspector]
	[Range(0f, 3f)]
	public float SunIntensity = 2f;

	[HideInInspector]
	[Range(0f, 3f)]
	public float MoonIntensity = 1f;

	[Range(1f, 60f)]
	public float SunFalloffIntensity = 9.4f;

	[HideInInspector]
	public float SunControl = 1f;

	[HideInInspector]
	public float MoonControl = 1f;

	public override bool CheckResources()
	{
		CheckSupport(needDepth: true);
		fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources() || (!distanceFog && !heightFog))
		{
			Graphics.Blit(source, destination);
			return;
		}
		Camera component = GetComponent<Camera>();
		Transform transform = component.transform;
		float nearClipPlane = component.nearClipPlane;
		float farClipPlane = component.farClipPlane;
		float fieldOfView = component.fieldOfView;
		float aspect = component.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = fieldOfView * 0.5f;
		Vector3 vector = transform.right * nearClipPlane * Mathf.Tan(num * ((float)Math.PI / 180f)) * aspect;
		Vector3 vector2 = transform.up * nearClipPlane * Mathf.Tan(num * ((float)Math.PI / 180f));
		Vector3 vector3 = transform.forward * nearClipPlane - vector + vector2;
		float num2 = vector3.magnitude * farClipPlane / nearClipPlane;
		vector3.Normalize();
		vector3 *= num2;
		Vector3 vector4 = transform.forward * nearClipPlane + vector + vector2;
		vector4.Normalize();
		vector4 *= num2;
		Vector3 vector5 = transform.forward * nearClipPlane + vector - vector2;
		vector5.Normalize();
		vector5 *= num2;
		Vector3 vector6 = transform.forward * nearClipPlane - vector - vector2;
		vector6.Normalize();
		vector6 *= num2;
		identity.SetRow(0, vector3);
		identity.SetRow(1, vector4);
		identity.SetRow(2, vector5);
		identity.SetRow(3, vector6);
		Vector3 position = transform.position;
		float num3 = position.y - height;
		float z = ((num3 <= 0f) ? 1f : 0f);
		fogMaterial.SetMatrix("_FrustumCornersWS", identity);
		fogMaterial.SetVector("_CameraWS", position);
		fogMaterial.SetVector("_HeightParams", new Vector4(height, num3, z, heightDensity * 0.5f));
		fogMaterial.SetVector("_DistanceParams", new Vector4(0f - Mathf.Max(startDistance, 0f), 0f, 0f, 0f));
		fogMaterial.SetVector("_SunVector", SunSource.transform.rotation * -Vector3.forward);
		fogMaterial.SetVector("_MoonVector", MoonSource.transform.rotation * -Vector3.forward);
		fogMaterial.SetFloat("_SunIntensity", SunIntensity);
		fogMaterial.SetFloat("_MoonIntensity", MoonIntensity);
		fogMaterial.SetFloat("_SunAlpha", SunFalloffIntensity);
		fogMaterial.SetColor("_SunColor", SunColor);
		fogMaterial.SetColor("_MoonColor", MoonColor);
		fogMaterial.SetColor("_UpperColor", TopColor);
		fogMaterial.SetColor("_BottomColor", BottomColor);
		fogMaterial.SetFloat("_FogBlendHeight", BlendHeight);
		fogMaterial.SetFloat("_FogGradientHeight", FogGradientHeight);
		fogMaterial.SetFloat("_SunControl", SunControl);
		fogMaterial.SetFloat("_MoonControl", MoonControl);
		if (Dither == DitheringControl.Enabled)
		{
			fogMaterial.SetFloat("_EnableDithering", 1f);
			fogMaterial.SetTexture("_NoiseTex", NoiseTexture);
		}
		else
		{
			fogMaterial.SetFloat("_EnableDithering", 0f);
		}
		FogMode fogMode = RenderSettings.fogMode;
		float fogDensity = RenderSettings.fogDensity;
		float fogStartDistance = RenderSettings.fogStartDistance;
		float fogEndDistance = RenderSettings.fogEndDistance;
		bool flag = fogMode == FogMode.Linear;
		float num4 = (flag ? (fogEndDistance - fogStartDistance) : 0f);
		float num5 = ((Mathf.Abs(num4) > 0.0001f) ? (1f / num4) : 0f);
		Vector4 value = default(Vector4);
		value.x = fogDensity * 1.2011224f;
		value.y = fogDensity * 1.442695f;
		value.z = (flag ? (0f - num5) : 0f);
		value.w = (flag ? (fogEndDistance * num5) : 0f);
		fogMaterial.SetVector("_SceneFogParams", value);
		fogMaterial.SetVector("_SceneFogMode", new Vector4((float)fogMode, useRadialDistance ? 1 : 0, 0f, 0f));
		int num6 = 0;
		CustomGraphicsBlit(passNr: (!distanceFog || !heightFog) ? (distanceFog ? 1 : 2) : 0, source: source, dest: destination, fxMaterial: fogMaterial);
	}

	private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
	{
		RenderTexture.active = dest;
		fxMaterial.SetTexture("_MainTex", source);
		GL.PushMatrix();
		GL.LoadOrtho();
		fxMaterial.SetPass(passNr);
		GL.Begin(7);
		GL.MultiTexCoord2(0, 0f, 0f);
		GL.Vertex3(0f, 0f, 3f);
		GL.MultiTexCoord2(0, 1f, 0f);
		GL.Vertex3(1f, 0f, 2f);
		GL.MultiTexCoord2(0, 1f, 1f);
		GL.Vertex3(1f, 1f, 1f);
		GL.MultiTexCoord2(0, 0f, 1f);
		GL.Vertex3(0f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
	}
}
