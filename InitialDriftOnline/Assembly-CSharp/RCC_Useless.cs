using UnityEngine;

public class RCC_Useless : MonoBehaviour
{
	public enum Useless
	{
		MainController,
		MobileControllers,
		Behavior,
		Graphics
	}

	public Useless useless;

	private void Awake()
	{
		if (useless == Useless.Behavior)
		{
			_ = RCC_Settings.Instance.behaviorSelectedIndex;
		}
		if (useless == Useless.MainController)
		{
			_ = RCC_Settings.Instance.controllerSelectedIndex;
		}
		if (useless == Useless.MobileControllers)
		{
			switch (RCC_Settings.Instance.mobileController)
			{
			}
		}
		if (useless == Useless.Graphics)
		{
			QualitySettings.GetQualityLevel();
		}
	}
}
