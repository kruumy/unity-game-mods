using UnityEngine;

public class SRCusorManager : MonoBehaviour
{
	public GameObject Menu;

	public GameObject ExitMenu;

	public GameObject Garage;

	public GameObject CarsDealer;

	public GameObject RoomConnexion;

	public GameObject TofuAcceptation;

	public GameObject RaceNotificationBtnYes;

	public GameObject RaceNotification;

	public GameObject Chat;

	public GameObject ChooseLangTuto;

	public GameObject tutom;

	public GameObject NosFuel;

	public GameObject TuningMenu;

	public GameObject Player_List;

	public GameObject GarageBtnList;

	private void Start()
	{
		PlayerPrefs.SetInt("MenuOpen", 0);
	}

	private void Update()
	{
		if (Player_List.activeSelf || TuningMenu.activeSelf || Chat.activeSelf || Menu.activeSelf || ExitMenu.activeSelf || Garage.activeSelf || CarsDealer.activeSelf || NosFuel.activeSelf || RoomConnexion.activeSelf || TofuAcceptation.activeSelf || (RaceNotificationBtnYes.activeSelf && RaceNotification.activeSelf) || (tutom.activeSelf && ChooseLangTuto.activeSelf))
		{
			Cursor.visible = true;
		}
		else
		{
			Cursor.visible = false;
		}
		if (CarsDealer.activeSelf || Menu.activeSelf || ExitMenu.activeSelf || (Garage.activeSelf && GarageBtnList.activeSelf))
		{
			PlayerPrefs.SetInt("MenuOpen", 1);
		}
		else
		{
			PlayerPrefs.SetInt("MenuOpen", 0);
		}
	}
}
