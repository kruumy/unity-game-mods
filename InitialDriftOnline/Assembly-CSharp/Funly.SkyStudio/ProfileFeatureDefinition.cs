using System;

namespace Funly.SkyStudio;

[Serializable]
public class ProfileFeatureDefinition
{
	public enum FeatureType
	{
		ShaderKeyword,
		BooleanValue,
		ShaderKeywordDropdown
	}

	public string featureKey;

	public string[] featureKeys;

	public FeatureType featureType;

	public string shaderKeyword;

	public string[] shaderKeywords;

	public string[] dropdownLabels;

	public int dropdownSelectedIndex;

	public string name;

	public bool value;

	public string tooltip;

	public string dependsOnFeature;

	public bool dependsOnValue;

	public bool isShaderKeywordFeature;

	public static ProfileFeatureDefinition CreateShaderFeature(string featureKey, string shaderKeyword, bool value, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
	{
		return new ProfileFeatureDefinition
		{
			featureType = FeatureType.ShaderKeyword,
			featureKey = featureKey,
			shaderKeyword = shaderKeyword,
			name = name,
			value = value,
			tooltip = tooltip,
			dependsOnFeature = dependsOnFeature,
			dependsOnValue = dependsOnValue
		};
	}

	public static ProfileFeatureDefinition CreateShaderFeatureDropdown(string[] featureKeys, string[] shaderKeywords, string[] labels, int selectedIndex, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
	{
		return new ProfileFeatureDefinition
		{
			featureType = FeatureType.ShaderKeywordDropdown,
			featureKeys = featureKeys,
			shaderKeywords = shaderKeywords,
			dropdownLabels = labels,
			name = name,
			dropdownSelectedIndex = selectedIndex,
			tooltip = tooltip,
			dependsOnFeature = dependsOnFeature,
			dependsOnValue = dependsOnValue
		};
	}

	public static ProfileFeatureDefinition CreateBooleanFeature(string featureKey, bool value, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
	{
		return new ProfileFeatureDefinition
		{
			featureType = FeatureType.BooleanValue,
			featureKey = featureKey,
			name = name,
			value = value,
			tooltip = tooltip
		};
	}
}
