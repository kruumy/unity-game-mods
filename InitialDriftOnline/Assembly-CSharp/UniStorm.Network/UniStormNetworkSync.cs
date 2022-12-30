using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace UniStorm.Network;

public class UniStormNetworkSync : MonoBehaviourPun
{
	[Tooltip("The UniStorm Prefab that will be spawned for players. There's nothing special that needs to be done for the UniStorm Prefab.")]
	public GameObject UniStormPrefab;

	public string LobbyCameraName = "UniStorm Lobby Camera";

	[HideInInspector]
	public PhotonView PlayerPhotonView;

	[HideInInspector]
	public bool UniStormInitlialized;

	private int WaitForRpc;

	private void Start()
	{
		PlayerPhotonView = GetComponent<PhotonView>();
		if (PlayerPhotonView.IsMine && PlayerPrefs.GetInt("UniStormLoad") == 1)
		{
			Debug.Log("UNISTOOOOOOOOOOOOOOOOOORM");
			PlayerPrefs.SetInt("UniStormLoad", 0);
			Object.Instantiate(UniStormPrefab, Vector3.zero, Quaternion.identity).GetComponent<PhotonView>();
			if ((bool)GameObject.Find(LobbyCameraName))
			{
				GameObject.Find(LobbyCameraName).SetActive(value: false);
			}
		}
	}

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient && UniStormSystem.Instance != null && UniStormSystem.Instance.UniStormInitialized && WaitForRpc == 0)
		{
			WaitForRpc = 1;
			PlayerPhotonView.RPC("ChangeWeather", RpcTarget.All, UniStormSystem.Instance.AllWeatherTypes.IndexOf(UniStormSystem.Instance.CurrentWeatherType), UniStormSystem.Instance.m_TimeFloat, UniStormSystem.Instance.Minute, UniStormSystem.Instance.Hour, UniStormSystem.Instance.Day, UniStormSystem.Instance.Month, UniStormSystem.Instance.Year);
			StartCoroutine(WaitRPC());
		}
	}

	private IEnumerator WaitRPC()
	{
		yield return new WaitForSeconds(0.6f);
		WaitForRpc = 0;
	}

	[PunRPC]
	public void ChangeWeather(int WeatherTypeIndex, float m_TimeFloat, int Minute, int Hour, int Day, int Month, int Year)
	{
		if (!PhotonNetwork.IsMasterClient && UniStormSystem.Instance != null && UniStormSystem.Instance.UniStormInitialized)
		{
			if (UniStormSystem.Instance.AllWeatherTypes[WeatherTypeIndex] != UniStormSystem.Instance.CurrentWeatherType && UniStormInitlialized)
			{
				UniStormManager.Instance.ChangeWeatherWithTransition(UniStormSystem.Instance.AllWeatherTypes[WeatherTypeIndex]);
			}
			else if (UniStormSystem.Instance.AllWeatherTypes[WeatherTypeIndex] != UniStormSystem.Instance.CurrentWeatherType && !UniStormInitlialized)
			{
				UniStormManager.Instance.ChangeWeatherInstantly(UniStormSystem.Instance.AllWeatherTypes[WeatherTypeIndex]);
			}
			if (!UniStormInitlialized)
			{
				UniStormSystem.Instance.UseUniStormMenu = UniStormSystem.EnableFeature.Disabled;
				UniStormInitlialized = true;
			}
			UniStormSystem.Instance.m_TimeFloat = m_TimeFloat;
			UniStormSystem.Instance.Minute = Minute;
			UniStormSystem.Instance.Day = Day;
			UniStormSystem.Instance.Month = Month;
			UniStormSystem.Instance.Year = Year;
		}
	}
}
