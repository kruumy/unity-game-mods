using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class SRConcessionManager : MonoBehaviour
{
	public GameObject FirstCars;

	public Text MoneyDisplay;

	public int NombreDeVoiture;

	public int NombreDeSkin;

	public ScrollRect SRR;

	private void Start()
	{
	}

	private void Update()
	{
		if (FirstCars.activeSelf)
		{
			NombreDeVoiture = FirstCars.GetComponent<MyPlace>().AllCarDealerCars.Length;
			NombreDeSkin = FirstCars.GetComponent<MyPlace>().PriceSkin.Length + 1;
		}
		MoneyDisplay.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
	}

	public void SetScrollPos(int CarsNumberInList)
	{
		float verticalNormalizedPosition = 1f / (float)NombreDeVoiture * (float)CarsNumberInList;
		float verticalNormalizedPosition2 = 1f / (float)NombreDeVoiture * ((float)CarsNumberInList - 1f);
		if (CarsNumberInList < 2)
		{
			SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
		}
		else if (CarsNumberInList <= 5)
		{
			SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = verticalNormalizedPosition2;
		}
		else
		{
			SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = verticalNormalizedPosition;
		}
	}

	public void SetScrollPosHori(int CarsNumberInList)
	{
		float horizontalNormalizedPosition = 1f / (float)NombreDeSkin * (float)CarsNumberInList;
		float horizontalNormalizedPosition2 = 1f / (float)NombreDeSkin * 5f;
		if (CarsNumberInList <= 3)
		{
			SRR.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
			return;
		}
		switch (CarsNumberInList)
		{
		case 4:
			SRR.GetComponent<ScrollRect>().horizontalNormalizedPosition = horizontalNormalizedPosition2;
			break;
		case 6:
			SRR.GetComponent<ScrollRect>().horizontalNormalizedPosition = 8f;
			break;
		default:
			SRR.GetComponent<ScrollRect>().horizontalNormalizedPosition = horizontalNormalizedPosition;
			break;
		}
	}
}
