using System.Collections;
using SickscoreGames.HUDNavigationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SRCheckOtherPlayerCam : MonoBehaviour
{
	public GameObject MyTargetPlayer_Go;

	public Text TextPlayerName;

	public string TextPlayerName_string;

	private int State;

	private RCC_CarControllerV3 jack;

	private int hudstate;

	public void SetMyTargetCam()
	{
		State = 0;
		TextPlayerName_string = TextPlayerName.text;
		StartCoroutine(txttostr());
	}

	private IEnumerator txttostr()
	{
		GameObject[] list_player = GameObject.FindGameObjectsWithTag("3DPSEUDO");
		yield return new WaitForSeconds(0.2f);
		TextPlayerName_string = TextPlayerName.text;
		yield return new WaitForSeconds(0.1f);
		GameObject[] array = list_player;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TextMeshPro>().text == TextPlayerName_string && State == 0)
			{
				MyTargetPlayer_Go = gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject;
				jack = gameObject.GetComponentInParent<RCC_CarControllerV3>();
				State = 1;
			}
		}
	}

	public void GoTargetVision()
	{
		if (GetComponent<Image>().enabled && (bool)MyTargetPlayer_Go && RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().speed < 5f && Object.FindObjectOfType<RCC_Camera>().pivot.activeSelf)
		{
			Object.FindObjectOfType<SRUIManager>().CloseMenu();
			Object.FindObjectOfType<RCC_Camera>().cameraMode = RCC_Camera.CameraMode.TPS;
			Object.FindObjectOfType<SRPlayerListRoom>().HUD_RPM_OTHERCAM.SetActive(value: false);
			Object.FindObjectOfType<RCC_Camera>().playerCar = jack;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
			Object.FindObjectOfType<HUDNavigationSystem>().PlayerController = MyTargetPlayer_Go.transform;
			SRID8[] array = Object.FindObjectsOfType<SRID8>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.GetComponent<Camera>().enabled = false;
			}
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().TopCamera.GetComponent<Camera>().enabled = false;
			MyTargetPlayer_Go.GetComponent<SRPlayerFonction>().TopCamera.GetComponent<Camera>().enabled = true;
		}
		Debug.Log("CHANGING PLAYER VISION");
	}

	public void SetMineCam()
	{
		if (Object.FindObjectOfType<RCC_Camera>().enabled)
		{
			SRID8[] array = Object.FindObjectsOfType<SRID8>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.GetComponent<Camera>().enabled = false;
			}
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().TopCamera.GetComponent<Camera>().enabled = true;
			Object.FindObjectOfType<SRPlayerListRoom>().HUD_RPM_OTHERCAM.SetActive(value: true);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;
			Object.FindObjectOfType<RCC_Camera>().playerCar = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>();
			Object.FindObjectOfType<HUDNavigationSystem>().PlayerController = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform;
		}
	}
}
