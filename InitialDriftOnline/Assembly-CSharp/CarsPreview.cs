using UnityEngine;
using UnityEngine.UI;

public class CarsPreview : MonoBehaviour
{
	public GameObject CarsPreviews;

	public GameObject WheelCamUI;

	public GameObject WheelCamInCars;

	public GameObject WheelCamInCarsSuspension;

	public GameObject slidersuspensioncam;

	private void Start()
	{
	}

	private void Update()
	{
		CarsPreviews.GetComponent<Image>().sprite = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().MyIcon;
		WheelCamInCars.transform.position = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<AudioListener>().gameObject.transform.position;
		WheelCamInCars.transform.rotation = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<AudioListener>().gameObject.transform.rotation;
	}

	public void EnableWheelCam()
	{
		WheelCamInCarsSuspension.GetComponent<Camera>().enabled = false;
		WheelCamInCars.GetComponent<Camera>().enabled = true;
		WheelCamUI.SetActive(value: true);
	}

	public void EnableSuspensionCam()
	{
		WheelCamInCarsSuspension.GetComponent<Camera>().enabled = true;
		WheelCamInCars.GetComponent<Camera>().enabled = false;
		WheelCamUI.SetActive(value: true);
	}

	public void DisableUICam()
	{
		WheelCamInCarsSuspension.GetComponent<Camera>().enabled = false;
		WheelCamInCars.GetComponent<Camera>().enabled = false;
		WheelCamUI.SetActive(value: false);
	}

	public void CamZoom()
	{
		WheelCamInCarsSuspension.GetComponent<Camera>().fieldOfView = slidersuspensioncam.GetComponent<Slider>().value;
	}
}
