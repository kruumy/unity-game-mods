using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[CreateAssetMenu(menuName = "Steamworks/Player Services/Inventory Tag Generator")]
public class TagGeneratorDefinition : ScriptableObject
{
	public SteamItemDef_t DefinitionID;

	public string TagName;

	public List<TagGeneratorValue> TagValues;
}
