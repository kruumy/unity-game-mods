using UnityEngine;

namespace UniStorm.Example;

public class LightningEvent : MonoBehaviour
{
	private void Start()
	{
		UniStormSystem.Instance.OnLightningStrikeObjectEvent.AddListener(delegate
		{
			TestLightningEvent();
		});
	}

	private void TestLightningEvent()
	{
		if (UniStormSystem.Instance.LightningStruckObject != null)
		{
			Debug.Log(UniStormSystem.Instance.LightningStruckObject.name);
		}
	}
}
