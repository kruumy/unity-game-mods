using UnityEngine;

public class SRLightRPC : MonoBehaviour
{
	public Color ColorForTheLight;

	private void Start()
	{
	}

	public void SetColorInChildren(int r, int g, int b, int a)
	{
		ColorForTheLight = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
		RCC_Light[] componentsInChildren = GetComponentsInChildren<RCC_Light>();
		foreach (RCC_Light rCC_Light in componentsInChildren)
		{
			if (rCC_Light.lightType == RCC_Light.LightType.HeadLight || rCC_Light.lightType == RCC_Light.LightType.HighBeamHeadLight)
			{
				rCC_Light.GetComponent<Light>().color = ColorForTheLight;
			}
		}
	}

	private void Update()
	{
	}
}
