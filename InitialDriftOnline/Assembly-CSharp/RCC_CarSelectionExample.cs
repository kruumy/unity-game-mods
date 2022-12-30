using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RCC_CarSelectionExample : MonoBehaviour
{
	private List<RCC_CarControllerV3> _spawnedVehicles = new List<RCC_CarControllerV3>();

	public Transform spawnPosition;

	public int selectedIndex;

	public RCC_Camera RCCCamera;

	public string nextScene;

	private void Start()
	{
		if (!RCCCamera)
		{
			RCCCamera = Object.FindObjectOfType<RCC_Camera>();
		}
		CreateVehicles();
	}

	private void CreateVehicles()
	{
		for (int i = 0; i < RCC_Vehicles.Instance.vehicles.Length; i++)
		{
			RCC_CarControllerV3 rCC_CarControllerV = RCC.SpawnRCC(RCC_Vehicles.Instance.vehicles[i], spawnPosition.position, spawnPosition.rotation, registerAsPlayerVehicle: false, isControllable: false, isEngineRunning: false);
			rCC_CarControllerV.gameObject.SetActive(value: false);
			_spawnedVehicles.Add(rCC_CarControllerV);
		}
		SpawnVehicle();
		if ((bool)RCCCamera && (bool)RCCCamera.GetComponent<RCC_CameraCarSelection>())
		{
			RCCCamera.GetComponent<RCC_CameraCarSelection>().enabled = true;
		}
	}

	public void NextVehicle()
	{
		selectedIndex++;
		if (selectedIndex > _spawnedVehicles.Count - 1)
		{
			selectedIndex = 0;
		}
		SpawnVehicle();
	}

	public void PreviousVehicle()
	{
		selectedIndex--;
		if (selectedIndex < 0)
		{
			selectedIndex = _spawnedVehicles.Count - 1;
		}
		SpawnVehicle();
	}

	public void SpawnVehicle()
	{
		for (int i = 0; i < _spawnedVehicles.Count; i++)
		{
			_spawnedVehicles[i].gameObject.SetActive(value: false);
		}
		_spawnedVehicles[selectedIndex].gameObject.SetActive(value: true);
		RCC_SceneManager.Instance.activePlayerVehicle = _spawnedVehicles[selectedIndex];
	}

	public void SelectVehicle()
	{
		RCC.RegisterPlayerVehicle(_spawnedVehicles[selectedIndex]);
		_spawnedVehicles[selectedIndex].StartEngine();
		_spawnedVehicles[selectedIndex].SetCanControl(state: true);
		PlayerPrefs.SetInt("SelectedRCCVehicle", selectedIndex);
		if ((bool)RCCCamera && (bool)RCCCamera.GetComponent<RCC_CameraCarSelection>())
		{
			RCCCamera.GetComponent<RCC_CameraCarSelection>().enabled = false;
		}
		if (nextScene != "")
		{
			OpenScene();
		}
	}

	public void DeSelectVehicle()
	{
		RCC.DeRegisterPlayerVehicle();
		_spawnedVehicles[selectedIndex].transform.position = spawnPosition.position;
		_spawnedVehicles[selectedIndex].transform.rotation = spawnPosition.rotation;
		_spawnedVehicles[selectedIndex].KillEngine();
		_spawnedVehicles[selectedIndex].SetCanControl(state: false);
		_spawnedVehicles[selectedIndex].GetComponent<Rigidbody>().ResetInertiaTensor();
		_spawnedVehicles[selectedIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
		_spawnedVehicles[selectedIndex].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		if ((bool)RCCCamera && (bool)RCCCamera.GetComponent<RCC_CameraCarSelection>())
		{
			RCCCamera.GetComponent<RCC_CameraCarSelection>().enabled = true;
		}
	}

	public void OpenScene()
	{
		SceneManager.LoadScene(nextScene);
	}
}
