using UnityEngine;
using UnityEngine.UI;

namespace SickscoreGames.HUDNavigationSystem;

public static class HUDNavigationExtensions
{
	public static float GetDistance(this HUDNavigationElement element, Transform other)
	{
		return Vector2.Distance(new Vector2(element.transform.position.x, element.transform.position.z), new Vector2(other.position.x, other.position.z));
	}

	public static Vector3 GetPosition(this HUDNavigationElement element)
	{
		return element.transform.position;
	}

	public static Vector3 GetPositionOffset(this HUDNavigationElement element, Vector3 otherPosition)
	{
		return element.transform.position - otherPosition;
	}

	public static float GetRadius(this RectTransform rect)
	{
		Vector3[] array = new Vector3[4];
		rect.GetLocalCorners(array);
		float result = Mathf.Abs(array[0].y);
		if (Mathf.Abs(array[0].x) < Mathf.Abs(array[0].y))
		{
			result = Mathf.Abs(array[0].x);
		}
		return result;
	}

	public static Vector3 KeepInRectBounds(this RectTransform rect, Vector3 markerPos, out bool outOfBounds)
	{
		Vector3 vector = markerPos;
		markerPos = Vector3.Min(markerPos, rect.rect.max);
		markerPos = Vector3.Max(markerPos, rect.rect.min);
		outOfBounds = vector != markerPos;
		return markerPos;
	}

	public static float GetIconRadius(this HUDNavigationElement element, NavigationElementType elementType)
	{
		return ((elementType == NavigationElementType.Radar) ? element.Radar.PrefabRect.sizeDelta.x : element.Minimap.PrefabRect.sizeDelta.x) / 2f;
	}

	public static bool IsVisibleOnScreen(this HUDNavigationElement element, Vector3 screenPos)
	{
		if (screenPos.z > 0f && screenPos.x > 0f && screenPos.x < (float)Screen.width && screenPos.y > 0f)
		{
			return screenPos.y < (float)Screen.height;
		}
		return false;
	}

	public static void SetMarkerPosition(this HUDNavigationElement element, NavigationElementType elementType, Vector3 markerPos, RectTransform parentRect = null)
	{
		switch (elementType)
		{
		case NavigationElementType.Radar:
			element.Radar.transform.localPosition = markerPos;
			break;
		case NavigationElementType.CompassBar:
			element.CompassBar.transform.position = new Vector3(markerPos.x + parentRect.localPosition.x, parentRect.position.y, 0f);
			break;
		case NavigationElementType.Minimap:
			element.Minimap.transform.localPosition = markerPos;
			break;
		}
	}

	public static void SetMarkerActive(this HUDNavigationElement element, NavigationElementType elementType, bool value)
	{
		GameObject gameObject = null;
		switch (elementType)
		{
		case NavigationElementType.Radar:
			gameObject = element.Radar.gameObject;
			break;
		case NavigationElementType.CompassBar:
			gameObject = element.CompassBar.gameObject;
			break;
		case NavigationElementType.Minimap:
			gameObject = element.Minimap.gameObject;
			break;
		}
		if (gameObject != null && value != gameObject.activeSelf)
		{
			if (value)
			{
				element.OnAppear.Invoke(element, elementType);
			}
			else
			{
				element.OnDisappear.Invoke(element, elementType);
			}
			gameObject.gameObject.SetActive(value);
		}
	}

	public static void SetMarkerScale(this HUDNavigationElement element, bool scalingEnabled, float distance, float scaleDistance, float minScale, RectTransform prefabRect)
	{
		if (scalingEnabled && !(prefabRect == null))
		{
			float value = (distance - 1f) / (scaleDistance - 1f);
			prefabRect.localScale = Vector2.Lerp(Vector2.one * minScale, Vector2.one, Mathf.Clamp01(value));
		}
	}

	public static void SetMarkerFade(this HUDNavigationElement element, bool fadingEnabled, float distance, float fadeDistance, float minFade, CanvasGroup canvasGroup)
	{
		if (fadingEnabled && !(canvasGroup == null))
		{
			float value = (distance - 1f) / (fadeDistance - 1f);
			canvasGroup.alpha = Mathf.Lerp(1f * minFade, 1f, Mathf.Clamp01(value));
		}
	}

	public static void ShowRadarAboveArrow(this HUDNavigationElement element, bool value)
	{
		if (!(element.Radar.ArrowAbove == null) && value != element.Radar.ArrowAbove.gameObject.activeSelf)
		{
			element.Radar.ArrowAbove.gameObject.SetActive(value);
		}
	}

