using System.Collections;
using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class RespawnCube : MonoBehaviour
{
	public Transform RespawnPointer;

	public int Detection;

	private GameObject LASTPOSTP;

	public RCC_CarControllerV3 playerCar;

	private void Start()
	{
		Detection = 0;
		ObscuredPrefs.SetInt("WAITTP", 10);
		LASTPOSTP = GameObject.Find("Scene Objects/AutoRespawn_Collider");
	}

	private void Update()
	{
		if (Detection != 10)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (ObscuredPrefs.GetInt("WAITTP") == 10 && ObscuredPrefs.GetInt("ONTYPING") == 0)
			{
				if (ObscuredPrefs.GetBool("TOFU RUN"))
				{
					ObscuredPrefs.SetBool("RespawnInTofu", value: true);
				}
				ObscuredPrefs.SetInt("WAITTP", 0);
				StartCoroutine(WaitTP());
			}
			else if (ObscuredPrefs.GetInt("ONTYPING") == 0)
			{
				LASTPOSTP.GetComponent<SRautorespawn>().WaitMessage();
			}
		}
		if ((!Input.GetKeyDown(KeyCode.Joystick1Button2) || !(PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")) && (!RCC_LogitechSteeringWheel.GetKeyPressed(0, 2) || !(PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel")) && (!Input.GetButton("PS4_Square") || !(PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")))
		{
			return;
		}
		if (ObscuredPrefs.GetInt("WAITTP") == 10)
		{
			if (ObscuredPrefs.GetBool("TOFU RUN"))
			{
				ObscuredPrefs.SetBool("RespawnInTofu", value: true);
			}
			ObscuredPrefs.SetInt("WAITTP", 0);
			StartCoroutine(WaitTP());
		}
		else
		{
			LASTPOSTP.GetComponent<SRautorespawn>().WaitMessage();
		}
		Debug.Log("RESPAWN 4 !");
	}

	private IEnumerator WaitTP()
	{
		_ = Vector3.zero;
		_ = Quaternion.identity;
		Vector3 lastKnownPos = RespawnPointer.position;
		Quaternion lastKnownRot = RespawnPointer.rotation;
		playerCar.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		yield return new WaitForSeconds(0.3f);
		playerCar.gameObject.transform.position = lastKnownPos;
		playerCar.gameObject.transform.rotation = lastKnownRot;
		yield return new WaitForSeconds(0.3f);
		playerCar.gameObject.GetComponent<Rigidbody>().drag = 0.01f;
		yield return new WaitForSeconds(9f);
		ObscuredPrefs.SetInt("WAITTP", 10);
	}

	public void TPSOUSMAP()
	{
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		zero = RespawnPointer.position;
		identity = RespawnPointer.rotation;
		playerCar.gameObject.GetComponent<Rigidbody>().velocity = base.transform.right * 0f;
		playerCar.gameObject.transform.position = zero;
		playerCar.gameObject.transform.rotation = identity;
		playerCar.gameObject.GetComponent<Rigidbody>().velocity = base.transform.right * 0f;
	}

	public void SetTarget(GameObject player)
	{
		playerCar = player.GetComponent<RCC_CarControllerV3>();
	}

	public void OnTriggerEnter(Collider player)
	{
		if ((bool)player.GetComponentInParent<RCC_PhotonNetwork>() && player.GetComponentInParent<PhotonView>().Owner.IsLocal)
		{
			Detection = 10;
			ObscuredPrefs.SetString("CurrentCubee", base.gameObject.name);
		}
	}

	public void OnTriggerExit(Collider player)
	{
		if (player.gameObject.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			Detection = 0;
		}
	}
}
