using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.Foundation;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SRPlayerListRoom : MonoBehaviourPunCallbacks
{
	public GameObject PlayerListUI;

	public SteamUserData userDataa;

	private string playernameacutal;

	public GameObject[] CaseParPlayer;

	public GameObject[] PlayerListGO;

	public GameObject[] IconCamera;

	public bool EnableBg;

	public Sprite BackGroundOfIcon;

	[Space]
	public SRCheckOtherPlayerCam[] VisionPlayer;

	public GameObject HUD_RPM_OTHERCAM;

	public Animator TutoCam;

	private void Start()
	{
		PlayerListUI.SetActive(value: false);
		new SRPlayerListRoom();
	}

	public void UIGestion()
	{
		GameObject[] caseParPlayer = CaseParPlayer;
		foreach (GameObject obj in caseParPlayer)
		{
			obj.GetComponentInChildren<Text>().text = "";
			obj.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 110);
			obj.GetComponentInChildren<IDHome>().transform.gameObject.GetComponent<Image>().sprite = BackGroundOfIcon;
			obj.GetComponentInChildren<IDHome>().transform.gameObject.GetComponent<Image>().enabled = false;
			obj.GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().enabled = false;
		}
		caseParPlayer = IconCamera;
		for (int i = 0; i < caseParPlayer.Length; i++)
		{
			caseParPlayer[i].SetActive(value: false);
		}
	}

	public void SteamIcon()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Joystick1Button8) && ObscuredPrefs.GetBool("TooglePlayerListXbox") && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (Input.GetButtonDown("PS4_ClickLeft") && ObscuredPrefs.GetBool("TooglePlayerListXbox") && PlayerPrefs.GetString("ControllerTypeChoose") == "PS4"))
		{
			PlayerListUI.SetActive(value: true);
			SteamIcon();
			PlayerListing();
		}
		if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.Joystick1Button8) || (Input.GetButtonUp("PS4_ClickLeft") && PlayerPrefs.GetString("ControllerTypeChoose") == "PS4"))
		{
			PlayerListUI.SetActive(value: false);
		}
	}

	public void PlayerListing()
	{
		UIGestion();
		TutoCam.Play("Playerlist_showtuto");
		PlayerListGO = GameObject.FindGameObjectsWithTag("3DPSEUDO");
		VisionPlayer = UnityEngine.Object.FindObjectsOfType<SRCheckOtherPlayerCam>();
		SRCheckOtherPlayerCam[] visionPlayer = VisionPlayer;
		for (int i = 0; i < visionPlayer.Length; i++)
		{
			visionPlayer[i].SetMyTargetCam();
		}
		if (EnableBg)
		{
			CaseParPlayer[0].GetComponentInChildren<IDHome>().gameObject.GetComponent<Image>().enabled = true;
		}
		CaseParPlayer[0].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().enabled = true;
		CaseParPlayer[0].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().sprite = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().MyIcon;
		CaseParPlayer[0].GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        // OLD: CaseParPlayer[0].GetComponentInChildren<Text>().text = "<color=#ACACAC>[</color><color=#F3E400>" + ObscuredPrefs.GetInt("MyLvl") + "</color><color=#ACACAC>]</color> " + PlayerPrefs.GetString("PLAYERNAMEE");
		if(File.ReadAllLines("Settings.txt")[3].Split('=')[1] == "true")
			CaseParPlayer[0].GetComponentInChildren<Text>().text = "<color=#ACACAC>[</color><color=#F3E400>" + 42069 + "</color><color=#ACACAC>]</color> " + PlayerPrefs.GetString("PLAYERNAMEE");
		else
            CaseParPlayer[0].GetComponentInChildren<Text>().text = "<color=#ACACAC>[</color><color=#F3E400>" + ObscuredPrefs.GetInt("MyLvl") + "</color><color=#ACACAC>]</color> " + PlayerPrefs.GetString("PLAYERNAMEE");
        int num = 0;
		GameObject[] playerListGO = PlayerListGO;
		foreach (GameObject gameObject in playerListGO)
		{
			num++;
			CaseParPlayer[num].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().enabled = true;
			Sprite myIcon = gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponentInChildren<SkinManager>().MyIcon;
			CaseParPlayer[num].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().sprite = myIcon;
			if (EnableBg)
			{
				CaseParPlayer[num].GetComponentInChildren<IDHome>().gameObject.GetComponent<Image>().enabled = true;
			}
			playernameacutal = gameObject.GetComponent<TextMeshPro>().text;
			CaseParPlayer[num].GetComponentInChildren<Text>().text = playernameacutal;
			CaseParPlayer[num].GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			CaseParPlayer[num].GetComponentInChildren<SRID7>().GetComponent<Text>().text = gameObject.GetComponentInChildren<Text>().text;
		}
		StartCoroutine(Refresh());
	}

	public void Picid()
	{
		Convert.ToUInt64(RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<TextMeshPro>().transform.gameObject.GetComponentInChildren<Text>().text.ToString());
	}

	private IEnumerator Refresh()
	{
		yield return new WaitForSeconds(0.2f);
		PlayerListingRefresh();
	}

	public void PlayerListingRefresh()
	{
		UIGestion();
		PlayerListGO = GameObject.FindGameObjectsWithTag("3DPSEUDO");
		if (EnableBg)
		{
			CaseParPlayer[0].GetComponentInChildren<IDHome>().gameObject.GetComponent<Image>().enabled = true;
		}
		CaseParPlayer[0].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().enabled = true;
		CaseParPlayer[0].GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        // OLD: CaseParPlayer[0].GetComponentInChildren<Text>().text = "<color=#ACACAC>[</color><color=#F3E400>" + ObscuredPrefs.GetInt("MyLvl") + "</color><color=#ACACAC>]</color> " + PlayerPrefs.GetString("PLAYERNAMEE");
        if (File.ReadAllLines("Settings.txt")[3].Split('=')[1] == "true")
            CaseParPlayer[0].GetComponentInChildren<Text>().text = "<color=#ACACAC>[</color><color=#F3E400>" + 42069 + "</color><color=#ACACAC>]</color> " + PlayerPrefs.GetString("PLAYERNAMEE");
        else
            CaseParPlayer[0].GetComponentInChildren<Text>().text = "<color=#ACACAC>[</color><color=#F3E400>" + ObscuredPrefs.GetInt("MyLvl") + "</color><color=#ACACAC>]</color> " + PlayerPrefs.GetString("PLAYERNAMEE");
        int num = 0;
		GameObject[] playerListGO = PlayerListGO;
		foreach (GameObject gameObject in playerListGO)
		{
			num++;
			CaseParPlayer[num].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().enabled = true;
			Sprite myIcon = gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponentInChildren<SkinManager>().MyIcon;
			CaseParPlayer[num].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().transform.gameObject.GetComponent<Image>().sprite = myIcon;
			if (EnableBg)
			{
				CaseParPlayer[num].GetComponentInChildren<IDHome>().gameObject.GetComponent<Image>().enabled = true;
			}
			playernameacutal = gameObject.GetComponent<TextMeshPro>().text;
			CaseParPlayer[num].GetComponentInChildren<Text>().text = playernameacutal;
			CaseParPlayer[num].GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	public void GoLink(GameObject LinkGO)
	{
		string text = LinkGO.GetComponent<Text>().text;
		if (text == "Empty")
		{
			Application.OpenURL("https://store.steampowered.com/app/1456200/Initial_Drift_Online/?curator_clanid=30833987");
		}
		else
		{
			Application.OpenURL("https://steamcommunity.com/profiles/" + text);
		}
	}
}