	public static void ShowRadarBelowArrow(this HUDNavigationElement element, bool value)
	{
		if (!(element.Radar.ArrowBelow == null) && value != element.Radar.ArrowBelow.gameObject.activeSelf)
		{
			element.Radar.ArrowBelow.gameObject.SetActive(value);
		}
	}

	public static void ShowCompassBarDistance(this HUDNavigationElement element, int distance = 0)
	{
		if (!(element.CompassBar.DistanceText == null))
		{
			bool useCompassBarDistanceText = element.useCompassBarDistanceText;
			if (useCompassBarDistanceText != element.CompassBar.DistanceText.gameObject.activeSelf)
			{
				element.CompassBar.DistanceText.gameObject.SetActive(useCompassBarDistanceText);
			}
			if (useCompassBarDistanceText)
			{
				element.CompassBar.DistanceText.text = string.Format(element.compassBarDistanceTextFormat, distance);
			}
		}
	}

	public static void SetIndicatorActive(this HUDNavigationElement element, bool value)
	{
		if (value != element.Indicator.gameObject.activeSelf)
		{
			if (value)
			{
				element.OnAppear.Invoke(element, NavigationElementType.Indicator);
			}
			else
			{
				element.OnDisappear.Invoke(element, NavigationElementType.Indicator);
			}
			element.Indicator.gameObject.SetActive(value);
		}
	}

	public static void ShowIndicatorDistance(this HUDNavigationElement element, bool onScreen, int distance = 0)
	{
		Text text = (onScreen ? element.Indicator.OnscreenDistanceText : element.Indicator.OffscreenDistanceText);
		if (text != null)
		{
			bool flag = (onScreen ? element.useIndicatorDistanceText : (element.useIndicatorDistanceText && element.showOffscreenIndicatorDistance));
			if (flag != text.gameObject.activeSelf)
			{
				text.gameObject.SetActive(flag);
			}
			if (flag)
			{
				text.text = string.Format(onScreen ? element.indicatorOnscreenDistanceTextFormat : element.indicatorOffscreenDistanceTextFormat, distance);
			}
		}
	}

	public static void SetIndicatorOnOffscreen(this HUDNavigationElement element, bool value)
	{
		if (element.Indicator.OnscreenRect != null && value != element.Indicator.OnscreenRect.gameObject.activeSelf)
		{
			element.Indicator.OnscreenRect.gameObject.SetActive(value);
		}
		if (element.Indicator.OffscreenRect != null && !value != element.Indicator.OffscreenRect.gameObject.activeSelf)
		{
			element.Indicator.OffscreenRect.gameObject.SetActive(!value);
		}
	}

	public static void SetIndicatorPosition(this HUDNavigationElement element, Vector3 indicatorPos, RectTransform parentRect = null)
	{
		element.Indicator.transform.position = ((parentRect != null) ? new Vector3(indicatorPos.x + parentRect.localPosition.x, indicatorPos.y + parentRect.localPosition.y, 0f) : indicatorPos);
	}

	public static void SetIndicatorOffscreenRotation(this HUDNavigationElement element, Quaternion rotation)
	{
		if (element.Indicator.OffscreenPointer != null)
		{
			element.Indicator.OffscreenPointer.transform.rotation = rotation;
		}
	}

	public static Vector3 MinimapRotationOffset(this Transform _transform, Vector3 position)
	{
		return position.x * new Vector2(_transform.right.x, 0f - _transform.right.z) + position.y * new Vector2(0f - _transform.forward.x, _transform.forward.z);
	}

	public static void ShowMinimapAboveArrow(this HUDNavigationElement element, bool value)
	{
		if (!(element.Minimap.ArrowAbove == null) && value != element.Minimap.ArrowAbove.gameObject.activeSelf)
		{
			element.Minimap.ArrowAbove.gameObject.SetActive(value);
		}
	}

	public static void ShowMinimapBelowArrow(this HUDNavigationElement element, bool value)
	{
		if (!(element.Minimap.ArrowBelow == null) && value != element.Minimap.ArrowBelow.gameObject.activeSelf)
		{
			element.Minimap.ArrowBelow.gameObject.SetActive(value);
		}
	}

	public static Vector2 GetMapUnitScale(this HNSMapProfile profile)
	{
		return new Vector2(profile.MapTextureSize.x / profile.MapBounds.size.x, profile.MapTextureSize.y / profile.MapBounds.size.z);
	}

	public static float GetMapAspect(this HNSMapProfile profile)
	{
		return profile.MapTextureSize.x / profile.MapTextureSize.y;
	}
}
