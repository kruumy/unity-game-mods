using System.Collections;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class SkinManager : MonoBehaviour
{
	[Space]
	[Header("== SKIN DE BASE = ELEMENT 0 ==")]
	public GameObject[] Skins;

	public Sprite[] IconSkin;

	public string CarsPlayerPrefName;

	private string MyGameObjectName;

	private int SkinNumber;

	private int TempoForReload;

	[Space]
	[Header("== ICON FOR PLAYERLIST & GARAGE PREVIEW ==")]
	public Sprite MyIcon;

	[Space]
	[Header("== SKIN 0 : CHANGE COLOR ==")]
	public Renderer[] VehiculePart;

	public Color32 jack;

	public Color32 ColorOrig;

	private void Awake()
	{
	}

	private void Start()
	{
		TempoForReload = 0;
		StartCoroutine(CheckItsme());
	}

	private void Update()
	{
		if (GetComponentInParent<RCC_PhotonNetwork>().isMine && TempoForReload == 0)
		{
			TempoForReload = 1;
			StartCoroutine(TempoCheckSkin());
			SetSkin();
		}
	}

	private IEnumerator CheckItsme()
	{
		yield return new WaitForSeconds(0.3f);
		if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			SetColorFromPP();
		}
		else
		{
			StartCoroutine(CheckItsme());
		}
	}

	private IEnumerator TempoCheckSkin()
	{
		yield return new WaitForSeconds(0.5f);
		TempoForReload = 0;
	}

	public void SetSkin()
	{
		for (int i = 0; i < Skins.Length; i++)
		{
			if (ObscuredPrefs.GetInt("SkinUse" + CarsPlayerPrefName) == i)
			{
				Skins[i].SetActive(value: true);
				MyIcon = IconSkin[i];
				SkinNumber = i;
				if (i == 0 && GetComponentInParent<RCC_PhotonNetwork>().isMine)
				{
					ChangeColorBaseSkin();
				}
			}
			else
			{
				Skins[i].SetActive(value: false);
			}
		}
	}

	public void UpdatePP()
	{
		ObscuredPrefs.SetColor("Skin0Color" + CarsPlayerPrefName, jack);
		ChangeColorBaseSkin();
	}

	public void SetColorFromPP()
	{
		if (PlayerPrefs.GetInt("ColorOrigSkin0" + CarsPlayerPrefName) == 0)
		{
			ObscuredPrefs.SetColor("Skin0Color" + CarsPlayerPrefName, ColorOrig);
			Renderer[] vehiculePart = VehiculePart;
			for (int i = 0; i < vehiculePart.Length; i++)
			{
				vehiculePart[i].material.SetColor("BODY", jack);
			}
			PlayerPrefs.SetInt("ColorOrigSkin0" + CarsPlayerPrefName, 1);
		}
		else
		{
			jack = ObscuredPrefs.GetColor("Skin0Color" + CarsPlayerPrefName);
			Renderer[] vehiculePart = VehiculePart;
			for (int i = 0; i < vehiculePart.Length; i++)
			{
				vehiculePart[i].material.SetColor("BODY", jack);
			}
		}
		SendMySkinColorToOther();
	}

	public void ChangeColorBaseSkin()
	{
		Renderer[] vehiculePart = VehiculePart;
		for (int i = 0; i < vehiculePart.Length; i++)
		{
			vehiculePart[i].material.color = jack;
		}
	}

	public void SendMySkinColorToOther()
	{
		jack = ObscuredPrefs.GetColor("Skin0Color" + CarsPlayerPrefName);
		GetComponentInParent<SRPlayerCollider>().SendMySkin0Color(jack.r, jack.g, jack.b, jack.a);
	}

	public void SetSkinForThisPlayer(int r, int g, int b, int a)
	{
		jack = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
		Renderer[] vehiculePart = VehiculePart;
		for (int i = 0; i < vehiculePart.Length; i++)
		{
			vehiculePart[i].material.SetColor("BODY", jack);
		}
		vehiculePart = VehiculePart;
		for (int i = 0; i < vehiculePart.Length; i++)
		{
			vehiculePart[i].material.color = jack;
		}
	}
}
