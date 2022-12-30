using CodeStage.AntiCheat.Storage;
using TMPro;
using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class RacePlayerFc : MonoBehaviour
{
	public GameObject MyCollider;

	private GameObject Ctrlr;

	private void Start()
	{
		Ctrlr = GameObject.FindGameObjectWithTag("RaceManager");
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (GetComponentInParent<RCC_PhotonNetwork>().isMine && (bool)other.GetComponentInParent<RCC_PhotonNetwork>() && !other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN"))
		{
			if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().tag == "Player")
			{
				string enemyPhoton = other.GetComponentInParent<SRPlayerCollider>().transform.name;
				string text = other.GetComponentInParent<SRPlayerCollider>().gameObject.transform.GetComponentInChildren<TextMeshPro>().text;
				Debug.Log("FIGHT AVEC : " + text);
				Ctrlr.GetComponent<RaceManager>().AskToPlayer(enemyPhoton, text);
			}
			else
			{
				GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceManager>().StopAskInfo();
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (GetComponentInParent<RCC_PhotonNetwork>().isMine && (bool)other.GetComponentInParent<RCC_PhotonNetwork>())
		{
			GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceManager>().StopAskInfo();
		}
	}
}
