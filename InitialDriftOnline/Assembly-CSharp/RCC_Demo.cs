using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC Demo Manager")]
public class RCC_Demo : MonoBehaviour
{
	[Header("Spawnable Vehicles")]
	public RCC_CarControllerV3[] selectableVehicles;

	internal int selectedVehicleIndex;

	internal int selectedBehaviorIndex;

	public void SelectVehicle(int index)
	{
		selectedVehicleIndex = index;
	}

	public void Spawn()
	{
		Vector3 vector = default(Vector3);
		Quaternion rotation = default(Quaternion);
		if ((bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			vector = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
			rotation = RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation;
		}
		if (vector == Vector3.zero && (bool)RCC_SceneManager.Instance.activePlayerCamera)
		{
			vector = RCC_SceneManager.Instance.activePlayerCamera.transform.position;
			rotation = RCC_SceneManager.Instance.activePlayerCamera.transform.rotation;
		}
		rotation.x = 0f;
		rotation.z = 0f;
		RCC_CarControllerV3 activePlayerVehicle = RCC_SceneManager.Instance.activePlayerVehicle;
		if ((bool)activePlayerVehicle)
		{
			Object.Destroy(activePlayerVehicle.gameObject);
		}
		RCC.SpawnRCC(selectableVehicles[selectedVehicleIndex], vector, rotation, registerAsPlayerVehicle: true, isControllable: true, isEngineRunning: true);
	}

	public void SetBehavior(int index)
	{
		selectedBehaviorIndex = index;
	}

	public void InitBehavior()
	{
		RCC.SetBehavior(selectedBehaviorIndex);
	}

	public void SetController(int index)
	{
		RCC.SetController(index);
	}

	public void SetMobileController(int index)
	{
		switch (index)
		{
		case 0:
			RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);
			break;
		case 1:
			RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
			break;
		case 2:
			RCC.SetMobileController(RCC_Settings.MobileController.SteeringWheel);
			break;
		case 3:
			RCC.SetMobileController(RCC_Settings.MobileController.Joystick);
			break;
		}
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
