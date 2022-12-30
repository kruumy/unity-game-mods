using UnityEngine;

public class SRTopCam : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject)
		{
			GameObject gameObject = RCC_SceneManager.Instance.activePlayerVehicle.gameObject;
			base.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 80f, gameObject.transform.position.z);
			Debug.Log(base.gameObject.transform.rotation.w);
		}
	}
}
