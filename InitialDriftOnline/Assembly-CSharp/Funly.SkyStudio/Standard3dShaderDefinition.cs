using System;
using UnityEngine;

namespace Funly.SkyStudio;

public class Standard3dShaderDefinition : BaseShaderDefinition
{
	public const float MaxStarSize = 0.2f;

	public const float MaxStarDensity = 1f;

	public const float MinEdgeFeathering = 0.0001f;

	public const float MinStarFadeBegin = -0.999f;

	public const float MaxSpriteItems = 100000f;

	public const float MinRotationSpeed = -10f;

	public const float MaxRotationSpeed = 10f;

	public const float MaxCloudRotationSpeed = 0.5f;

	public const float MaxHDRValue = 25f;

	public Standard3dShaderDefinition()
	{
		base.shaderName = "Funly/Sky Studio/Skybox/3D Standard";
	}

	protected override ProfileFeatureSection[] ProfileFeatureSection()
	{
		return new ProfileFeatureSection[10]
		{
			new ProfileFeatureSection("Features", "FeaturesSectionKey", new ProfileFeatureDefinition[13]
			{
				ProfileFeatureDefinition.CreateBooleanFeature("MobileQualityFeature", value: false, "Use Mobile Features (Faster)", null, dependsOnValue: false, "Enables Sky Studio to render in mobile mode, which has simplified options and settings for low-end devices"),
				ProfileFeatureDefinition.CreateShaderFeature("GradientSkyFeature", "GRADIENT_BACKGROUND", value: true, "Gradient Background", null, dependsOnValue: false, "Enables gradient background feature in shader as an alternative to a cubemap background."),
				ProfileFeatureDefinition.CreateShaderFeature("SunFeature", "SUN", value: false, "Sun", null, dependsOnValue: false, "Enables sun feature in skybox shader."),
				ProfileFeatureDefinition.CreateShaderFeature("MoonFeature", "MOON", value: false, "Moon", null, dependsOnValue: false, "Enables moon feature in skybox shader."),
				ProfileFeatureDefinition.CreateShaderFeature("CloudFeature", "CLOUDS", value: false, "Clouds", null, dependsOnValue: false, "Enables cloud feature in the skybox shader."),
				ProfileFeatureDefinition.CreateShaderFeature("FogFeature", "HORIZON_FOG", value: false, "Fog", null, dependsOnValue: false, "Enables fog feature in the skybox shader."),
				ProfileFeatureDefinition.CreateShaderFeature("StarBasicFeature", "STARS_BASIC", value: false, "Stars (Mobile)", "MobileQualityFeature", dependsOnValue: true, "Enable fast simple cubemap stars designed for mobile devices for faster rendering."),
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer1Feature", "STAR_LAYER_1", value: false, "Star Layer 1", "MobileQualityFeature", dependsOnValue: false, "Enables a layer of stars in the shader. Use less star layers for better performance."),
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer2Feature", "STAR_LAYER_2", value: false, "Star Layer 2", "MobileQualityFeature", dependsOnValue: false, "Enables a layer of stars in the shader. Use less star layers for better performance."),
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer3Feature", "STAR_LAYER_3", value: false, "Star Layer 3", "MobileQualityFeature", dependsOnValue: false, "Enables a layer of stars in the shader. Use less star layers for better performance."),
				ProfileFeatureDefinition.CreateBooleanFeature("RainFeature", value: false, "Rain", null, dependsOnValue: false, "Enables animated rain in the scene."),
				ProfileFeatureDefinition.CreateBooleanFeature("RainSplashFeature", value: false, "Rain Surface Splashes", null, dependsOnValue: false, "Enables surface splashes to simulate raindrops hitting the ground and other scene objects."),
				ProfileFeatureDefinition.CreateBooleanFeature("LightningFeature", value: false, "Lightning", null, dependsOnValue: false, "Enables lighting bolts in the scene at user staged spawn areas.")
			}),
			new ProfileFeatureSection("Sky", "SkySectionKey", new ProfileFeatureDefinition[1] { ProfileFeatureDefinition.CreateShaderFeature("VertexGradientSkyFeature", "VERTEX_GRADIENT_BACKGROUND", value: false, "Use Vertex Gradient (Faster)", null, dependsOnValue: false, "If enabled the background gradient colors will be calculated per vertex (which is very fast on low-end hardware) as opposed to per pixel fragment.") }),
			new ProfileFeatureSection("Sun", "SunSectionKey", new ProfileFeatureDefinition[4]
			{
				ProfileFeatureDefinition.CreateShaderFeature("SunCustomTextureFeature", "SUN_CUSTOM_TEXTURE", value: false, "Use Custom Texture", null, dependsOnValue: false, "Enables a custom texture to be used for the sun."),
				ProfileFeatureDefinition.CreateShaderFeature("SunSpriteSheetFeature", "SUN_SPRITE_SHEET", value: false, "Texture Is Sprite Sheet Animation", "SunCustomTextureFeature", dependsOnValue: true, "If enabled the sun texture will be used as a sprite sheet animation."),
				ProfileFeatureDefinition.CreateShaderFeature("SunAlphaBlendFeature", "SUN_ALPHA_BLEND", value: false, "Use Alpha Blending", "SunCustomTextureFeature", dependsOnValue: true, "Enables alpha blending of the sun texture into the background. If disabled additive blending will be used."),
				ProfileFeatureDefinition.CreateShaderFeature("SunRotationFeature", "SUN_ROTATION", value: false, "Animate Sun Rotation", "SunCustomTextureFeature", dependsOnValue: true, "If enabled the sun texture will rotate using the rotation speed property")
			}),
			new ProfileFeatureSection("Moon", "MoonSectionKey", new ProfileFeatureDefinition[4]
			{
				ProfileFeatureDefinition.CreateShaderFeature("MoonCustomTextureFeature", "MOON_CUSTOM_TEXTURE", value: false, "Use Custom Texture", null, dependsOnValue: false, "Enables a custom texture to be used for the moon."),
				ProfileFeatureDefinition.CreateShaderFeature("MoonSpriteSheetFeature", "MOON_SPRITE_SHEET", value: false, "Texture Is Sprite Sheet Animation", "MoonCustomTextureFeature", dependsOnValue: true, "If enabled the moon texture will be used as a sprite sheet animation."),
				ProfileFeatureDefinition.CreateShaderFeature("MoonAlphaBlendFeature", "MOON_ALPHA_BLEND", value: false, "Use Alpha Blending", "MoonCustomTextureFeature", dependsOnValue: true, "Enables alpha blending of the moon texture into the background. If disabled additive blending will be used."),
				ProfileFeatureDefinition.CreateShaderFeature("MoonRotationFeature", "MOON_ROTATION", value: false, "Animate Moon Rotation", "MoonCustomTextureFeature", dependsOnValue: true, "If enabled the moon texture will rotate using the rotation speed property")
			}),
			new ProfileFeatureSection("Star Layer 1", "Star1SectionKey", new ProfileFeatureDefinition[2]
			{
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer1CustomTextureFeature", "STAR_LAYER_1_CUSTOM_TEXTURE", value: false, "Use Custom Texture", null, dependsOnValue: false, "Enables a layer of stars in the shader. Use less star layers for better performance."),
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer1SpriteSheetFeature", "STAR_LAYER_1_SPRITE_SHEET", value: false, "Texture Is Sprite Sheet Animation", "StarLayer1CustomTextureFeature", dependsOnValue: true, "If enabled star texture will be used as a sprite sheet animation.")
			}),
			new ProfileFeatureSection("Star Layer 2", "Star2SectionKey", new ProfileFeatureDefinition[2]
			{
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer2CustomTextureFeature", "STAR_LAYER_2_CUSTOM_TEXTURE", value: false, "Use Custom Texture", null, dependsOnValue: false, "Enables a layer of stars in the shader. Use less star layers for better performance."),
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer2SpriteSheetFeature", "STAR_LAYER_2_SPRITE_SHEET", value: false, "Texture Is Sprite Sheet Animation", "StarLayer2CustomTextureFeature", dependsOnValue: true, "If enabled star texture will be used as a sprite sheet animation.")
			}),
			new ProfileFeatureSection("Clouds", "CloudSectionKey", new ProfileFeatureDefinition[5]
			{
				ProfileFeatureDefinition.CreateShaderFeatureDropdown(new string[3] { "NoiseCloudFeature", "CubemapCloudFeature", "CubemapNormalCloudFeature" }, new string[3] { "NOISE_CLOUDS", "CUBEMAP_CLOUDS", "CUBEMAP_NORMAL_CLOUDS" }, new string[3] { "Standard Clouds", "Cubemap Clouds", "Cubemap Lit Clouds" }, 0, "Cloud Type", "CloudFeature", dependsOnValue: true, "Use the standard soft clouds, or use a fully art customizable cubemap for clouds."),
				ProfileFeatureDefinition.CreateShaderFeatureDropdown(new string[2] { "CubemapCloudTextureFormatRGBFeature", "CubemapCloudTextureFormatRGBAFeature" }, new string[2] { "CUBEMAP_CLOUD_FORMAT_RGB", "CUBEMAP_CLOUD_FORMAT_RGBA" }, new string[2] { "RGB - Additive texture", "RGBA - Alpha texture" }, 1, "Cloud Texture Format", "CubemapCloudFeature", dependsOnValue: true, "Texture format so the shader knows how to load and blend the clouds into the background."),
				ProfileFeatureDefinition.CreateShaderFeature("CubemapCloudDoubleLayerFeature", "CUBEMAP_CLOUD_DOUBLE_LAYER", value: false, "Double Layer Clouds", "CubemapCloudFeature", dependsOnValue: true, "If enabled, the skybox will render 2 layers of clouds which gives a sense of relative cloud motion in the sky."),
				ProfileFeatureDefinition.CreateShaderFeature("CubemapCloudDoubleLayerCubemap", "CUBEMAP_CLOUD_DOUBLE_LAYER_CUSTOM_TEXTURE", value: false, "Double Layer Uses Custom Cubemap", "CubemapCloudDoubleLayerFeature", dependsOnValue: true, "If enabled, you can specify a custom texture to be used as the double layer. If disabled the same cloud texture will be used at a different rotation offset"),
				ProfileFeatureDefinition.CreateShaderFeature("CubemapNormalCloudDoubleLayerFeature", "CUBEMAP_NORMAL_CLOUD_DOUBLE_LAYER", value: false, "Double Layer Clouds", "CubemapNormalCloudFeature", dependsOnValue: true, "If enabled, the skybox will render 2 layers of clouds which gives a sense of relative cloud motion in the sky.")
			}),
			new ProfileFeatureSection("Star Layer 3", "Star3SectionKey", new ProfileFeatureDefinition[2]
			{
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer3CustomTextureFeature", "STAR_LAYER_3_CUSTOM_TEXTURE", value: false, "Use Custom Texture", null, dependsOnValue: false, "Enables a layer of stars in the shader. Use less star layers for better performance."),
				ProfileFeatureDefinition.CreateShaderFeature("StarLayer3SpriteSheetFeature", "STAR_LAYER_3_SPRITE_SHEET", value: false, "Texture Is Sprite Sheet Animation", "StarLayer3CustomTextureFeature", dependsOnValue: true, "If enabled star texture will be used as a sprite sheet animation.")
			}),
			new ProfileFeatureSection("Rain", "RainSectionKey", new ProfileFeatureDefinition[1] { ProfileFeatureDefinition.CreateBooleanFeature("RainSoundFeature", value: true, "Rain Sound", null, dependsOnValue: false, "Plays sound clip of rain in a loop.") }),
			new ProfileFeatureSection("Lightning", "LightningSectionKey", new ProfileFeatureDefinition[1] { ProfileFeatureDefinition.CreateBooleanFeature("ThunderFeature", value: true, "Thunder Sounds", "LightningFeature", dependsOnValue: true, "If enabled thunder sound effects will happen when lightning bolts strike.") })
		};
	}

