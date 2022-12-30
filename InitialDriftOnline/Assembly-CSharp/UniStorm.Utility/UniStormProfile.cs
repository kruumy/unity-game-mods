using UnityEngine;

namespace UniStorm.Utility;

public class UniStormProfile : ScriptableObject
{
	public enum FogTypeEnum
	{
		UnistormFog,
		UnityFog
	}

	public enum FogModeEnum
	{
		Exponential,
		ExponentialSquared
	}

	public Gradient SunColor;

	public Gradient StormySunColor;

	[GradientUsage(true)]
	public Gradient SunSpotColor;

	public Gradient MoonColor;

	public Gradient SkyColor;

	public Gradient AmbientSkyLightColor;

	public Gradient StormyAmbientSkyLightColor;

	public Gradient AmbientEquatorLightColor;

	public Gradient StormyAmbientEquatorLightColor;

	public Gradient AmbientGroundLightColor;

	public Gradient StormyAmbientGroundLightColor;

	public Gradient StarLightColor;

	public Gradient FogColor;

	public Gradient FogStormyColor;

	public Gradient CloudLightColor;

	public Gradient StormyCloudLightColor;

	public Gradient FogLightColor;

	public Gradient StormyFogLightColor;

	public Gradient CloudBaseColor;

	public Gradient CloudStormyBaseColor;

	public Gradient SkyTintColor;

	public AnimationCurve SunIntensityCurve = AnimationCurve.Linear(0f, 0f, 24f, 5f);

	public AnimationCurve MoonIntensityCurve = AnimationCurve.Linear(0f, 0f, 24f, 5f);

	public AnimationCurve AtmosphereThickness = AnimationCurve.Linear(0f, 0f, 24f, 5f);

	public AnimationCurve SunAttenuationCurve = AnimationCurve.Linear(0f, 0f, 24f, 5f);

	public AnimationCurve EnvironmentReflections = AnimationCurve.Linear(0f, 0f, 24f, 1f);

	public AnimationCurve AmbientIntensityCurve = AnimationCurve.Linear(0f, 0f, 24f, 1f);

	public AnimationCurve SunAtmosphericFogIntensity = AnimationCurve.Linear(0f, 2f, 24f, 2f);

	public AnimationCurve MoonAtmosphericFogIntensity = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public AnimationCurve SunControlCurve = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public AnimationCurve MoonObjectFade = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public FogTypeEnum FogType;

	public FogModeEnum FogMode;
}
