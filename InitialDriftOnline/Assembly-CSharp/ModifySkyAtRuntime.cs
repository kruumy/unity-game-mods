using Funly.SkyStudio;
using UnityEngine;

public class ModifySkyAtRuntime : MonoBehaviour
{
	[Range(0f, 1f)]
	public float speed = 0.15f;

	private void Update()
	{
		SkyProfile skyProfile = TimeOfDayController.instance.skyProfile;
		ColorKeyframe colorKeyframe = skyProfile.GetGroup<ColorKeyframeGroup>("SkyMiddleColorKey").keyframes[0];
		float h = Time.timeSinceLevelLoad * speed % 1f;
		colorKeyframe.color = Color.HSVToRGB(h, 0.8f, 0.8f);
		skyProfile.GetGroup<ColorKeyframeGroup>("SkyUpperColorKey").keyframes[0].color = colorKeyframe.color;
		TimeOfDayController.instance.UpdateSkyForCurrentTime();
	}
}
