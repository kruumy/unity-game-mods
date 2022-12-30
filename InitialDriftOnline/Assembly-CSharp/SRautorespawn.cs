using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SRautorespawn : MonoBehaviour
{
	public GameObject ChatGuiScript;

	public GameObject RCCPhotonDemoScript;

	public GameObject UIMessage;

	private GameObject LASTPOSTP;

	public string WAITTXT;

	public string StayOnTheRoad;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider player)
	{
		if (player.GetComponentInParent<PhotonView>().IsMine)
		{
			string text = "Scene Objects/RespawnCube/" + ObscuredPrefs.GetString("CurrentCubee");
			LASTPOSTP = GameObject.Find(text);
			LASTPOSTP.GetComponent<RespawnCube>().TPSOUSMAP();
			UIMessage.GetComponent<Text>().text = StayOnTheRoad;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
		}
	}

	public void WaitMessage()
	{
		UIMessage.GetComponent<Text>().text = WAITTXT;
		UIMessage.GetComponent<Animator>().Play("UIMessageShort");
	}
}
