using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Visual Dashboard Objects")]
public class RCC_DashboardObjects : MonoBehaviour
{
	[Serializable]
	public class RPMDial
	{
		public GameObject dial;

		public float multiplier = 0.05f;

		public RotateAround rotateAround = RotateAround.Z;

		private Quaternion dialOrgRotation = Quaternion.identity;

		public Text text;

		public void Init()
		{
			if ((bool)dial)
			{
				dialOrgRotation = dial.transform.localRotation;
			}
		}

		public void Update(float value)
		{
			Vector3 axis = Vector3.forward;
			switch (rotateAround)
			{
			case RotateAround.X:
				axis = Vector3.right;
				break;
			case RotateAround.Y:
				axis = Vector3.up;
				break;
			case RotateAround.Z:
				axis = Vector3.forward;
				break;
			}
			dial.transform.localRotation = dialOrgRotation * Quaternion.AngleAxis((0f - multiplier) * value, axis);
			if ((bool)text)
			{
				text.text = value.ToString("F0");
			}
		}
	}

	[Serializable]
	public class SpeedoMeterDial
	{
		public GameObject dial;

		public float multiplier = 1f;

		public RotateAround rotateAround = RotateAround.Z;

		private Quaternion dialOrgRotation = Quaternion.identity;

		public Text text;

		public TextMeshPro textspeed;

		public void Init()
		{
			if ((bool)dial)
			{
				dialOrgRotation = dial.transform.localRotation;
			}
		}

		public void Update(float value)
		{
			Vector3 axis = Vector3.forward;
			switch (rotateAround)
			{
			case RotateAround.X:
				axis = Vector3.right;
				break;
			case RotateAround.Y:
				axis = Vector3.up;
				break;
			case RotateAround.Z:
				axis = Vector3.forward;
				break;
			}
			dial.transform.localRotation = dialOrgRotation * Quaternion.AngleAxis((0f - multiplier) * value, axis);
			if ((bool)text)
			{
				text.text = value.ToString("F0");
			}
			if ((bool)textspeed)
			{
				textspeed.text = value.ToString("F0");
			}
		}
	}

	[Serializable]
	public class FuelDial
	{
		public GameObject dial;

		public float multiplier = 0.1f;

		public RotateAround rotateAround = RotateAround.Z;

		private Quaternion dialOrgRotation = Quaternion.identity;

		public Text text;

		public void Init()
		{
			if ((bool)dial)
			{
				dialOrgRotation = dial.transform.localRotation;
			}
		}

		public void Update(float value)
		{
			Vector3 axis = Vector3.forward;
			switch (rotateAround)
			{
			case RotateAround.X:
				axis = Vector3.right;
				break;
			case RotateAround.Y:
				axis = Vector3.up;
				break;
			case RotateAround.Z:
				axis = Vector3.forward;
				break;
			}
			dial.transform.localRotation = dialOrgRotation * Quaternion.AngleAxis((0f - multiplier) * value, axis);
			if ((bool)text)
			{
				text.text = value.ToString("F0");
			}
		}
	}

	[Serializable]
	public class HeatDial
	{
		public GameObject dial;

		public float multiplier = 0.1f;

		public RotateAround rotateAround = RotateAround.Z;

		private Quaternion dialOrgRotation = Quaternion.identity;

		public Text text;

		public void Init()
		{
			if ((bool)dial)
			{
				dialOrgRotation = dial.transform.localRotation;
			}
		}

		public void Update(float value)
		{
			Vector3 axis = Vector3.forward;
			switch (rotateAround)
			{
			case RotateAround.X:
				axis = Vector3.right;
				break;
			case RotateAround.Y:
				axis = Vector3.up;
				break;
			case RotateAround.Z:
				axis = Vector3.forward;
				break;
			}
			dial.transform.localRotation = dialOrgRotation * Quaternion.AngleAxis((0f - multiplier) * value, axis);
			if ((bool)text)
			{
				text.text = value.ToString("F0");
			}
		}
	}

	[Serializable]
	public class InteriorLight
	{
		public Light light;

		public float intensity = 1f;

		public LightRenderMode renderMode;

		public void Init()
		{
			if (RCC_Settings.Instance.useLightsAsVertexLights)
			{
				renderMode = LightRenderMode.ForceVertex;
			}
			else
			{
				renderMode = LightRenderMode.ForcePixel;
			}
		}

		public void Update(bool state)
		{
			if (!light.enabled)
			{
				light.enabled = true;
			}
			light.intensity = (state ? intensity : 0f);
		}
	}

	public enum RotateAround
	{
		X,
		Y,
		Z
	}

	private RCC_Settings RCCSettingsInstance;

	private RCC_CarControllerV3 carController;

	[Space]
	public RPMDial rPMDial;

	[Space]
	public SpeedoMeterDial speedDial;

	[Space]
	public FuelDial fuelDial;

	[Space]
	public HeatDial heatDial;

	[Space]
	public InteriorLight[] interiorLights;

	private RCC_Settings RCCSettings
	{
		get
		{
			if (RCCSettingsInstance == null)
			{
				RCCSettingsInstance = RCC_Settings.Instance;
				return RCCSettingsInstance;
			}
			return RCCSettingsInstance;
		}
	}

	private void Awake()
	{
		carController = GetComponentInParent<RCC_CarControllerV3>();
		rPMDial.Init();
		speedDial.Init();
		fuelDial.Init();
		heatDial.Init();
		for (int i = 0; i < interiorLights.Length; i++)
		{
			interiorLights[i].Init();
		}
	}

	private void Update()
	{
		if ((bool)carController)
		{
			Dials();
			Lights();
		}
	}

	private void Dials()
	{
		if (rPMDial.dial != null)
		{
			rPMDial.Update(carController.engineRPM);
		}
		if (speedDial.dial != null)
		{
			speedDial.Update(carController.speed);
		}
		if (fuelDial.dial != null)
		{
			fuelDial.Update(carController.fuelTank);
		}
		if (heatDial.dial != null)
		{
			heatDial.Update(carController.engineHeat);
		}
	}

	private void Lights()
	{
		for (int i = 0; i < interiorLights.Length; i++)
		{
			interiorLights[i].Update(carController.lowBeamHeadLightsOn);
		}
	}
}
