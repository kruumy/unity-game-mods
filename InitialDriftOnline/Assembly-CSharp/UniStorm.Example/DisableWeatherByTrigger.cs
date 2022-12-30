using UnityEngine;

namespace UniStorm.Example;

public class DisableWeatherByTrigger : MonoBehaviour
{
	public enum ControlEffectsEnum
	{
		Disable,
		Enable
	}

	public enum ControlSoundsEnum
	{
		Yes,
		No
	}

	public string TriggerTag = "Player";

	public ControlEffectsEnum ControlEffects;

	public ControlSoundsEnum ControlSounds;

	private void OnTriggerEnter(Collider C)
	{
		if (C.tag == TriggerTag && ControlEffects == ControlEffectsEnum.Disable)
		{
			UniStormManager.Instance.ChangeWeatherEffectsState(ActiveState: false);
			if (ControlSounds == ControlSoundsEnum.Yes)
			{
				UniStormManager.Instance.ChangeWeatherSoundsState(ActiveState: false);
			}
		}
		else if (C.tag == TriggerTag && ControlEffects == ControlEffectsEnum.Enable)
		{
			UniStormManager.Instance.ChangeWeatherEffectsState(ActiveState: true);
			if (ControlSounds == ControlSoundsEnum.Yes)
			{
				UniStormManager.Instance.ChangeWeatherSoundsState(ActiveState: true);
			}
		}
	}
}
