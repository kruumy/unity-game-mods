using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.GameServices;
using UnityEngine;

public class SRDLCManager : MonoBehaviour
{
	public SteamDLCData DLCTune;

	public GameObject addtunebtn;

	private void Start()
	{
	}

	public void Setupdlctune()
	{
		int num = 0;
		addtunebtn = GameObject.FindGameObjectWithTag("dlccashbtn");
		if (ObscuredPrefs.GetBool("TakedDLCGOld"))
		{
			Debug.Log("[DLC Cash] TUNE DEJA OBTENU : (" + ObscuredPrefs.GetBool("TakedDLCGOld") + ")");
			num++;
			if ((bool)addtunebtn)
			{
				addtunebtn.SetActive(value: false);
			}
		}
		else if (DLCTune.IsDlcInstalled && DLCTune.IsSubscribed && !ObscuredPrefs.GetBool("TakedDLCGOld") && num == 0)
		{
			num++;
			ObscuredPrefs.SetBool("TakedDLCGOld", value: true);
			Debug.Log("[NEW] DLC TUNE PURCHASED : (false -> " + ObscuredPrefs.GetBool("TakedDLCGOld") + ")");
			ObscuredPrefs.SetInt("Currency", ObscuredPrefs.GetInt("Currency") + 200000);
			ObscuredPrefs.SetInt("TOTALWINMONEY", ObscuredPrefs.GetInt("TOTALWINMONEY") + 200000);
			if ((bool)addtunebtn)
			{
				addtunebtn.SetActive(value: false);
			}
			Object.FindObjectOfType<CloudDataManager>().SaveData();
		}
		else if (num == 0)
		{
			Debug.Log("[DLC Cash] DLC NON ACHETER : (" + ObscuredPrefs.GetBool("TakedDLCGOld") + ")");
			if ((bool)addtunebtn)
			{
				addtunebtn.SetActive(value: true);
			}
		}
	}

	public void Cashpage()
	{
		DLCTune.OpenStore();
	}

	public void moretunebtn()
	{
		addtunebtn = GameObject.FindGameObjectWithTag("dlccashbtn");
		if (ObscuredPrefs.GetBool("TakedDLCGOld"))
		{
			if ((bool)addtunebtn)
			{
				addtunebtn.SetActive(value: false);
			}
		}
		else if (DLCTune.IsDlcInstalled && DLCTune.IsSubscribed && !ObscuredPrefs.GetBool("TakedDLCGOld"))
		{
			if ((bool)addtunebtn)
			{
				addtunebtn.SetActive(value: false);
			}
		}
		else if ((bool)addtunebtn)
		{
			addtunebtn.SetActive(value: true);
		}
	}

	private void Update()
	{
	}
}
