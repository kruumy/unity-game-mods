using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleInteractions : MonoBehaviour
{
	public LayerMask layerMask = 1;

	public float interactionDistance = 4f;

	private RaycastHit hit;

	private Transform pickupText;

	private Transform interactionText;

	private HUDNavigationSystem _HUDNavigationSystem;

	private void Start()
	{
		_HUDNavigationSystem = HUDNavigationSystem.Instance;
	}

	private void Update()
	{
		HandleKeyInput();
		HandleItemPickUp();
		HandlePrismColorChange();
	}

	private void HandleKeyInput()
	{
		if (Input.GetKey(KeyCode.X) && _HUDNavigationSystem.radarZoom < 5f)
		{
			_HUDNavigationSystem.radarZoom += 0.0175f;
		}
		else if (Input.GetKey(KeyCode.C) && _HUDNavigationSystem.radarZoom > 0.25f)
		{
			_HUDNavigationSystem.radarZoom -= 0.0175f;
		}
		else if (Input.GetKey(KeyCode.V) && _HUDNavigationSystem.indicatorOffscreenBorder < 0.7f)
		{
			_HUDNavigationSystem.indicatorOffscreenBorder += 0.01f;
		}
		else if (Input.GetKey(KeyCode.B) && _HUDNavigationSystem.indicatorOffscreenBorder > 0.07f)
		{
			_HUDNavigationSystem.indicatorOffscreenBorder -= 0.01f;
		}
		else if (Input.GetKey(KeyCode.N) && _HUDNavigationSystem.minimapScale > 0.06f)
		{
			_HUDNavigationSystem.minimapScale -= 0.0075f;
		}
		else if (Input.GetKey(KeyCode.M) && _HUDNavigationSystem.minimapScale < 0.35f)
		{
			_HUDNavigationSystem.minimapScale += 0.0075f;
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			_HUDNavigationSystem.EnableSystem(!_HUDNavigationSystem.isEnabled);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			_HUDNavigationSystem.EnableRadar(!_HUDNavigationSystem.useRadar);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			_HUDNavigationSystem.EnableCompassBar(!_HUDNavigationSystem.useCompassBar);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			_HUDNavigationSystem.EnableIndicators(!_HUDNavigationSystem.useIndicators);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			_HUDNavigationSystem.EnableMinimap(!_HUDNavigationSystem.useMinimap);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			_HUDNavigationSystem.radarMode = ((_HUDNavigationSystem.radarMode == RadarModes.RotateRadar) ? RadarModes.RotatePlayer : RadarModes.RotateRadar);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			_HUDNavigationSystem.minimapMode = ((_HUDNavigationSystem.minimapMode == MinimapModes.RotateMinimap) ? MinimapModes.RotatePlayer : MinimapModes.RotateMinimap);
		}
		if (Input.GetKeyDown(KeyCode.Alpha7) && _HUDNavigationSystem.currentMinimapProfile != null)
		{
			GameObject customLayer = _HUDNavigationSystem.currentMinimapProfile.GetCustomLayer("exampleLayer");
			if (customLayer != null)
			{
				customLayer.SetActive(!customLayer.activeSelf);
			}
		}
		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				SceneManager.LoadScene(1);
			}
			else
			{
				SceneManager.LoadScene(0);
			}
		}
	}

	private void HandleItemPickUp()
	{
		if (!_HUDNavigationSystem.isEnabled)
		{
			return;
		}
		if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out hit, interactionDistance, layerMask) && hit.collider.name.Contains("PickUp"))
		{
			HUDNavigationElement component = hit.collider.gameObject.GetComponent<HUDNavigationElement>();
			if (!(component != null))
			{
				return;
			}
			if (component.Indicator != null)
			{
				pickupText = component.Indicator.GetCustomTransform("pickupText");
				if (pickupText != null)
				{
					pickupText.gameObject.SetActive(value: true);
				}
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				Object.Destroy(component.gameObject);
			}
		}
		else if (pickupText != null)
		{
			pickupText.gameObject.SetActive(value: false);
			pickupText = null;
		}
	}

	private void HandlePrismColorChange()
	{
		if (!_HUDNavigationSystem.isEnabled)
		{
			return;
		}
		if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out hit, interactionDistance, layerMask) && hit.collider.name.Contains("Prism"))
		{
			HUDNavigationElement componentInChildren = hit.collider.gameObject.GetComponentInChildren<HUDNavigationElement>();
			if (!(componentInChildren != null))
			{
				return;
			}
			if (componentInChildren.Indicator != null)
			{
				interactionText = componentInChildren.Indicator.GetCustomTransform("interactionText");
				if (interactionText != null)
				{
					interactionText.gameObject.SetActive(value: true);
				}
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				Color elementColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
				ChangePrismColor(componentInChildren, elementColor);
			}
		}
		else if (interactionText != null)
		{
			interactionText.gameObject.SetActive(value: false);
			interactionText = null;
		}
	}

	public void SetInitialPrismColor(HUDNavigationElement element)
	{
		Renderer component = element.transform.parent.GetComponent<Renderer>();
		if (component != null)
		{
			ChangePrismColor(element, component.material.color);
		}
	}

	private static void ChangePrismColor(HUDNavigationElement element, Color elementColor)
	{
		if (element.Radar != null)
		{
			element.Radar.ChangeIconColor(elementColor);
		}
		if (element.CompassBar != null)
		{
			element.CompassBar.ChangeIconColor(elementColor);
		}
		if (element.Indicator != null)
		{
			element.Indicator.ChangeIconColor(elementColor);
			element.Indicator.ChangeOffscreenIconColor(elementColor);
		}
		if (element.Minimap != null)
		{
			element.Minimap.ChangeIconColor(elementColor);
		}
		Renderer component = element.transform.parent.GetComponent<Renderer>();
		if (component != null)
		{
			component.material.color = new Color(elementColor.r, elementColor.g, elementColor.b, component.material.color.a);
		}
		Light componentInChildren = element.transform.parent.gameObject.GetComponentInChildren<Light>();
		if (componentInChildren != null)
		{
			componentInChildren.color = new Color(elementColor.r, elementColor.g, elementColor.b);
		}
	}
}
