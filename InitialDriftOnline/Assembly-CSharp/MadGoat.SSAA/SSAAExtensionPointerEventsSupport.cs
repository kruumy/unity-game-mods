using System;
using UnityEngine;

namespace MadGoat.SSAA;

[Serializable]
public class SSAAExtensionPointerEventsSupport : SSAAExtensionBase
{
	public static string description = "Enables support for Unity's built in pointer events. Select the layers to be affected below. \n(Note: selecting everything or default can cause performance issues)";

	public LayerMask eventsLayerMask;

	public override void OnUpdate(MadGoatSSAA ssaaInstance)
	{
		if (enabled)
		{
			base.OnUpdate(ssaaInstance);
			int cullingMask = ssaaInstance.CurrentCamera.cullingMask;
			cullingMask |= eventsLayerMask.value;
			ssaaInstance.CurrentCamera.cullingMask = cullingMask;
		}
	}
}
