using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class MyPlace : MonoBehaviour
{
	public GameObject PrefabDeLaVoiture;

	private GameObject PhotonManager;

	private RCC_CarControllerV3[] SpawnList;

	[Space]
	[Header("== PRIX ==")]
	public ObscuredInt CarsPrice;

	public ObscuredInt[] PriceSkin;

	[Space]
	[Header("| NE PAS REMPLIR |")]
	[Space]
	[Header(" ")]
	public string CarsName;

	public ObscuredInt NumeroDansLaListe;

	public ObscuredInt NumeroDeSpawnPhoton;

	public GameObject[] AllCarDealerCars;

	private void Start()
	{
		AllCarDealerCars = GameObject.FindGameObjectsWithTag("CarDealerCars");
		CarsName = PrefabDeLaVoiture.GetComponentInChildren<SkinManager>().CarsPlayerPrefName;
		PhotonManager = GameObject.FindGameObjectWithTag("RCCCanvasPhoton");
		SpawnList = PhotonManager.GetComponent<RCC_PhotonDemo>().selectableVehicles;
		for (int i = 0; i != SpawnList.Length; i++)
		{
			if (SpawnList[i].gameObject.transform.name == PrefabDeLaVoiture.gameObject.transform.name)
			{
				NumeroDeSpawnPhoton = i;
			}
		}
		for (int j = 0; j != AllCarDealerCars.Length; j++)
		{
			if (AllCarDealerCars[j].gameObject.transform.name == base.gameObject.transform.name)
			{
				NumeroDansLaListe = j + 1;
			}
		}
	}

	private void Update()
	{
	}
}
