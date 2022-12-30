using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SRPlayerCollider : MonoBehaviour
{
	private PhotonView view;

	private string NameTarget;

	private RCC_ColorPickerBySliders LightColor;

	private int Tempo;

	private int Wait;

	public TextMeshPro Userblaze;

	private string Jack;

	private string EnvoyeurDelaDemande;

	private WheelCollider[] mecwheel;

	private WheelCollider[] MyWheel;

	private GameObject AileronsLocationOK;

	public void Start()
	{
		view = GetComponent<PhotonView>();
		NameTarget = base.gameObject.name;
		Tempo = 1;
		Wait = 0;
		MyWheel = GetComponentsInChildren<WheelCollider>();
	}

	public void DisableLeaveManRPC()
	{
		NameTarget = base.gameObject.name;
		view.RPC("DisableLeMecQuiSeBarre", RpcTarget.Others, NameTarget);
	}

	[PunRPC]
	private void DisableLeMecQuiSeBarre(string NameTarget)
	{
		GameObject gameObject = GameObject.Find(NameTarget);
		if ((bool)gameObject)
		{
			gameObject.SetActive(value: false);
		}
	}

	public void DisableLeaveManRPCBuff()
	{
		NameTarget = base.gameObject.name;
		view.RPC("DisableLeaveManRPCBuff", RpcTarget.OthersBuffered, NameTarget);
	}

	[PunRPC]
	private void DisableLeaveManRPCBuff(string NameTarget)
	{
		GameObject.Find(NameTarget).SetActive(value: false);
	}

	public void RPCChangeLightColor()
	{
		NameTarget = base.gameObject.name;
		string[] array = ((Color32)GameObject.Find(NameTarget + "/ChassisJoint(Clone)/Chassis/Lights/HeadLight").GetComponent<Light>().color).ToString().Replace("(", "").Replace(")", "")
			.Replace("R", "")
			.Replace("G", "")
			.Replace("B", "")
			.Replace("A", "")
			.Split(',');
		string value = array[0];
		string value2 = array[1];
		string value3 = array[2];
		string value4 = array[3];
		int num = Convert.ToInt32(value);
		int num2 = Convert.ToInt32(value2);
		int num3 = Convert.ToInt32(value3);
		int num4 = Convert.ToInt32(value4);
		view.RPC("SendRPCChangeLightColor", RpcTarget.OthersBuffered, NameTarget, num, num2, num3, num4);
	}

	[PunRPC]
	private void SendRPCChangeLightColor(string NameTarget, int R, int G, int B, int A)
	{
		RCC_Light[] componentsInChildren = GameObject.Find(NameTarget).GetComponentsInChildren<RCC_Light>();
		foreach (RCC_Light rCC_Light in componentsInChildren)
		{
			if (rCC_Light.lightType == RCC_Light.LightType.HeadLight || rCC_Light.lightType == RCC_Light.LightType.HighBeamHeadLight)
			{
				rCC_Light.GetComponent<Light>().color = new Color32((byte)R, (byte)G, (byte)B, (byte)A);
			}
		}
	}

	public void RPCChangeSmokeColor()
	{
		NameTarget = base.gameObject.name;
		string[] array = ((Color32)GameObject.Find(NameTarget + "/Wheel Colliders/RMCarDemoWheelFrontLeft/RCCWheelSlipAsphalt(Clone)").GetComponent<ParticleSystem>().main.startColor.color).ToString().Replace("(", "").Replace(")", "")
			.Replace("R", "")
			.Replace("G", "")
			.Replace("B", "")
			.Replace("A", "")
			.Split(',');
		string value = array[0];
		string value2 = array[1];
		string value3 = array[2];
		_ = array[3];
		int num = Convert.ToInt32(value);
		int num2 = Convert.ToInt32(value2);
		int num3 = Convert.ToInt32(value3);
		int num4 = 128;
		view.RPC("SendRPCChangeSmokeColor", RpcTarget.OthersBuffered, NameTarget, num, num2, num3, num4);
	}

	[PunRPC]
	private void SendRPCChangeSmokeColor(string NameTarget, int R, int G, int B, int A)
	{
		RCC_WheelCollider[] componentsInChildren = GameObject.Find(NameTarget).GetComponentsInChildren<RCC_WheelCollider>();
		foreach (RCC_WheelCollider rCC_WheelCollider in componentsInChildren)
		{
			for (int j = 0; j < rCC_WheelCollider.allWheelParticles.Count; j++)
			{
				ParticleSystem.MainModule main = rCC_WheelCollider.allWheelParticles[j].main;
				float a = 0.5f;
				main.startColor = new Color((int)(byte)R, (int)(byte)G, (int)(byte)B, a);
			}
		}
	}

	public void PlayernameOnCars2()
	{
		NameTarget = base.gameObject.name;
		// OLD: int @int = ObscuredPrefs.GetInt("MyLvl");
		int @int = ObscuredPrefs.GetInt("MyLvl");
        if (File.ReadAllLines("Settings.txt")[3].Split('=')[1] == "true")
            @int = 42069;
        int num = 0;
        view.RPC("SendRPC_PlayernameOnCars2", RpcTarget.Others, NameTarget, PlayerPrefs.GetString("PLAYERNAMEE"), @int);
	}

	[PunRPC]
	private void SendRPC_PlayernameOnCars2(string NameTarget, string TargetPseudo, int jack)
	{
		Debug.Log("TAKE THAT : <color=#ACACAC>[</color><color=#F3E400>" + jack + "</color><color=#ACACAC>]</color> " + TargetPseudo);
		GameObject gameObject = GameObject.Find(NameTarget);
		if ((bool)gameObject)
		{
			gameObject.GetComponentInChildren<TextMeshPro>().text = "<color=#ACACAC>[</color><color=#F3E400>" + jack + "</color><color=#ACACAC>]</color> " + TargetPseudo;
		}
		else
		{
			Debug.Log("Cible Introuvable");
		}
	}

	public void SendSteamId(string IDSTEAM)
	{
		NameTarget = base.gameObject.name;
		view.RPC("SendRPC_SendSteamId", RpcTarget.AllBuffered, NameTarget, IDSTEAM);
	}

	[PunRPC]
	private void SendRPC_SendSteamId(string NameTarget, string IDSTEAM)
	{
		GameObject gameObject = GameObject.Find(NameTarget);
		Debug.Log(NameTarget);
		if ((bool)gameObject)
		{
			gameObject.GetComponentInChildren<TextMeshPro>().transform.gameObject.GetComponentInChildren<Text>().text = IDSTEAM;
		}
	}

	public void TofuDeliveryStart()
	{
		NameTarget = base.gameObject.name;
		view.RPC("SendRPC_TofuDeliveryStart", RpcTarget.Others, NameTarget);
	}

	[PunRPC]
	private void SendRPC_TofuDeliveryStart(string NameTarget)
	{
		GameObject.Find(NameTarget).GetComponentInChildren<Animator>().Play("CagetteApear");
	}

	public void TofuDeliveryStop()
	{
		NameTarget = base.gameObject.name;
		view.RPC("SendRPC_TofuDeliveryStop", RpcTarget.Others, NameTarget);
	}

	[PunRPC]
	private void SendRPC_TofuDeliveryStop(string NameTarget)
	{
		GameObject.Find(NameTarget).GetComponentInChildren<Animator>().Play("CagetteDesappear");
	}

	public void AppelRPCSetGhostModeV2(int LayerNumber)
	{
		NameTarget = base.gameObject.name;
		if (LayerNumber == 8)
		{
			Jack = "Player";
		}
		if (LayerNumber == 10)
		{
			Jack = "PlayerCollider";
		}
		view.RPC("SetGhostModeV2", RpcTarget.AllBuffered, LayerNumber, NameTarget, Jack);
	}

	[PunRPC]
	private void SetGhostModeV2(int LayerNumber, string NameTarget, string Jack)
	{
		StartCoroutine(TempoJack(NameTarget, Jack));
	}

	private IEnumerator TempoJack(string NameTarget, string Jack)
	{
		yield return new WaitForSeconds(0.05f);
		GameObject.Find(NameTarget).GetComponentInChildren<BoxCollider>().gameObject.tag = Jack;
		MyWheel = GetComponentsInChildren<WheelCollider>();
		UpdateGhostMode();
	}

	public void UpdateGhostMode()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("PlayerCollider");
		GameObject[] array3 = array;
		foreach (GameObject gameObject in array3)
		{
			if ((bool)gameObject.GetComponent<BoxCollider>() && base.gameObject.GetComponentInChildren<BoxCollider>().gameObject.tag == "Player")
			{
				mecwheel = gameObject.GetComponentInParent<SRPlayerCollider>().gameObject.GetComponentsInChildren<WheelCollider>();
				Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), GetComponentInChildren<BoxCollider>(), ignore: false);
				WheelCollider[] myWheel = MyWheel;
				foreach (WheelCollider collider in myWheel)
				{
					Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), collider, ignore: false);
				}
				myWheel = mecwheel;
				foreach (WheelCollider collider2 in myWheel)
				{
					Physics.IgnoreCollision(GetComponentInChildren<BoxCollider>(), collider2, ignore: false);
				}
			}
			else if ((bool)gameObject.GetComponent<BoxCollider>() && base.gameObject.GetComponentInChildren<BoxCollider>().gameObject.tag == "PlayerCollider")
			{
				mecwheel = gameObject.GetComponentInParent<SRPlayerCollider>().gameObject.GetComponentsInChildren<WheelCollider>();
				Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), GetComponentInChildren<BoxCollider>(), ignore: true);
				WheelCollider[] myWheel = MyWheel;
				foreach (WheelCollider collider3 in myWheel)
				{
					Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), collider3, ignore: true);
				}
				myWheel = mecwheel;
				foreach (WheelCollider collider4 in myWheel)
				{
					Physics.IgnoreCollision(GetComponentInChildren<BoxCollider>(), collider4, ignore: true);
				}
			}
		}
		array3 = array2;
		foreach (GameObject gameObject2 in array3)
		{
			if ((bool)gameObject2.GetComponent<BoxCollider>())
			{
				mecwheel = gameObject2.GetComponentInParent<SRPlayerCollider>().gameObject.GetComponentsInChildren<WheelCollider>();
				Physics.IgnoreCollision(gameObject2.GetComponent<BoxCollider>(), GetComponentInChildren<BoxCollider>(), ignore: true);
				WheelCollider[] myWheel = MyWheel;
				foreach (WheelCollider collider5 in myWheel)
				{
					Physics.IgnoreCollision(gameObject2.GetComponent<BoxCollider>(), collider5, ignore: true);
				}
				myWheel = mecwheel;
				foreach (WheelCollider collider6 in myWheel)
				{
					Physics.IgnoreCollision(GetComponentInChildren<BoxCollider>(), collider6, ignore: true);
				}
			}
		}
	}

	public void SendMySkinModel(int SkinNumber)
	{
		NameTarget = base.gameObject.name;
		StartCoroutine(TempoSkin(SkinNumber, NameTarget));
	}

	private IEnumerator TempoSkin(int SkinNumber, string NameTarget)
	{
		yield return new WaitForSeconds(0.2f);
		view.RPC("SendMySkin", RpcTarget.OthersBuffered, SkinNumber, NameTarget);
	}

	[PunRPC]
	private void SendMySkin(int SkinNumber, string NameTarget)
	{
		GameObject gameObject = GameObject.Find(NameTarget);
		if ((bool)gameObject)
		{
			GameObject[] skins = gameObject.GetComponentInChildren<SkinManager>().Skins;
			for (int i = 0; i < skins.Length; i++)
			{
				skins[i].SetActive(value: false);
				gameObject.GetComponentInChildren<SkinManager>().Skins[SkinNumber].SetActive(value: true);
			}
		}
	}

	public void SendRaceInvitation(string CiblePhotonName, string MyName)
	{
		NameTarget = CiblePhotonName;
		EnvoyeurDelaDemande = base.gameObject.name;
		view.RPC("RaceInvitationReceveid", RpcTarget.Others, CiblePhotonName, MyName, EnvoyeurDelaDemande);
	}

	[PunRPC]
	private void RaceInvitationReceveid(string CiblePhotonName, string MyName, string EnvoyeurDelaDemande)
	{
		if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == CiblePhotonName)
		{
			Debug.Log("INVITATION RECU CHAKAL");
			GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceManager>().ShowMyInvitation(MyName, EnvoyeurDelaDemande);
		}
	}

	public void SendRunAcceptationRPC(string CibleDuRpcPhotonName)
	{
		view.RPC("RPC_SendRunAcceptationRPC", RpcTarget.Others, CibleDuRpcPhotonName);
	}

	[PunRPC]
	private void RPC_SendRunAcceptationRPC(string CibleDuRpcPhotonName)
	{
		if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == CibleDuRpcPhotonName)
		{
			Debug.Log("COURSE ACCEPTER PAR LAUTRE JOUEUR");
			GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceManager>().LetsGoRaceP1();
		}
	}

	public void JeStopLaCourse(string CibleDuRpcPhotonName)
	{
		view.RPC("RPC_JeStopLaCourse", RpcTarget.Others, CibleDuRpcPhotonName);
	}

	[PunRPC]
	private void RPC_JeStopLaCourse(string CibleDuRpcPhotonName)
	{
		if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == CibleDuRpcPhotonName)
		{
			GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceManager>().OtherLeaveRun();
		}
		else
		{
			Debug.Log("LAUTRE JOUEUR QUITTE MAIS C PAS POUR TOI");
		}
	}

	public void SendTheTimeOfRoom(int Minute)
	{
		string text = base.gameObject.name;
		view.RPC("ReceiveTimeRPC", RpcTarget.Others, Minute, text);
	}

	[PunRPC]
	public void ReceiveTimeRPC(int Minute, string MasterPlayer)
	{
		UnityEngine.Object.FindObjectOfType<SRSkyManager>().ReceiveTimeByRPC(Minute, MasterPlayer);
	}

	public void SendMyUserID(string vIn, string comefrom)
	{
		NameTarget = base.gameObject.name;
		view.RPC("RPCTakeMyID", RpcTarget.OthersBuffered, vIn, NameTarget, comefrom);
	}

	[PunRPC]
	public void RPCTakeMyID(string vIn, string NameTarget, string comefrom)
	{
		GameObject gameObject = GameObject.Find(NameTarget);
		if ((bool)gameObject)
		{
			Debug.Log("COME FROM : " + comefrom + " | YES TARGET : " + gameObject.name + " | ID : " + vIn);
			gameObject.GetComponentInChildren<TextMeshPro>().transform.gameObject.GetComponentInChildren<Text>().text = vIn;
		}
		else
		{
			Debug.Log("COME FROM : " + comefrom + " | NO TARGET :  | ID : " + vIn);
		}
	}

	public void EditPlayerPP(string target, string playerpreff, int value, string type, string stringvalue, float sfx, float master, float music)
	{
		if (stringvalue == "")
		{
			stringvalue = "";
		}
		Debug.Log("BEFORE RPC >> CIBLE : " + target + " | PP : " + playerpreff + " | INT FLOAT VALUE : " + value + " | TYPE : " + type + " | STRING VALUE : " + stringvalue);
		view.RPC("RPC_EditPlayerPP", RpcTarget.All, target, playerpreff, value, type, stringvalue, sfx, master, music);
	}

	[PunRPC]
	public void RPC_EditPlayerPP(string target, string playerpreff, int value, string type, string stringvalue, float sfx, float master, float music)
	{
		if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == target && type == "int")
		{
			ObscuredPrefs.SetInt(playerpreff, value);
		}
		else if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == target && type == "float")
		{
			ObscuredPrefs.SetFloat(playerpreff, value);
		}
		else if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == target && type == "string")
		{
			PlayerPrefs.SetString(playerpreff, stringvalue);
		}
		else if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name == target && type == "music")
		{
			UnityEngine.Object.FindObjectOfType<GoMap>().UpSound(sfx, master, music);
		}
	}

	public void SendNotificationChat(string target)
	{
		view.RPC("ReceivNotificationChat", RpcTarget.All, target);
	}

	[PunRPC]
	public void ReceivNotificationChat(string NameTarget)
	{
		if (PlayerPrefs.GetString("PLAYERNAMEE") == NameTarget)
		{
			UnityEngine.Object.FindObjectOfType<NotificationMSG>().gameObject.GetComponent<Animator>().Play("NotificationMSG");
		}
	}

	public void SetSpoilerFor(int SpoilerID)
	{
		string text = base.gameObject.name;
		view.RPC("RPC_SetSpoilerFor", RpcTarget.Others, text, SpoilerID);
	}

	[PunRPC]
	public void RPC_SetSpoilerFor(string target, int SpoilerID)
	{
		GameObject gameObject = GameObject.Find(target);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<SRPlayerFonction>().SetSpoiler(SpoilerID);
		}
	}

	public void SetPlateFor(int PlateID)
	{
		string text = base.gameObject.name;
		view.RPC("RPC_SetSpoilerFor", RpcTarget.Others, text, PlateID);
	}

	[PunRPC]
	public void RPC_SetPlateFor(string target, int PlateID)
	{
		GameObject gameObject = GameObject.Find(target);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<SRPlayerFonction>().SetPlate(PlateID);
		}
	}

	public void SendMySkin0Color(int r, int g, int b, int a)
	{
		string text = base.gameObject.name;
		view.RPC("RPC_SendMySkin0Color", RpcTarget.Others, text, r, g, b, a);
	}

	[PunRPC]
	public void RPC_SendMySkin0Color(string target, int r, int g, int b, int a)
	{
		GameObject gameObject = GameObject.Find(target);
		if ((bool)gameObject)
		{
			Debug.Log(" RECEPTION DE LA  COULEUR DE " + gameObject.name + " : " + r + " g: " + g + " b: " + b + " a: " + a);
			gameObject.GetComponentInChildren<SkinManager>().SetSkinForThisPlayer(r, g, b, a);
		}
	}

	public void SendMyLightColor(int r, int g, int b, int a)
	{
		string text = base.gameObject.name;
		view.RPC("RPC_SendMyLightColor", RpcTarget.Others, text, r, g, b, a);
	}

	[PunRPC]
	public void RPC_SendMyLightColor(string target, int r, int g, int b, int a)
	{
		GameObject gameObject = GameObject.Find(target);
		if ((bool)gameObject)
		{
			Debug.Log(" RECEPTION DE LA  COULEUR DE " + gameObject.name + " : " + r + " g: " + g + " b: " + b + " a: " + a);
			gameObject.GetComponentInChildren<SRLightRPC>().SetColorInChildren(r, g, b, a);
		}
	}
}
