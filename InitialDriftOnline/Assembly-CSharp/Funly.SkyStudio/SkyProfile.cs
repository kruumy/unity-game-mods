using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

[CreateAssetMenu(fileName = "skyProfile.asset", menuName = "Sky Studio/Sky Profile", order = 0)]
public class SkyProfile : ScriptableObject
{
	public const string DefaultShaderName = "Funly/Sky Studio/Skybox/3D Standard";

	public const string DefaultLegacyShaderName = "Funly/Sky Studio/Skybox/3D Standard - Global Keywords";

	[SerializeField]
	private Material m_SkyboxMaterial;

	[SerializeField]
	private string m_ShaderName = "Funly/Sky Studio/Skybox/3D Standard";

	public IProfileDefinition profileDefinition;

	public List<string> timelineManagedKeys = new List<string>();

	public KeyframeGroupDictionary keyframeGroups = new KeyframeGroupDictionary();

	public BoolDictionary featureStatus = new BoolDictionary();

	public LightningArtSet lightningArtSet;

	public RainSplashArtSet rainSplashArtSet;

	public Texture2D starLayer1DataTexture;

	public Texture2D starLayer2DataTexture;

	public Texture2D starLayer3DataTexture;

	[SerializeField]
	private int m_ProfileVersion = 2;

	private Dictionary<string, ProfileGroupDefinition> m_KeyToGroupInfo;

	public Material skyboxMaterial
	{
		get
		{
			return m_SkyboxMaterial;
		}
		set
		{
			if (value == null)
			{
				m_SkyboxMaterial = null;
			}
			else if ((bool)m_SkyboxMaterial && m_SkyboxMaterial.shader.name != value.shader.name)
			{
				m_SkyboxMaterial = value;
				m_ShaderName = value.shader.name;
				ReloadDefinitions();
			}
			else
			{
				m_SkyboxMaterial = value;
			}
		}
	}

	public string shaderName => m_ShaderName;

	public ProfileGroupSection[] groupDefinitions
	{
		get
		{
			if (profileDefinition == null)
			{
				return null;
			}
			return profileDefinition.groups;
		}
	}

	public ProfileFeatureSection[] featureDefinitions
	{
		get
		{
			if (profileDefinition == null)
			{
				return null;
			}
			return profileDefinition.features;
		}
	}

	public float GetNumberPropertyValue(string propertyKey)
	{
		return GetNumberPropertyValue(propertyKey, 0f);
	}

	public float GetNumberPropertyValue(string propertyKey, float timeOfDay)
	{
		NumberKeyframeGroup group = GetGroup<NumberKeyframeGroup>(propertyKey);
		if (group == null)
		{
			Debug.LogError("Can't find number group with property key: " + propertyKey);
			return -1f;
		}
		return group.NumericValueAtTime(timeOfDay);
	}

	public Color GetColorPropertyValue(string propertyKey)
	{
		return GetColorPropertyValue(propertyKey, 0f);
	}

	public Color GetColorPropertyValue(string propertyKey, float timeOfDay)
	{
		ColorKeyframeGroup group = GetGroup<ColorKeyframeGroup>(propertyKey);
		if (group == null)
		{
			Debug.LogError("Can't find color group with property key: " + propertyKey);
			return Color.white;
		}
		return group.ColorForTime(timeOfDay);
	}

	public Texture GetTexturePropertyValue(string propertyKey)
	{
		return GetTexturePropertyValue(propertyKey, 0f);
	}

	public Texture GetTexturePropertyValue(string propertyKey, float timeOfDay)
	{
		TextureKeyframeGroup group = GetGroup<TextureKeyframeGroup>(propertyKey);
		if (group == null)
		{
			Debug.LogError("Can't find texture group with property key: " + propertyKey);
			return null;
		}
		return group.TextureForTime(timeOfDay);
	}

	public SpherePoint GetSpherePointPropertyValue(string propertyKey)
	{
		return GetSpherePointPropertyValue(propertyKey, 0f);
	}

