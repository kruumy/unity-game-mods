using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class ValveItemDefAttribute
{
	public ValveItemDefSchemaAttributes attribute;

	public ValveItemDefLanguages language;

	public string stringValue = "";

	public bool boolValue;

	public Color colorValue;

	public int intValue;

	public List<string> stringArray;

	public ValveItemDefPriceData priceDataValue;

	public ValveItemDefPriceCategory priceCategoryValue;

	public List<ValveItemDefPromoRule> promoRulesValue;

	public List<ValveItemDefInventoryItemTag> inventoryTagValue;

	public List<TagGeneratorDefinition> tagGeneratorValue;

	public override string ToString()
	{
		switch (attribute)
		{
		case ValveItemDefSchemaAttributes.background_color:
		{
			Color color2 = colorValue;
			return "\"background_color\": \"" + ColorUtility.ToHtmlStringRGB(color2) + "\"";
		}
		case ValveItemDefSchemaAttributes.description:
			return "\"description" + GetLanguageSuffix() + "\": \"" + stringValue.Replace("\n\r", "\\n").Replace("\n", "\\n").Replace("\r", "\\n") + "\"";
		case ValveItemDefSchemaAttributes.display_type:
			return "\"display_type" + GetLanguageSuffix() + "\": \"" + stringValue.Replace("\n\r", "\\n").Replace("\n", "\\n").Replace("\r", "\\n") + "\"";
		case ValveItemDefSchemaAttributes.drop_interval:
			return "\"drop_interval\": " + intValue;
		case ValveItemDefSchemaAttributes.drop_limit:
			return "\"drop_limit\": " + intValue;
		case ValveItemDefSchemaAttributes.drop_max_per_winidow:
			return "\"drop_max_per_window\": " + intValue;
		case ValveItemDefSchemaAttributes.drop_start_time:
			return "\"drop_start_time\": \"" + stringValue + "\"";
		case ValveItemDefSchemaAttributes.drop_window:
			return "\"drop_window\": " + intValue;
		case ValveItemDefSchemaAttributes.granted_manually:
			return "\"granted_manually\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.hidden:
			return "\"hidden\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.icon_url:
			return "\"icon_url\": \"" + stringValue.Replace("\n\r", "").Replace("\n", "").Replace("\r", "") + "\"";
		case ValveItemDefSchemaAttributes.icon_url_large:
			return "\"icon_url_large\": \"" + stringValue.Replace("\n\r", "").Replace("\n", "").Replace("\r", "") + "\"";
		case ValveItemDefSchemaAttributes.item_quality:
			return "\"item_quality\": " + intValue;
		case ValveItemDefSchemaAttributes.item_slot:
			return "\"item_slot\": \"" + stringValue + "\"";
		case ValveItemDefSchemaAttributes.marketable:
			return "\"marketable\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.name:
			return "\"name" + GetLanguageSuffix() + "\": \"" + stringValue.Replace("\n\r", "\\n").Replace("\n", "\\n").Replace("\r", "\\n") + "\"";
		case ValveItemDefSchemaAttributes.name_color:
		{
			Color color = colorValue;
			return "\"name_color\": \"" + ColorUtility.ToHtmlStringRGB(color) + "\"";
		}
		case ValveItemDefSchemaAttributes.price:
		{
			ValveItemDefPriceData valveItemDefPriceData = priceDataValue;
			if (valveItemDefPriceData != null)
			{
				return "\"price\": \"" + valveItemDefPriceData.ToString() + "\"";
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.price_category:
		{
			ValveItemDefPriceCategory valveItemDefPriceCategory = priceCategoryValue;
			if (valveItemDefPriceCategory != null)
			{
				return "\"price\": \"" + valveItemDefPriceCategory.ToString() + "\"";
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.promo:
		{
			List<ValveItemDefPromoRule> list5 = promoRulesValue;
			if (list5 != null)
			{
				StringBuilder stringBuilder5 = new StringBuilder("\"");
				foreach (ValveItemDefPromoRule item in list5)
				{
					if (stringBuilder5.Length > 1)
					{
						stringBuilder5.Append(";");
					}
					stringBuilder5.Append(item.ToString());
				}
				stringBuilder5.Append("\"");
				return "\"promo\": " + stringBuilder5.ToString();
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.store_hidden:
			return "\"store_hidden\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.store_images:
		{
			List<string> list4 = stringArray;
			if (list4 != null)
			{
				StringBuilder stringBuilder4 = new StringBuilder("\"");
				foreach (string item2 in list4)
				{
					if (stringBuilder4.Length > 1)
					{
						stringBuilder4.Append(";");
					}
					stringBuilder4.Append(item2);
				}
				stringBuilder4.Append("\"");
				return "\"store_images\": \"" + stringBuilder4.ToString() + "\"";
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.store_tags:
		{
			List<string> list3 = stringArray;
			if (list3 != null)
			{
				StringBuilder stringBuilder3 = new StringBuilder("\"");
				foreach (string item3 in list3)
				{
					if (stringBuilder3.Length > 1)
					{
						stringBuilder3.Append(";");
					}
					stringBuilder3.Append(item3);
				}
				stringBuilder3.Append("\"");
				return "\"store_tags\": " + stringBuilder3.ToString();
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.tags:
		{
			List<ValveItemDefInventoryItemTag> list2 = inventoryTagValue;
			if (list2 != null)
			{
				StringBuilder stringBuilder2 = new StringBuilder("\"");
				foreach (ValveItemDefInventoryItemTag item4 in list2)
				{
					if (stringBuilder2.Length > 1)
					{
						stringBuilder2.Append(";");
					}
					stringBuilder2.Append(item4.ToString());
				}
				stringBuilder2.Append("\"");
				return "\"tags\": " + stringBuilder2.ToString();
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.tag_generators:
		{
			List<TagGeneratorDefinition> list = tagGeneratorValue;
			if (list != null)
			{
				StringBuilder stringBuilder = new StringBuilder("\"");
				foreach (TagGeneratorDefinition item5 in list)
				{
					if (stringBuilder.Length > 1)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(item5.DefinitionID.m_SteamItemDef);
				}
				stringBuilder.Append("\"");
				return "\"tag_generators\": " + stringBuilder.ToString();
			}
			return string.Empty;
		}
		case ValveItemDefSchemaAttributes.tradable:
			return "\"tradable\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.use_bundle_price:
			return "\"use_bundle_price\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.use_drop_limit:
			return "\"use_drop_limit\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.use_drop_window:
			return "\"use_drop_limit\": " + boolValue.ToString().ToLower();
		case ValveItemDefSchemaAttributes.purchase_bundle_discount:
			return "\"purchase_bundle_discount\": " + intValue;
		default:
			return string.Empty;
		}
	}

	private string GetLanguageSuffix()
	{
		if (language == ValveItemDefLanguages.none)
		{
			return string.Empty;
		}
		return "_" + language;
	}
}
