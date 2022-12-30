using UnityEngine;
using UnityEngine.UI;

public class infoPPrace : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		float brakeTorque = RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque;
		float steeringSensitivity = RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
		float drag = RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<Rigidbody>().drag;
		float mass = RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<Rigidbody>().mass;
		GetComponent<Text>().text = "BRAKE : " + brakeTorque + " \n STEERING : " + steeringSensitivity + "\n DRAG : " + drag;
		Debug.Log("BRAKE: " + brakeTorque + " | STEERING: " + steeringSensitivity + " | DRAG: " + drag + " | MASS: " + mass);
	}
}
