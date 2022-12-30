using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buttonkey : MonoBehaviour
{
	public KeyCode Touche1;

	public KeyCode Touche2;

	public KeyCode Touche3PS4;

	private Button _button;

	public int state;

	public int tempologii;

	public GameObject[] HideUnhide;

	private void Awake()
	{
		_button = GetComponent<Button>();
		state = 1;
		tempologii = 0;
	}

	private void Update()
	{
		if (Input.GetKeyDown(Touche1) || (Input.GetKeyDown(Touche2) && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (RCC_LogitechSteeringWheel.GetKeyPressed(0, 7) && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel" && tempologii == 0) || (Input.GetButtonDown("PS4_Share") && PlayerPrefs.GetString("ControllerTypeChoose") == "PS4" && tempologii == 0))
		{
			SetHud();
		}
	}

	private IEnumerator Tempoo()
	{
		yield return new WaitForSeconds(0.4f);
		tempologii = 0;
	}

	public void SetHud()
	{
		GameObject[] hideUnhide;
		PhotonView[] array;
		if (state == 0)
		{
			tempologii = 1;
			StartCoroutine(Tempoo());
			state = 1;
			hideUnhide = HideUnhide;
			for (int i = 0; i < hideUnhide.Length; i++)
			{
				hideUnhide[i].transform.gameObject.SetActive(value: true);
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<Mask>().gameObject.GetComponent<Camera>().enabled = true;
				PlayerPrefs.SetInt("HUDOFF", 0);
			}
			array = Object.FindObjectsOfType<PhotonView>();
			foreach (PhotonView photonView in array)
			{
				if (!photonView.IsMine)
				{
					GameObject.Find(photonView.gameObject.transform.name + "/Text (TMP)").GetComponent<TextMeshPro>().enabled = true;
				}
			}
			return;
		}
		tempologii = 1;
		StartCoroutine(Tempoo());
		state = 0;
		hideUnhide = HideUnhide;
		for (int i = 0; i < hideUnhide.Length; i++)
		{
			hideUnhide[i].transform.gameObject.SetActive(value: false);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<Mask>().gameObject.GetComponent<Camera>().enabled = false;
			PlayerPrefs.SetInt("HUDOFF", 1);
		}
		array = Object.FindObjectsOfType<PhotonView>();
		foreach (PhotonView photonView2 in array)
		{
			if (!photonView2.IsMine)
			{
				GameObject.Find(photonView2.gameObject.transform.name + "/Text (TMP)").GetComponent<TextMeshPro>().enabled = false;
			}
		}
	}
}
