using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class SRID6 : MonoBehaviour
{
	public ObscuredString a;

	public ObscuredString c;

	public GameObject aaa;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void resetdate()
	{
		Debug.Log("MDRRRRR");
		if ((ObscuredString)PlayerPrefs.GetString("PLAYERNAMEE") == a || (ObscuredString)PlayerPrefs.GetString("PLAYERNAMEE") == c)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
			aaa.SetActive(value: true);
		}
	}
}
