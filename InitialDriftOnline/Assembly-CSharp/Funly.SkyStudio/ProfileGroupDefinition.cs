using UnityEngine;

namespace Funly.SkyStudio;

public class ProfileGroupDefinition
{
	public enum GroupType
	{
		None,
		Color,
		Number,
		Texture,
		SpherePoint,
		Boolean
	}

	public enum FormatStyle
	{
		None,
		Integer,
		Float
	}

	public enum RebuildType
	{
		None,
		Stars
	}

	public GroupType type;

	public FormatStyle formatStyle;

	public RebuildType rebuildType;

	public string propertyKey;

	public string groupName;

	public Color color;

	public SpherePoint spherePoint;

	public float minimumValue = -1f;

	public float maximumValue = -1f;

	public float value = -1f;

	public bool boolValue;

	public Texture2D texture;

	public string tooltip;

	public string dependsOnFeature;

	public bool dependsOnValue;

	public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, string tooltip)
	{
		return NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, RebuildType.None, null, dependsOnValue: false, tooltip);
	}

	public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, RebuildType.None, FormatStyle.Float, dependsOnKeyword, dependsOnValue, tooltip);
	}

	public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, rebuildType, FormatStyle.Float, dependsOnKeyword, dependsOnValue, tooltip);
	}

	public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, RebuildType rebuildType, FormatStyle formatStyle, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return new ProfileGroupDefinition
		{
			type = GroupType.Number,
			formatStyle = formatStyle,
			groupName = groupName,
			propertyKey = propKey,
			value = value,
			minimumValue = minimumValue,
			maximumValue = maximumValue,
			tooltip = tooltip,
			rebuildType = rebuildType,
			dependsOnFeature = dependsOnKeyword,
			dependsOnValue = dependsOnValue
		};
	}

	public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, string tooltip)
	{
		return ColorGroupDefinition(groupName, propKey, color, RebuildType.None, null, dependsOnValue: false, tooltip);
	}

	public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, string dependsOnFeature, bool dependsOnValue, string tooltip)
	{
		return ColorGroupDefinition(groupName, propKey, color, RebuildType.None, dependsOnFeature, dependsOnValue, tooltip);
	}

	public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return new ProfileGroupDefinition
		{
			type = GroupType.Color,
			propertyKey = propKey,
			groupName = groupName,
			color = color,
			tooltip = tooltip,
			rebuildType = rebuildType,
			dependsOnFeature = dependsOnKeyword,
			dependsOnValue = dependsOnValue
		};
	}

	public static ProfileGroupDefinition SpherePointGroupDefinition(string groupName, string propKey, float horizontalRotation, float verticalRotation, string tooltip)
	{
		return SpherePointGroupDefinition(groupName, propKey, horizontalRotation, verticalRotation, RebuildType.None, null, dependsOnValue: false, tooltip);
	}

	public static ProfileGroupDefinition SpherePointGroupDefinition(string groupName, string propKey, float horizontalRotation, float verticalRotation, RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return new ProfileGroupDefinition
		{
			type = GroupType.SpherePoint,
			propertyKey = propKey,
			groupName = groupName,
			tooltip = tooltip,
			rebuildType = rebuildType,
			dependsOnFeature = dependsOnKeyword,
			dependsOnValue = dependsOnValue,
			spherePoint = new SpherePoint(horizontalRotation, verticalRotation)
		};
	}

	public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, string tooltip)
	{
		return TextureGroupDefinition(groupName, propKey, texture, RebuildType.None, null, dependsOnValue: false, tooltip);
	}

	public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return TextureGroupDefinition(groupName, propKey, texture, RebuildType.None, dependsOnKeyword, dependsOnValue, tooltip);
	}

	public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return new ProfileGroupDefinition
		{
			type = GroupType.Texture,
			groupName = groupName,
			propertyKey = propKey,
			texture = texture,
			tooltip = tooltip,
			rebuildType = rebuildType,
			dependsOnFeature = dependsOnKeyword,
			dependsOnValue = dependsOnValue
		};
	}

	public static ProfileGroupDefinition BoolGroupDefinition(string groupName, string propKey, bool value, string dependsOnKeyword, bool dependsOnValue, string tooltip)
	{
		return new ProfileGroupDefinition
		{
			type = GroupType.Boolean,
			groupName = groupName,
			propertyKey = propKey,
			dependsOnFeature = dependsOnKeyword,
			dependsOnValue = dependsOnValue,
			tooltip = tooltip,
			boolValue = value
		};
	}
}
