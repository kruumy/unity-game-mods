using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class FinishZone : MonoBehaviour
{
	public GameObject LogoArriver;

	private int OneShot;

	public GameObject RaceManager;

	private void Start()
	{
	}

	private void Update()
	{
		if (PlayerPrefs.GetInt("ImInRun") == 1)
		{
			LogoArriver.SetActive(value: true);
		}
		else
		{
			LogoArriver.SetActive(value: false);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 1)
		{
			RaceManager.GetComponent<RaceManager>().FinishFirst();
			Debug.Log("JE SUIS PREMIER / PERSONNE ENTREE DANS LAZONE : " + other.gameObject.GetComponentInParent<PhotonView>().gameObject.transform.name);
			PlayerPrefs.SetString("RaceEnemyBlaze", "");
			other.gameObject.tag = "Player";
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
		else if (PlayerPrefs.GetInt("ImInRun") == 1 && other.gameObject.GetComponentInParent<PhotonView>().gameObject.transform.name == PlayerPrefs.GetString("RaceEnemyBlaze"))
		{
			PlayerPrefs.SetInt("ImInRun", 0);
			RaceManager.GetComponent<RaceManager>().FinishSecond();
			other.gameObject.tag = "Player";
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
			PlayerPrefs.SetString("RaceEnemyBlaze", "");
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && !ObscuredPrefs.GetBool("TOFU RUN"))
		{
			other.gameObject.tag = "Player";
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
	}
}
