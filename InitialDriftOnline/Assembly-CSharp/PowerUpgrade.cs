using UnityEngine;
using UnityEngine.UI;

public class PowerUpgrade : MonoBehaviour
{
	public int Stage0;

	public int Stage1;

	public int Stage2;

	public int Stage3;

	public int Stage4;

	public int Stage5;

	public GameObject S0;

	public GameObject S1;

	public GameObject S2;

	public GameObject S3;

	public GameObject S4;

	public GameObject S5;

	private void Start()
	{
		UpdateHightLightbtn();
	}

	private void Update()
	{
	}

	public void UpdateHightLightbtn()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		if (PlayerPrefs.GetString("PowerBtnUsed" + text) == "Stage (0)")
		{
			HighLightThis(S0);
		}
		if (PlayerPrefs.GetString("PowerBtnUsed" + text) == "Stage (1)")
		{
			HighLightThis(S1);
		}
		if (PlayerPrefs.GetString("PowerBtnUsed" + text) == "Stage (2)")
		{
			HighLightThis(S2);
		}
		if (PlayerPrefs.GetString("PowerBtnUsed" + text) == "Stage (3)")
		{
			HighLightThis(S3);
		}
		if (PlayerPrefs.GetString("PowerBtnUsed" + text) == "Stage (4)")
		{
			HighLightThis(S4);
		}
		if (PlayerPrefs.GetString("PowerBtnUsed" + text) == "Stage (5)")
		{
			HighLightThis(S5);
		}
	}

	public void HighLightThis(GameObject target)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		S0.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		S1.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		S2.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		S3.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		S4.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		S5.GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		target.GetComponent<Image>().color = new Color32(185, 144, 144, byte.MaxValue);
		PlayerPrefs.SetString("PowerBtnUsed" + text, target.transform.name);
	}

	public void PowerStage0()
	{
		Debug.Log("STAGE 0");
		RCC_Customization.SetMaximumSpeed(RCC_SceneManager.Instance.activePlayerVehicle, Stage0);
		RCC_Customization.SaveStatsTemp(RCC_SceneManager.Instance.activePlayerVehicle);
	}

	public void PowerStage1()
	{
		Debug.Log("STAGE 1");
		RCC_Customization.SetMaximumSpeed(RCC_SceneManager.Instance.activePlayerVehicle, Stage1);
		RCC_Customization.SaveStatsTemp(RCC_SceneManager.Instance.activePlayerVehicle);
	}

	public void PowerStage2()
	{
		Debug.Log("STAGE 2");
		RCC_Customization.SetMaximumSpeed(RCC_SceneManager.Instance.activePlayerVehicle, Stage2);
		RCC_Customization.SaveStatsTemp(RCC_SceneManager.Instance.activePlayerVehicle);
	}

	public void PowerStage3()
	{
		Debug.Log("STAGE 3");
		RCC_Customization.SetMaximumSpeed(RCC_SceneManager.Instance.activePlayerVehicle, Stage3);
		RCC_Customization.SaveStatsTemp(RCC_SceneManager.Instance.activePlayerVehicle);
	}

	public void PowerStage4()
	{
		Debug.Log("STAGE 4");
		RCC_Customization.SetMaximumSpeed(RCC_SceneManager.Instance.activePlayerVehicle, Stage4);
		RCC_Customization.SaveStatsTemp(RCC_SceneManager.Instance.activePlayerVehicle);
	}

	public void PowerStage5()
	{
		Debug.Log("STAGE 5");
		RCC_Customization.SetMaximumSpeed(RCC_SceneManager.Instance.activePlayerVehicle, Stage5);
		RCC_Customization.SaveStatsTemp(RCC_SceneManager.Instance.activePlayerVehicle);
	}
}
