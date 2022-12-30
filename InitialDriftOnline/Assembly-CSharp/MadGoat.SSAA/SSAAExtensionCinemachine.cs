using System;

namespace MadGoat.SSAA;

[Serializable]
public class SSAAExtensionCinemachine : SSAAExtensionBase
{
	public static string description = "Enables support for Unity's Cinemachine\n   • updateFromOriginal - if enabled, the SSAA camera brain settings will be synced with this cameras brain (if available)";

	public static string requirement = "- Requires Cinemachine package\n- Requires Unity 2018.1 or newer";

	private bool cinemachineInstalled;

	public bool updateFromOriginal = true;

	public override bool IsSupported()
	{
		return cinemachineInstalled;
	}

	public override void OnInitialize(MadGoatSSAA ssaaInstance)
	{
		base.OnInitialize(ssaaInstance);
		_ = enabled;
	}

	public override void OnUpdate(MadGoatSSAA ssaaInstance)
	{
		_ = enabled;
	}

	public override void OnDeinitialize(MadGoatSSAA ssaaInstance)
	{
		base.OnDeinitialize(ssaaInstance);
		_ = enabled;
	}
}
