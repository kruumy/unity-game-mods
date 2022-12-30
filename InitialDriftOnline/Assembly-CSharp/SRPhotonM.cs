using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class SRPhotonM : MonoBehaviourPunCallbacks
{
	public GameObject FadeUI;

	public Text[] RoomName;

	public Text[] RoomPlayerCount;

	public string Mapname;

	public GameObject CarsCam;

	public Button First;

	private ScrollRect SRR;

	public int Irocount;

	public int harunacount;

	public int akagicount;

	public int usuicount;

	public int[] Irocountdetail = new int[6];

	public int[] harunacountdetail = new int[6];

	public int[] akagicountdetail = new int[6];

	public int[] usuicountdetail = new int[6];

	private void Start()
	{
		CarsCam.SetActive(value: false);
		SRR = GetComponentInChildren<ScrollRect>();
		First.Select();
		First.interactable = false;
		First.Select();
		First.interactable = true;
		First.Select();
		if ((bool)GetComponent<SRcontrollerSelector>())
		{
			GetComponent<SRcontrollerSelector>().SetDropValueInServerList();
		}
	}

	private void Update()
	{
		Cursor.visible = true;
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);
		Irocount = 0;
		harunacount = 0;
		akagicount = 0;
		usuicount = 0;
		foreach (RoomInfo room in roomList)
		{
			for (int i = 1; i < RoomName.Length; i++)
			{
				if (room.Name == Mapname + i)
				{
					RoomName[i].text = room.Name;
					RoomPlayerCount[i].text = "[" + room.PlayerCount + " / 16]";
				}
				if (room.Name == "Irohazaka" + i)
				{
					Irocountdetail[i] = room.PlayerCount;
				}
				if (room.Name == "HARUNA" + i)
				{
					harunacountdetail[i] = room.PlayerCount;
				}
				if (room.Name == "Akagi" + i)
				{
					akagicountdetail[i] = room.PlayerCount;
				}
				if (room.Name == "USUI" + i)
				{
					usuicountdetail[i] = room.PlayerCount;
				}
			}
		}
		for (int j = 1; j < 11; j++)
		{
			Irocount += Irocountdetail[j];
			harunacount += harunacountdetail[j];
			akagicount += akagicountdetail[j];
			usuicount += usuicountdetail[j];
		}
		PlayerPrefs.SetInt("iropcount", Irocount);
		PlayerPrefs.SetInt("usuipcount", usuicount);
		PlayerPrefs.SetInt("harunapcount", harunacount);
		PlayerPrefs.SetInt("akagipcount", akagicount);
		if ((bool)Object.FindObjectOfType<SR_PlayerCountPerMap>())
		{
			Object.FindObjectOfType<SR_PlayerCountPerMap>().Updatedata();
		}
	}

	public void ExitGame()
	{
		FadeUI.SetActive(value: true);
		StartCoroutine(StartCompteur());
	}

	private IEnumerator StartCompteur()
	{
		yield return new WaitForSeconds(1.5f);
		Application.Quit();
	}

	public void SetMiddle(int CarsNumberInList)
	{
		float verticalNormalizedPosition = 1f / ((float)RoomName.Length - 1f) * (float)CarsNumberInList;
		if (CarsNumberInList <= 5)
		{
			SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
		}
		else if (CarsNumberInList >= RoomName.Length - 1 - 3)
		{
			SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
		}
		else
		{
			SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = verticalNormalizedPosition;
		}
	}
}
