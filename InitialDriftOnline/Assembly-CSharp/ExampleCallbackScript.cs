using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class ExampleCallbackScript : MonoBehaviour
{
	public void ChangeIndicatorColors(HUDNavigationElement element)
	{
		if (element.Indicator != null)
		{
			if (element.Indicator.OnscreenIcon != null)
			{
				element.Indicator.OnscreenIcon.color = Color.magenta;
			}
			if (element.Indicator.OffscreenIcon != null)
			{
				element.Indicator.OffscreenIcon.color = Color.magenta;
			}
		}
	}

	public void OnElementAppeared(HUDNavigationElement element, NavigationElementType type)
	{
		Debug.LogFormat("{0} element of {1} appeared.", type, element.name);
	}

	public void OnElementDisappeared(HUDNavigationElement element, NavigationElementType type)
	{
		Debug.LogFormat("{0} element of {1} disappeared.", type, element.name);
	}

	public void OnElementEnterRadius(HUDNavigationElement element, NavigationElementType type)
	{
		Debug.LogFormat("{0} element of {1} entered radius.", type, element.name);
	}

	public void OnElementLeaveRadius(HUDNavigationElement element, NavigationElementType type)
	{
		Debug.LogFormat("{0} element of {1} left radius.", type, element.name);
	}
}
