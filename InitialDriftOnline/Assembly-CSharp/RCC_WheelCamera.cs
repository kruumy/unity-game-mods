using System.Collections;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/RCC Wheel Camera")]
public class RCC_WheelCamera : MonoBehaviour
{
	public void FixShake()
	{
		StartCoroutine(FixShakeDelayed());
	}

	private IEnumerator FixShakeDelayed()
	{
		if ((bool)GetComponent<Rigidbody>())
		{
			yield return new WaitForFixedUpdate();
			GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			yield return new WaitForFixedUpdate();
			GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		}
	}
}
