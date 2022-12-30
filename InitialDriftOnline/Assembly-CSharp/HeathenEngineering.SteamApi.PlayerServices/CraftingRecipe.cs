using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[CreateAssetMenu(menuName = "Steamworks/Player Services/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
	public List<InventoryItemDefinitionCount> Items;

	public override string ToString()
	{
		string text = "";
		foreach (InventoryItemDefinitionCount item in Items)
		{
			text = text + item.Item.DefinitionID.m_SteamItemDef + "x" + item.Count + ",";
		}
		return text.Remove(text.Length - 1, 1);
	}
}