	public SpherePoint GetSpherePointPropertyValue(string propertyKey, float timeOfDay)
	{
		SpherePointKeyframeGroup group = GetGroup<SpherePointKeyframeGroup>(propertyKey);
		if (group == null)
		{
			Debug.LogError("Can't find a sphere point group with property key: " + propertyKey);
			return null;
		}
		return group.SpherePointForTime(timeOfDay);
	}

	public bool GetBoolPropertyValue(string propertyKey)
	{
		return GetBoolPropertyValue(propertyKey, 0f);
	}

	public bool GetBoolPropertyValue(string propertyKey, float timeOfDay)
	{
		BoolKeyframeGroup group = GetGroup<BoolKeyframeGroup>(propertyKey);
		if (group == null)
		{
			Debug.LogError("Can't find boolean group with property key: " + propertyKey);
			return false;
		}
		return group.BoolForTime(timeOfDay);
	}

	public SkyProfile()
	{
		ReloadFullProfile();
	}

	private void OnEnable()
	{
		ReloadFullProfile();
	}

	private void ReloadFullProfile()
	{
		ReloadDefinitions();
		MergeProfileWithDefinitions();
		RebuildKeyToGroupInfoMapping();
		ValidateTimelineGroupKeys();
	}

	private void ReloadDefinitions()
	{
		profileDefinition = GetShaderInfoForMaterial(m_ShaderName);
	}

	private IProfileDefinition GetShaderInfoForMaterial(string shaderName)
	{
		return new Standard3dShaderDefinition();
	}

	public void MergeProfileWithDefinitions()
	{
		MergeGroupsWithDefinitions();
		MergeShaderKeywordsWithDefinitions();
	}

