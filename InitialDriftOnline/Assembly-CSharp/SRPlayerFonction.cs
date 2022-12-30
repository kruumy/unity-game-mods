using System.Collections;
using Photon.Pun;
using Steamworks;
using UnityEngine;

public class SRPlayerFonction : MonoBehaviour
{
	public GameObject BattleCube;

	public GameObject BipForceAE86;

	public GameObject TopCamera;

	private string SteamIdString;

	private Vector3 campos;

	private Quaternion camrot;

	private bool OK;

	[Space]
	public GameObject[] SpoilerDorigine;

	public GameObject AileronsLocations;

	[Space]
	public GameObject[] Spoilers;

	[Space]
	public GameObject[] Trail;

	[Space]
	public GameObject PlateDorigine;

	public GameObject[] Plate;

	private void Start()
	{
		OK = true;
	}

	public void SetSpoiler(int SPOILERID)
	{
		GameObject[] spoilerDorigine;
		switch (SPOILERID)
		{
		case -1:
		{
			if (SpoilerDorigine.Length != 0)
			{
				spoilerDorigine = SpoilerDorigine;
				for (int i = 0; i < spoilerDorigine.Length; i++)
				{
					spoilerDorigine[i].SetActive(value: true);
				}
			}
			spoilerDorigine = Spoilers;
			for (int i = 0; i < spoilerDorigine.Length; i++)
			{
				spoilerDorigine[i].SetActive(value: false);
			}
			return;
		}
		case -2:
		{
			if (SpoilerDorigine.Length != 0)
			{
				spoilerDorigine = SpoilerDorigine;
				for (int i = 0; i < spoilerDorigine.Length; i++)
				{
					spoilerDorigine[i].SetActive(value: false);
				}
			}
			spoilerDorigine = Spoilers;
			for (int i = 0; i < spoilerDorigine.Length; i++)
			{
				spoilerDorigine[i].SetActive(value: false);
			}
			return;
		}
		}
		if (SpoilerDorigine.Length != 0)
		{
			spoilerDorigine = SpoilerDorigine;
			for (int i = 0; i < spoilerDorigine.Length; i++)
			{
				spoilerDorigine[i].SetActive(value: false);
			}
		}
		spoilerDorigine = Spoilers;
		for (int i = 0; i < spoilerDorigine.Length; i++)
		{
			spoilerDorigine[i].SetActive(value: false);
		}
		Spoilers[SPOILERID].SetActive(value: true);
	}

	public void SetPlate(int PlateID)
	{
		GameObject[] plate;
		if (PlateID == -1)
		{
			PlateDorigine.SetActive(value: true);
			plate = Plate;
			for (int i = 0; i < plate.Length; i++)
			{
				plate[i].SetActive(value: false);
			}
			return;
		}
		PlateDorigine.SetActive(value: false);
		plate = Plate;
		for (int i = 0; i < plate.Length; i++)
		{
			plate[i].SetActive(value: false);
		}
		Plate[PlateID].SetActive(value: true);
	}

	private IEnumerator SendMySteamId()
	{
		yield return new WaitForSeconds(1.2f);
	}

	private void Update()
	{
		if (GetComponent<PhotonView>().IsMine)
		{
			Quaternion rotation = GameObject.FindWithTag("cam").transform.rotation;
			GameObject[] array = GameObject.FindGameObjectsWithTag("3DPSEUDO");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.rotation = rotation;
			}
		}
		if (base.gameObject.transform.rotation.z <= -0.2507755f || base.gameObject.transform.rotation.z >= 0.2507755f)
		{
			TopCamera.SetActive(value: false);
			OK = true;
		}
		else if (OK)
		{
			TopCamera.SetActive(value: true);
			OK = false;
		}
	}

	public void TrailState(bool enabled)
	{
		GameObject[] trail = Trail;
		for (int i = 0; i < trail.Length; i++)
		{
			trail[i].SetActive(enabled);
		}
	}

	public void TrailStateBrake(bool emitting)
	{
		GameObject[] trail = Trail;
		for (int i = 0; i < trail.Length; i++)
		{
			trail[i].GetComponent<TrailRenderer>().emitting = emitting;
		}
	}

	public void EnableBattleCube()
	{
		BattleCube.SetActive(value: true);
	}

	public void BipAt100(bool yesno)
	{
		if (yesno && (bool)BipForceAE86)
		{
			BipForceAE86.SetActive(value: true);
		}
		else if ((bool)BipForceAE86)
		{
			BipForceAE86.SetActive(value: false);
		}
	}

	public void More100kmh()
	{
		SteamUserStats.SetAchievement("100KMH");
		SteamUserStats.StoreStats();
	}

	public void More200kmh()
	{
		SteamUserStats.SetAchievement("200KMH");
		SteamUserStats.StoreStats();
	}
}
