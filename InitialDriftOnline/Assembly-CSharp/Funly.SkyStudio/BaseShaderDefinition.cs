using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public abstract class BaseShaderDefinition : IProfileDefinition
{
	private ProfileGroupSection[] m_ProfileDefinitions;

	[SerializeField]
	private ProfileFeatureSection[] m_ProfileFeatures;

	private Dictionary<string, ProfileFeatureDefinition> m_KeyToFeature;

	public string shaderName { get; protected set; }

	public ProfileGroupSection[] groups => m_ProfileDefinitions ?? (m_ProfileDefinitions = ProfileDefinitionTable());

	public ProfileFeatureSection[] features => m_ProfileFeatures ?? (m_ProfileFeatures = ProfileFeatureSection());

	public ProfileFeatureDefinition GetFeatureDefinition(string featureKey)
	{
		if (m_KeyToFeature == null)
		{
			m_KeyToFeature = new Dictionary<string, ProfileFeatureDefinition>();
			ProfileFeatureSection[] array = features;
			for (int i = 0; i < array.Length; i++)
			{
				ProfileFeatureDefinition[] featureDefinitions = array[i].featureDefinitions;
				foreach (ProfileFeatureDefinition profileFeatureDefinition in featureDefinitions)
				{
					if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.BooleanValue || profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
					{
						m_KeyToFeature[profileFeatureDefinition.featureKey] = profileFeatureDefinition;
					}
					else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
					{
						string[] featureKeys = profileFeatureDefinition.featureKeys;
						foreach (string key in featureKeys)
						{
							m_KeyToFeature[key] = profileFeatureDefinition;
						}
					}
				}
			}
		}
		if (featureKey == null)
		{
			return null;
		}
		if (!m_KeyToFeature.ContainsKey(featureKey))
		{
			return null;
		}
		return m_KeyToFeature[featureKey];
	}

	protected abstract ProfileFeatureSection[] ProfileFeatureSection();

	protected abstract ProfileGroupSection[] ProfileDefinitionTable();
}