	protected override ProfileGroupSection[] ProfileDefinitionTable()
	{
		return new ProfileGroupSection[12]
		{
			new ProfileGroupSection("Sky", "SkySectionKey", "SkySectionIcon", null, dependsOnValue: false, new ProfileGroupDefinition[10]
			{
				ProfileGroupDefinition.TextureGroupDefinition("Sky Cubemap", "SkyCubemapKey", null, ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: false, "Image used as background for the skybox."),
				ProfileGroupDefinition.ColorGroupDefinition("Sky Upper Color", "SkyUpperColorKey", ColorHelper.ColorWithHex(2892384u), ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: true, "Top color of the sky when using a gradient background."),
				ProfileGroupDefinition.ColorGroupDefinition("Sky Middle Color", "SkyMiddleColorKey", Color.white, ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: true, "Middle color of the sky when using the gradient background."),
				ProfileGroupDefinition.ColorGroupDefinition("Sky Lower Color", "SkyLowerColorKey", ColorHelper.ColorWithHex(14928002u), ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: true, "Bottom color of the sky when using a gradient background."),
				ProfileGroupDefinition.NumberGroupDefinition("Sky Middle Color Balance", "SkyMiddleColorPosition", 0f, 1f, 0.5f, ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: true, "Shift the middle color closer to lower color or closer upper color to alter the gradient balance."),
				ProfileGroupDefinition.NumberGroupDefinition("Horizon Position", "HorizonTransitionStartKey", -1f, 1f, -0.3f, ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: true, "This vertical position controls where the gradient background will begin."),
				ProfileGroupDefinition.NumberGroupDefinition("Sky Gradient Length", "HorizonTransitionLengthKey", 0f, 2f, 1f, ProfileGroupDefinition.RebuildType.None, "GradientSkyFeature", dependsOnValue: true, "The length of the background gradient fade from the bottom color to the top color."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Start", "StarTransitionStartKey", -1f, 1f, 0.2f, "Vertical position where the stars will begin fading in from. Typically this is just above the horizon."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Transition Length", "StarTransitionLengthKey", 0f, 2f, 0.5f, "The length of the fade-in where stars go from invisible to visible."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Distance Scale", "HorizonStarScaleKey", 0.01f, 1f, 0.7f, "Scale value applied to stars closers to the horizon for distance effect.")
			}),
			new ProfileGroupSection("Sun", "SunSectionKey", "SunSectionIcon", "SunFeature", dependsOnValue: true, new ProfileGroupDefinition[13]
			{
				ProfileGroupDefinition.ColorGroupDefinition("Sun Color", "SunColorKey", ColorHelper.ColorWithHex(16769024u), "Color of the sun."),
				ProfileGroupDefinition.TextureGroupDefinition("Sun Texture", "SunTextureKey", null, ProfileGroupDefinition.RebuildType.None, "SunCustomTextureFeature", dependsOnValue: true, "Texture used for the sun."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Sprite Columns", "SunSpriteColumnCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "SunSpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Sprite Rows", "SunSpriteRowCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "SunSpriteSheetFeature", dependsOnValue: true, "Number of rows in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Sprite Item Count", "SunSpriteItemCount", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "SunSpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Sprite Animation Speed", "SunSpriteAnimationSpeed", 0f, 90f, 15f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "SunSpriteSheetFeature", dependsOnValue: true, "Frames per second to flip through the sprite images."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Rotation Speed", "SunRotationSpeedKey", -10f, 10f, 1f, ProfileGroupDefinition.RebuildType.None, "SunRotationFeature", dependsOnValue: true, "Speed value for sun texture rotation animation."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Size", "SunSizeKey", 0f, 1f, 0.1f, "Size of the sun."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Edge Feathering", "SunEdgeFeatheringKey", 0.0001f, 1f, 0.8f, "Percent amount of gradient fade-in from the sun edges to it's center point."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Bloom Intensity", "SunColorIntensityKey", 1f, 25f, 1f, "Value that's multiplied against the suns color to intensify bloom effects."),
				ProfileGroupDefinition.ColorGroupDefinition("Sun Light Color", "SunLightColorKey", Color.white, "Color of the directional light coming from the sun."),
				ProfileGroupDefinition.NumberGroupDefinition("Sun Light Intensity", "SunLightIntensityKey", 0f, 5f, 1f, "Intensity of the directional light coming from the sun."),
				ProfileGroupDefinition.SpherePointGroupDefinition("Sun Position", "SunPositionKey", 0f, 0f, "Position of the sun in the skybox expressed as a horizontal and vertical rotation.")
			}),
			new ProfileGroupSection("Moon", "MoonSectionKey", "MoonSectionIcon", "MoonFeature", dependsOnValue: true, new ProfileGroupDefinition[13]
			{
				ProfileGroupDefinition.ColorGroupDefinition("Moon Color", "MoonColorKey", ColorHelper.ColorWithHex(10000536u), "Color of the moon."),
				ProfileGroupDefinition.TextureGroupDefinition("Moon Texture", "MoonTextureKey", null, ProfileGroupDefinition.RebuildType.None, "MoonCustomTextureFeature", dependsOnValue: true, "Texture used for the moon"),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Sprite Columns", "MoonSpriteColumnCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "MoonSpriteSheetFeature", dependsOnValue: true, "Number of columns in the moon sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Sprite Rows", "MoonSpriteRowCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "MoonSpriteSheetFeature", dependsOnValue: true, "Number of rows in the moon sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Sprite Item Count", "MoonSpriteItemCount", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "MoonSpriteSheetFeature", dependsOnValue: true, "Number of columns in the moon sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Sprite Animation Speed", "MoonSpriteAnimationSpeed", 0f, 90f, 15f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "MoonSpriteSheetFeature", dependsOnValue: true, "Frames per second to flip through the sprite images."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Rotation Speed", "MoonRotationSpeedKey", -10f, 10f, 1f, ProfileGroupDefinition.RebuildType.None, "MoonRotationFeature", dependsOnValue: true, "Speed value for moon texture rotation animation."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Size", "MoonSizeKey", 0f, 1f, 0.08f, "Size of the moon."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Edge Feathering", "MoonEdgeFeatheringKey", 0.0001f, 1f, 0.1f, "Percentage of fade-in from edge to the center of the moon."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Bloom Intensity", "MoonColorIntensityKey", 1f, 25f, 1f, "Value multiplied with the moon color to help intensify bloom filters."),
				ProfileGroupDefinition.ColorGroupDefinition("Moon Light Color", "MoonLightColorKey", Color.white, "Color of the directional light coming from the moon."),
				ProfileGroupDefinition.NumberGroupDefinition("Moon Light Intensity", "MoonLightIntensityKey", 0f, 5f, 1f, "Intensity of the directional light coming from the moon."),
				ProfileGroupDefinition.SpherePointGroupDefinition("Moon Position", "MoonPositionKey", 0f, 0f, "Position of the moon in the skybox expressed as a horizontal and vertical rotation.")
			}),
			new ProfileGroupSection("Clouds", "CloudSectionKey", "CloudSectionIcon", "CloudFeature", dependsOnValue: true, new ProfileGroupDefinition[29]
			{
				ProfileGroupDefinition.TextureGroupDefinition("Cloud Cubemap", "CloudCubemapTextureKey", null, ProfileGroupDefinition.RebuildType.None, "CubemapCloudFeature", dependsOnValue: true, "Cubemap texture with clouds to render"),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Cubemap Rotation Speed", "CloudCubemapRotationSpeedKey", -0.5f, 0.5f, 0.1f, "CubemapCloudFeature", dependsOnValue: true, "Speed and direction to rotate the cloud cubemap in."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Cubemap Height", "CloudCubemapHeightKey", -1f, 1f, 0f, "CubemapCloudFeature", dependsOnValue: true, "Adjust the horizon height of the cloud cubemap."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Cubemap Tint Color", "CloudCubemapTintColorKey", Color.white, "CubemapCloudFeature", dependsOnValue: true, "Tint color that will be multiplied against the cubemap texture color."),
				ProfileGroupDefinition.TextureGroupDefinition("Cloud Cubemap Double Layer Cubemap", "CloudCubemapDoubleLayerCustomTextureKey", null, "CubemapCloudDoubleLayerCubemap", dependsOnValue: true, "Cubemap texture to use as the atmosphere layer for the clouds."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Cubemap Double Layer Rotation Speed", "CloudCubemapDoubleLayerRotationSpeedKey", -0.5f, 0.5f, 0.15f, "CubemapCloudDoubleLayerFeature", dependsOnValue: true, "Speed and direction to rotate the second layer of clouds."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Cubemap Double Layer Height", "CloudCubemapDoubleLayerHeightKey", -1f, 1f, 0f, "CubemapCloudDoubleLayerFeature", dependsOnValue: true, "Adjust the horizon height of the second cloud cubemap layer."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Cubemap Double Layer Tint Color", "CloudCubemapDoubleLayerTintColorKey", Color.white, "CubemapCloudDoubleLayerFeature", dependsOnValue: true, "Tint color that will be multiplied against the double texture cubemap."),
				ProfileGroupDefinition.TextureGroupDefinition("Cloud Lit Cubemap", "CloudCubemapNormalTextureKey", null, ProfileGroupDefinition.RebuildType.None, "CubemapNormalCloudFeature", dependsOnValue: true, "Cubemap texture with clouds to render"),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Lit - Light Color", "CloudCubemapNormalLitColorKey", Color.white, "CubemapNormalCloudFeature", dependsOnValue: true, "Color of the clouds when directly illuminated by a lighting. Typically the top side of the cloud."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Lit - Shadow Color", "CloudCubemapNormalShadowColorKey", Color.gray, "CubemapNormalCloudFeature", dependsOnValue: true, "Color of the clouds when they are in a shadow. Typically the bottom side of the cloud."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Lit - Rotation Speed", "CloudCubemapNormalRotationSpeedKey", -0.5f, 0.5f, 0.1f, "CubemapNormalCloudFeature", dependsOnValue: true, "Speed and direction to rotate the cloud cubemap in."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Lit - Height", "CloudCubemapNormalHeightKey", -1f, 1f, 0f, "CubemapNormalCloudFeature", dependsOnValue: true, "Adjust the horizon height of the cloud cubemap."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Lit - Ambient Intensity", "CloudCubemapNormalAmbientIntensityKey", 0f, 1f, 0.1f, "CubemapNormalCloudFeature", dependsOnValue: true, "Ambient light intensity used for minimum brightness in cloud lighting calculations."),
				ProfileGroupDefinition.TextureGroupDefinition("Cloud Lit - Double Layer Cubemap", "CloudCubemapNormalDoubleLayerCustomTextureKey", null, "CubemapNormalCloudDoubleLayerCubemap", dependsOnValue: true, "Cubemap texture to use as the atmosphere layer for the clouds."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Lit - Double Layer Lit Color", "CloudCubemapNormalDoubleLayerLitColorKey", Color.white, "CubemapNormalCloudDoubleLayerFeature", dependsOnValue: true, "Color of the clouds when directly illuminated by a lighting. Typically the top side of the cloud."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Lit - Double Layer Shadow Color", "CloudCubemapNormalDoubleLayerShadowKey", Color.gray, "CubemapNormalCloudDoubleLayerFeature", dependsOnValue: true, "Color of the clouds when they are in a shadow. Typically the bottom side of the cloud."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Lit - Double Layer Rotation Speed", "CloudCubemapNormalDoubleLayerRotationSpeedKey", -0.5f, 0.5f, 0.15f, "CubemapNormalCloudDoubleLayerFeature", dependsOnValue: true, "Speed and direction to rotate the second layer of clouds."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Lit - Double Layer Height", "CloudCubemapNormalDoubleLayerHeightKey", -1f, 1f, 0f, "CubemapNormalCloudDoubleLayerFeature", dependsOnValue: true, "Adjust the horizon height of the second cloud cubemap layer."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Density", "CloudDensityKey", 0f, 1f, 0.25f, "NoiseCloudFeature", dependsOnValue: true, "Density controls the amount of clouds in the scene."),
				ProfileGroupDefinition.TextureGroupDefinition("Cloud Noise Texture", "CloudNoiseTextureKey", null, "NoiseCloudFeature", dependsOnValue: true, "Noise pattern used to generate the standard procedural clouds from. You can generate your own RGB noise textures with patterns to customize the visual style using photoshop and filters."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Texture Tiling", "CloudTextureTiling", 0.1f, 20f, 0.55f, "NoiseCloudFeature", dependsOnValue: true, "Tiling changes the scale of the texture and how many times it will repeat in the sky. A higher number will increase visible resolution."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Speed", "CloudSpeedKey", 0f, 1f, 0.1f, "NoiseCloudFeature", dependsOnValue: true, "Speed that the clouds move at as a percent from 0 to 1."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Direction", "CloudDirectionKey", 0f, (float)Math.PI * 2f, 1f, "NoiseCloudFeature", dependsOnValue: true, "Direction that the clouds move in as an angle in radians between 0 and 2PI."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Height", "CloudHeightKey", 0f, 1f, 0.7f, "NoiseCloudFeature", dependsOnValue: true, "Height (or altitude) of the clouds in the scene."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Color 1", "CloudColor1Key", Color.white, "NoiseCloudFeature", dependsOnValue: true, "Primary color of the cloud features."),
				ProfileGroupDefinition.ColorGroupDefinition("Cloud Color 2", "CloudColor2Key", Color.gray, "NoiseCloudFeature", dependsOnValue: true, "Secondary color of the clouds between the primary features."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Fade-Out Distance", "CloudFadePositionKey", 0f, 1f, 0.7f, "NoiseCloudFeature", dependsOnValue: true, "Distance at which the clouds will begin to fade away towards the horizon."),
				ProfileGroupDefinition.NumberGroupDefinition("Cloud Fade-Out Amount", "CloudFadeAmountKey", 0f, 1f, 0.75f, "NoiseCloudFeature", dependsOnValue: true, "This is the amount of fade out that will happen to clouds as they reach the horizon.")
			}),
			new ProfileGroupSection("Fog", "FogSectionKey", "FogSectionIcon", "FogFeature", dependsOnValue: true, new ProfileGroupDefinition[4]
			{
				ProfileGroupDefinition.ColorGroupDefinition("Fog Color", "FogColorKey", Color.white, "Color of the fog at the horizon."),
				ProfileGroupDefinition.NumberGroupDefinition("Fog Density", "FogDensityKey", 0f, 1f, 0.12f, "Density, or thickness, of the fog to display at the horizon."),
				ProfileGroupDefinition.NumberGroupDefinition("Fog Height", "FogLengthKey", 0.03f, 1f, 0.1f, "The height of the fog as it extends from the horizon upwards into the sky"),
				ProfileGroupDefinition.BoolGroupDefinition("Update Global Fog Color", "FogSyncWithGlobal", value: true, null, dependsOnValue: false, "Enable this to update the color of global fog, used by the rest of Unity, to match the current sky fog color from Sky Studio.")
			}),
			new ProfileGroupSection("Stars (Mobile)", "StarsBasicSectionKey", "StarSectionIcon", "StarBasicFeature", dependsOnValue: true, new ProfileGroupDefinition[7]
			{
				ProfileGroupDefinition.TextureGroupDefinition("Star Cubemap", "StarBasicCubemapKey", null, ProfileGroupDefinition.RebuildType.None, "StarBasicFeature", dependsOnValue: true, "Cubemap image for basic stars on mobile."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Twinkle Speed", "StarBasicTwinkleSpeedKey", 0f, 10f, 1f, "StarBasicFeature", dependsOnValue: true, "Speed at which the stars will twinkle at."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Twinkle Amount", "StarBasicTwinkleAmountKey", 0f, 1f, 0.75f, "StarBasicFeature", dependsOnValue: true, "The amount of fade out that happens when stars twinkle."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Opacity", "StarBasicOpacityKey", 0f, 1f, 1f, "StarBasicFeature", dependsOnValue: true, "Determines if stars are visible or not. Animate this property on the Sky Timeline to fade stars in/out in day-night cycles."),
				ProfileGroupDefinition.ColorGroupDefinition("Star Tint Color", "StarBasicTintColorKey", Color.white, "StarBasicFeature", dependsOnValue: true, "Tint color of the stars."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Smoothing Exponent", "StarBasicExponentKey", 0.5f, 5f, 1.1f, "Exponent used to smooth out the edges of the star texture."),
				ProfileGroupDefinition.NumberGroupDefinition("Star Bloom Intensity (HDR)", "StarBasicIntensityKey", 1f, 25f, 1f, "Intensity boost that's multiplied against the stars to help them glow with HDR bloom filters.")
			}),
			new ProfileGroupSection("Star Layer 1", "Star1SectionKey", "StarSectionIcon", "StarLayer1Feature", dependsOnValue: true, new ProfileGroupDefinition[13]
			{
				ProfileGroupDefinition.TextureGroupDefinition("Star 1 Texture", "Star1TextureKey", null, ProfileGroupDefinition.RebuildType.None, "StarLayer1CustomTextureFeature", dependsOnValue: true, "Texture used for the star image."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Sprite Columns", "Star1SpriteColumnCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer1SpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Sprite Rows", "Star1SpriteRowCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer1SpriteSheetFeature", dependsOnValue: true, "Number of rows in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Sprite Item Count", "Star1SpriteItemCount", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer1SpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Sprite Animation Speed", "Star1SpriteAnimationSpeed", 0f, 90f, 15f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer1SpriteSheetFeature", dependsOnValue: true, "Frames per second to flip through the sprite images."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Rotation Speed", "Star1RotationSpeed", -10f, 10f, 0f, ProfileGroupDefinition.RebuildType.None, "StarLayer1CustomTextureFeature", dependsOnValue: true, "Speed and direction the star rotates at."),
				ProfileGroupDefinition.ColorGroupDefinition("Star 1 Color", "Star1ColorKey", Color.white, "Color of the star."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Size", "Star1SizeKey", 0f, 0.2f, 0.005f, "Size of the star."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Density", "Star1DensityKey", 0f, 1f, 0.02f, ProfileGroupDefinition.RebuildType.Stars, null, dependsOnValue: false, "Density of the stars in this layer."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Twinkle Amount", "Star1TwinkleAmountKey", 0f, 1f, 0.7f, "Percentage amount of twinkle animation."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Twinkle Speed", "Star1TwinkleSpeedKey", 0f, 10f, 5f, "Speed at which the star twinkles at."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Edge Feathering", "Star1EdgeFeathering", 0.0001f, 1f, 0.4f, "Percentage of fade-in from the stars edges to the center."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 1 Bloom Intensity", "Star1ColorIntensityKey", 1f, 25f, 1f, "Value multiplied with the star color to intensify bloom filters.")
			}),
			new ProfileGroupSection("Star Layer 2", "Star2SectionKey", "StarSectionIcon", "StarLayer2Feature", dependsOnValue: true, new ProfileGroupDefinition[13]
			{
				ProfileGroupDefinition.TextureGroupDefinition("Star 2 Texture", "Star2TextureKey", null, ProfileGroupDefinition.RebuildType.None, "StarLayer2CustomTextureFeature", dependsOnValue: true, "Texture used for the star image."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Sprite Columns", "Star2SpriteColumnCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer2SpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Sprite Rows", "Star2SpriteRowCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer2SpriteSheetFeature", dependsOnValue: true, "Number of rows in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Sprite Item Count", "Star2SpriteItemCount", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer2SpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Sprite Animation Speed", "Star2SpriteAnimationSpeed", 0f, 90f, 15f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer2SpriteSheetFeature", dependsOnValue: true, "Frames per second to flip through the sprite images."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Rotation Speed", "Star2RotationSpeed", -10f, 10f, 0f, ProfileGroupDefinition.RebuildType.None, "StarLayer2CustomTextureFeature", dependsOnValue: true, "Speed and direction the star rotates at."),
				ProfileGroupDefinition.ColorGroupDefinition("Star 2 Color", "Star2ColorKey", Color.white, "Color of the star."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Size", "Star2SizeKey", 0f, 0.2f, 0.005f, "Size of the star."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Density", "Star2DensityKey", 0f, 1f, 0.01f, ProfileGroupDefinition.RebuildType.Stars, null, dependsOnValue: false, "Density of the stars in this layer."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Twinkle Amount", "Star2TwinkleAmountKey", 0f, 1f, 0.7f, "Texture used for the star image."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Twinkle Speed", "Star2TwinkleSpeedKey", 0f, 10f, 5f, "Speed at which the star twinkles at."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Edge Feathering", "Star2EdgeFeathering", 0.0001f, 1f, 0.4f, "Percentage of fade-in from the stars edges to the center."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 2 Bloom Intensity", "Star2ColorIntensityKey", 1f, 25f, 1f, "Value multiplied with the star color to intensify bloom filters.")
			}),
			new ProfileGroupSection("Star Layer 3", "Star3SectionKey", "StarSectionIcon", "StarLayer3Feature", dependsOnValue: true, new ProfileGroupDefinition[13]
			{
				ProfileGroupDefinition.TextureGroupDefinition("Star 3 Texture", "Star3TextureKey", null, ProfileGroupDefinition.RebuildType.None, "StarLayer3CustomTextureFeature", dependsOnValue: true, "Texture used for the star image."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Sprite Columns", "Star3SpriteColumnCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer3SpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Sprite Rows", "Star3SpriteRowCountKey", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer3SpriteSheetFeature", dependsOnValue: true, "Number of rows in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Sprite Item Count", "Star3SpriteItemCount", 1f, 100000f, 1f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer3SpriteSheetFeature", dependsOnValue: true, "Number of columns in the sprite sheet."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Sprite Animation Speed", "Star3SpriteAnimationSpeed", 0f, 90f, 15f, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Integer, "StarLayer3SpriteSheetFeature", dependsOnValue: true, "Frames per second to flip through the sprite images."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Rotation Speed", "Star3RotationSpeed", -10f, 10f, 0f, ProfileGroupDefinition.RebuildType.None, "StarLayer3CustomTextureFeature", dependsOnValue: true, "Speed and direction the star rotates at."),
				ProfileGroupDefinition.ColorGroupDefinition("Star 3 Color", "Star3ColorKey", Color.white, "Color of the star."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Size", "Star3SizeKey", 0f, 0.2f, 0.005f, "Size of the star."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Density", "Star3DensityKey", 0f, 1f, 0.01f, ProfileGroupDefinition.RebuildType.Stars, null, dependsOnValue: false, "Density of the stars in this layer."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Twinkle Amount", "Star3TwinkleAmountKey", 0f, 1f, 0.7f, "Speed at which the star twinkles at."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Twinkle Speed", "Star3TwinkleSpeedKey", 0f, 10f, 5f, "Speed at which the star twinkles at."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Edge Feathering", "Star3EdgeFeathering", 0.0001f, 1f, 0.4f, "Percentage of fade-in from the stars edges to the center."),
				ProfileGroupDefinition.NumberGroupDefinition("Star 3 Bloom Intensity", "Star3ColorIntensityKey", 1f, 25f, 1f, "Value multiplied with the star color to intensify bloom filters.")
			}),
			new ProfileGroupSection("Rain", "RainSectionKey", "RainSectionIcon", "RainFeature", dependsOnValue: true, new ProfileGroupDefinition[12]
			{
				ProfileGroupDefinition.NumberGroupDefinition("Rain Sound Volume", "RainSoundVolume", 0f, 1f, 0.5f, "RainSoundFeature", dependsOnValue: true, "Set the volume of the rain effects."),
				ProfileGroupDefinition.TextureGroupDefinition("Rain Near Texture", "RainNearTextureKey", null, "The rain texture used for animating rain drizzle in the near foreground."),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Near Texture Tiling", "RainNearTextureTiling", 0.01f, 20f, 1f, "Set the tiling amount for the near texture"),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Near Visibility", "RainNearIntensityKey", 0f, 1f, 0.3f, "Set the alpha visibility of the rain downfall effect of the near texture."),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Near Speed", "RainNearSpeedKey", 0f, 5f, 2.5f, "Set the speed of the near rain texture."),
				ProfileGroupDefinition.TextureGroupDefinition("Rain Far Texture", "RainFarTextureKey", null, "The rain texture used for animating rain drizzle in the far background."),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Far Texture Tiling", "RainFarTextureTiling", 0.01f, 20f, 1f, "Set the tiling amount for the far texture"),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Far Visibility", "RainFarIntensityKey", 0f, 1f, 0.15f, "Set the alpha visibility of the rain downfall effect of the far texture."),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Far Speed", "RainFarSpeedKey", 0f, 5f, 2f, "Set the speed of the far rain texture."),
				ProfileGroupDefinition.ColorGroupDefinition("Rain Tint Color", "RainTintColorKey", Color.white, "Tint color that will be multilied against the rain texture color."),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Turbulence", "RainWindTurbulenceKey", 0f, 1f, 0.2f, "Set the amount of jitter in the rain texture animation."),
				ProfileGroupDefinition.NumberGroupDefinition("Rain Turbulence Speed", "RainWindTurbulenceSpeedKey", 0f, 2f, 0.5f, "Set the speed at which the jitter animation is applied to the rain texture")
			}),
			new ProfileGroupSection("Rain Surface Splashes", "RainSplashSectionKey", "RainSplashSectionIcon", "RainSplashFeature", dependsOnValue: true, new ProfileGroupDefinition[8]
			{
				ProfileGroupDefinition.NumberGroupDefinition("Splash Count", "RainSplashMaxConcurrentKey", 0f, 1000f, 400f, "Number of raindrop splashes that will be placed into the scene."),
				ProfileGroupDefinition.NumberGroupDefinition("Splash Area Start", "RainSplashAreaStartKey", 0f, 50f, 1.5f, "The closest distance from the main camera where rain splashes can be spawned at."),
				ProfileGroupDefinition.NumberGroupDefinition("Splash Area Length", "RainSplashAreaLengthKey", 0f, 40f, 5.5f, "The length of the area that rain splashes can happen at, which starts from the Splash Area Offset."),
				ProfileGroupDefinition.NumberGroupDefinition("Splash Scale", "RainSplashScaleKey", 0.001f, 60f, 2.5f, "The size of randrop splashes."),
				ProfileGroupDefinition.NumberGroupDefinition("Splash Scale Varience", "RainSplashScaleVarienceKey", 0f, 1f, 0.25f, "The amount of variety in the initial size of rain splashes so they all don't look exactly the same."),
				ProfileGroupDefinition.NumberGroupDefinition("Splash Intensity", "RainSplashIntensityKey", 0f, 1f, 0.75f, "Lower value makes rain splashes less visible (more transparent), and higher values make splashes more visible."),
				ProfileGroupDefinition.NumberGroupDefinition("Splash Surfce Offset", "RainSplashSurfaceOffsetKey", 0f, 1f, 0.15f, "Offset from the collision surface normal to keep rain splashes above the object they hit."),
				ProfileGroupDefinition.ColorGroupDefinition("Splash Tint Color", "RainSplashTintColorKey", Color.white, "Tint color that will be multilied against rain splash texture.")
			}),
			new ProfileGroupSection("Lightning", "LightningSectionKey", "LightningSectionIcon", "LightningFeature", dependsOnValue: true, new ProfileGroupDefinition[6]
			{
				ProfileGroupDefinition.NumberGroupDefinition("Thunder Sound Volume", "ThunderSoundVolumeKey", 0f, 1f, 0.35f, "ThunderFeature", dependsOnValue: true, "Set the volume of the rain effects."),
				ProfileGroupDefinition.NumberGroupDefinition("Thunder Sound Delay", "ThunderSoundDelayKey", 0f, 20f, 0.4f, "ThunderFeature", dependsOnValue: true, "The amout of time between the lightnig bolt being created and the sound effect playing. Increase to create feeling of distance from lightning."),
				ProfileGroupDefinition.NumberGroupDefinition("Lightning Probability", "LightningProbabilityKey", 0f, 1f, 0.2f, "The probability determines how many lightning bolts will be spawned in the scene."),
				ProfileGroupDefinition.NumberGroupDefinition("Lightning Cool Down", "LightningStrikeCoolDown", 0f, 30f, 2f, "The amount of time that's required to pass after a lighting bolt strike, before another lighting bolt is allowed to be spawned."),
				ProfileGroupDefinition.NumberGroupDefinition("Lightning Intensity", "LightningIntensityKey", 0f, 1f, 1f, "Alpha intensity for the lightnight bolt aniamtion, less intense will create a more transparent and light look."),
				ProfileGroupDefinition.ColorGroupDefinition("Lightning Tint Color", "LightningTintColorKey", Color.white, "Tint color that will be multilied against the lightning texture color.")
			})
		};
	}
}
