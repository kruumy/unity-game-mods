using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using UnityEngine;

public class SRSatomiTofu : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponentInParent<PhotonView>().IsMine && ObscuredPrefs.GetBool("TOFU RUN") && ObscuredPrefs.GetString("TOFULOCATION") == "ReverseNew")
		{
			GetComponent<Animator>().Play("LivraisonForSatomi");
		}
	}
}