	public void MergeGroupsWithDefinitions()
	{
		HashSet<string> propertyKeysSet = ProfilePropertyKeys.GetPropertyKeysSet();
		ProfileGroupSection[] array = groupDefinitions;
		for (int i = 0; i < array.Length; i++)
		{
			ProfileGroupDefinition[] groups = array[i].groups;
			foreach (ProfileGroupDefinition profileGroupDefinition in groups)
			{
				if (!propertyKeysSet.Contains(profileGroupDefinition.propertyKey))
				{
					continue;
				}
				if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Color)
				{
					if (!keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
					{
						AddColorGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.color);
					}
					else
					{
						keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
					}
				}
				else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Number)
				{
					if (!keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
					{
						AddNumericGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.minimumValue, profileGroupDefinition.maximumValue, profileGroupDefinition.value);
						continue;
					}
					NumberKeyframeGroup group = keyframeGroups.GetGroup<NumberKeyframeGroup>(profileGroupDefinition.propertyKey);
					group.name = profileGroupDefinition.groupName;
					group.minValue = profileGroupDefinition.minimumValue;
					group.maxValue = profileGroupDefinition.maximumValue;
				}
				else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Texture)
				{
					if (!keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
					{
						AddTextureGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.texture);
					}
					else
					{
						keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
					}
				}
				else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.SpherePoint)
				{
					if (!keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
					{
						AddSpherePointGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.spherePoint);
					}
					else
					{
						keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
					}
				}
				else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Boolean)
				{
					if (!keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
					{
						AddBooleanGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.boolValue);
					}
					else
					{
						keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
					}
				}
			}
		}
	}

	public Dictionary<string, ProfileGroupDefinition> GroupDefinitionDictionary()
	{
		ProfileGroupSection[] array = ProfileDefinitionTable();
		Dictionary<string, ProfileGroupDefinition> dictionary = new Dictionary<string, ProfileGroupDefinition>();
		ProfileGroupSection[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ProfileGroupDefinition[] groups = array2[i].groups;
			foreach (ProfileGroupDefinition profileGroupDefinition in groups)
			{
				dictionary.Add(profileGroupDefinition.propertyKey, profileGroupDefinition);
			}
		}
		return dictionary;
	}

	public ProfileGroupSection[] ProfileDefinitionTable()
	{
		return groupDefinitions;
	}

	private void AddNumericGroup(string propKey, string groupName, float min, float max, float value)
	{
		NumberKeyframeGroup value2 = new NumberKeyframeGroup(groupName, min, max, new NumberKeyframe(0f, value));
		keyframeGroups[propKey] = value2;
	}

	private void AddColorGroup(string propKey, string groupName, Color color)
	{
		ColorKeyframeGroup value = new ColorKeyframeGroup(groupName, new ColorKeyframe(color, 0f));
		keyframeGroups[propKey] = value;
	}

	private void AddTextureGroup(string propKey, string groupName, Texture2D texture)
	{
		TextureKeyframeGroup value = new TextureKeyframeGroup(groupName, new TextureKeyframe(texture, 0f));
		keyframeGroups[propKey] = value;
	}

	private void AddSpherePointGroup(string propKey, string groupName, SpherePoint point)
	{
		SpherePointKeyframeGroup value = new SpherePointKeyframeGroup(groupName, new SpherePointKeyframe(point, 0f));
		keyframeGroups[propKey] = value;
	}

	private void AddBooleanGroup(string propKey, string groupName, bool value)
	{
		BoolKeyframeGroup value2 = new BoolKeyframeGroup(groupName, new BoolKeyframe(0f, value));
		keyframeGroups[propKey] = value2;
	}

	public T GetGroup<T>(string propertyKey) where T : class
	{
		if (!keyframeGroups.ContainsKey(propertyKey))
		{
			Debug.Log("Key does not exist in sky profile, ignoring: " + propertyKey);
			return null;
		}
		return keyframeGroups[propertyKey] as T;
	}

	public IKeyframeGroup GetGroup(string propertyKey)
	{
		return keyframeGroups[propertyKey];
	}

	public IKeyframeGroup GetGroupWithId(string groupId)
	{
		if (groupId == null)
		{
			return null;
		}
		foreach (string keyframeGroup2 in keyframeGroups)
		{
			IKeyframeGroup keyframeGroup = keyframeGroups[keyframeGroup2];
			if (keyframeGroup.id == groupId)
			{
				return keyframeGroup;
			}
		}
		return null;
	}

	public ProfileGroupSection[] GetProfileDefinitions()
	{
		return groupDefinitions;
	}

	public ProfileGroupSection GetSectionInfo(string sectionKey)
	{
		ProfileGroupSection[] array = groupDefinitions;
		foreach (ProfileGroupSection profileGroupSection in array)
		{
			if (profileGroupSection.sectionKey == sectionKey)
			{
				return profileGroupSection;
			}
		}
		return null;
	}

	public bool IsManagedByTimeline(string propertyKey)
	{
		return timelineManagedKeys.Contains(propertyKey);
	}

	public void ValidateTimelineGroupKeys()
	{
		List<string> list = new List<string>();
		HashSet<string> propertyKeysSet = ProfilePropertyKeys.GetPropertyKeysSet();
		foreach (string timelineManagedKey in timelineManagedKeys)
		{
			if (!IsManagedByTimeline(timelineManagedKey) || !propertyKeysSet.Contains(timelineManagedKey))
			{
				list.Add(timelineManagedKey);
			}
		}
		foreach (string item in list)
		{
			if (timelineManagedKeys.Contains(item))
			{
				timelineManagedKeys.Remove(item);
			}
		}
	}

	public List<ProfileGroupDefinition> GetGroupDefinitionsManagedByTimeline()
	{
		List<ProfileGroupDefinition> list = new List<ProfileGroupDefinition>();
		foreach (string timelineManagedKey in timelineManagedKeys)
		{
			ProfileGroupDefinition groupDefinitionForKey = GetGroupDefinitionForKey(timelineManagedKey);
			if (groupDefinitionForKey != null)
			{
				list.Add(groupDefinitionForKey);
			}
		}
		return list;
	}

	public List<ProfileGroupDefinition> GetGroupDefinitionsNotManagedByTimeline()
	{
		List<ProfileGroupDefinition> list = new List<ProfileGroupDefinition>();
		ProfileGroupSection[] array = groupDefinitions;
		for (int i = 0; i < array.Length; i++)
		{
			ProfileGroupDefinition[] groups = array[i].groups;
			foreach (ProfileGroupDefinition profileGroupDefinition in groups)
			{
				if (!IsManagedByTimeline(profileGroupDefinition.propertyKey) && CanGroupBeOnTimeline(profileGroupDefinition))
				{
					list.Add(profileGroupDefinition);
				}
			}
		}
		return list;
	}

	public ProfileGroupDefinition GetGroupDefinitionForKey(string propertyKey)
	{
		ProfileGroupDefinition value = null;
		if (m_KeyToGroupInfo.TryGetValue(propertyKey, out value))
		{
			return value;
		}
		return null;
	}

	public void RebuildKeyToGroupInfoMapping()
	{
		m_KeyToGroupInfo = new Dictionary<string, ProfileGroupDefinition>();
		ProfileGroupSection[] array = groupDefinitions;
		for (int i = 0; i < array.Length; i++)
		{
			ProfileGroupDefinition[] groups = array[i].groups;
			foreach (ProfileGroupDefinition profileGroupDefinition in groups)
			{
				m_KeyToGroupInfo[profileGroupDefinition.propertyKey] = profileGroupDefinition;
			}
		}
	}

	public void TrimGroupToSingleKeyframe(string propertyKey)
	{
		GetGroup(propertyKey)?.TrimToSingleKeyframe();
	}

	public bool CanGroupBeOnTimeline(ProfileGroupDefinition definition)
	{
		if (definition.type == ProfileGroupDefinition.GroupType.Texture || (definition.propertyKey.Contains("Star") && definition.propertyKey.Contains("Density")) || definition.propertyKey.Contains("Sprite") || definition.type == ProfileGroupDefinition.GroupType.Boolean)
		{
			return false;
		}
		return true;
	}

	protected void MergeShaderKeywordsWithDefinitions()
	{
		ProfileFeatureSection[] features = profileDefinition.features;
		for (int i = 0; i < features.Length; i++)
		{
			ProfileFeatureDefinition[] array = features[i].featureDefinitions;
			foreach (ProfileFeatureDefinition profileFeatureDefinition in array)
			{
				string text = null;
				bool value = false;
				if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.BooleanValue || profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
				{
					text = profileFeatureDefinition.featureKey;
					value = profileFeatureDefinition.value;
				}
				else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
				{
					text = profileFeatureDefinition.featureKeys[profileFeatureDefinition.dropdownSelectedIndex];
					value = true;
				}
				if (text != null && !featureStatus.dict.ContainsKey(text))
				{
					SetFeatureEnabled(text, value);
				}
			}
		}
	}

	public bool IsFeatureEnabled(string featureKey, bool recursive = true)
	{
		if (featureKey == null)
		{
			return false;
		}
		ProfileFeatureDefinition featureDefinition = profileDefinition.GetFeatureDefinition(featureKey);
		if (featureDefinition == null)
		{
			return false;
		}
		if (!featureStatus.dict.ContainsKey(featureKey) || !featureStatus[featureKey])
		{
			return false;
		}
		if (!recursive)
		{
			return true;
		}
		ProfileFeatureDefinition profileFeatureDefinition = featureDefinition;
		while (profileFeatureDefinition != null)
		{
			ProfileFeatureDefinition featureDefinition2 = profileDefinition.GetFeatureDefinition(profileFeatureDefinition.dependsOnFeature);
			if (featureDefinition2 == null || featureDefinition2.featureKey == null)
			{
				break;
			}
			if (featureStatus[featureDefinition2.featureKey] != profileFeatureDefinition.dependsOnValue)
			{
				return false;
			}
			profileFeatureDefinition = featureDefinition2;
		}
		return true;
	}

	public void SetFeatureEnabled(string featureKey, bool value)
	{
		if (featureKey == null)
		{
			Debug.LogError("Can't set null feature key value");
		}
		else
		{
			featureStatus[featureKey] = value;
		}
	}
}
