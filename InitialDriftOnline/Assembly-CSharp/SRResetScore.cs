using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class SRResetScore : MonoBehaviour
{
	public Sprite Trash;

	public Sprite validation;

	public GameObject ActionImage;

	public GameObject Menu;

	public GameObject[] LBBTN_List;

	public GameObject[] ResetButton_List;

	public GameObject[] Point_Rouge;

	public string PPTODELETE;

	public void Start()
	{
		ActionImage.GetComponent<Image>().sprite = null;
		ActionImage.SetActive(value: false);
		PPTODELETE = "";
		GameObject[] lBBTN_List = LBBTN_List;
		for (int i = 0; i < lBBTN_List.Length; i++)
		{
			lBBTN_List[i].GetComponent<Button>().interactable = true;
		}
		lBBTN_List = ResetButton_List;
		foreach (GameObject obj in lBBTN_List)
		{
			obj.GetComponent<Image>().enabled = false;
			obj.GetComponent<Button>().enabled = false;
		}
		lBBTN_List = Point_Rouge;
		for (int i = 0; i < lBBTN_List.Length; i++)
		{
			lBBTN_List[i].SetActive(value: false);
		}
	}

	private void Update()
	{
	}

	public void SetAction()
	{
		if (ActionImage.GetComponent<Image>().sprite == null)
		{
			ActionImage.SetActive(value: true);
			ActionImage.GetComponent<Image>().sprite = Trash;
			ActionImage.GetComponent<Image>().color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			GameObject[] lBBTN_List = LBBTN_List;
			for (int i = 0; i < lBBTN_List.Length; i++)
			{
				lBBTN_List[i].GetComponent<Button>().interactable = false;
			}
			lBBTN_List = ResetButton_List;
			foreach (GameObject obj in lBBTN_List)
			{
				obj.GetComponent<Image>().enabled = true;
				obj.GetComponent<Button>().enabled = true;
			}
			lBBTN_List = Point_Rouge;
			for (int i = 0; i < lBBTN_List.Length; i++)
			{
				lBBTN_List[i].SetActive(value: false);
			}
		}
		else if (ActionImage.activeSelf && ActionImage.GetComponent<Image>().sprite == validation)
		{
			ActionImage.GetComponent<Image>().sprite = null;
			ActionImage.GetComponent<Image>().color = new Color32(byte.MaxValue, 0, 0, 0);
			GameObject[] lBBTN_List = LBBTN_List;
			for (int i = 0; i < lBBTN_List.Length; i++)
			{
				lBBTN_List[i].GetComponent<Button>().interactable = true;
			}
			lBBTN_List = ResetButton_List;
			foreach (GameObject obj2 in lBBTN_List)
			{
				obj2.GetComponent<Image>().enabled = false;
				obj2.GetComponent<Button>().enabled = false;
			}
			lBBTN_List = Point_Rouge;
			for (int i = 0; i < lBBTN_List.Length; i++)
			{
				lBBTN_List[i].SetActive(value: false);
			}
			DeletePP();
		}
	}

	public void ShowPicto(bool state)
	{
		if (ActionImage.activeSelf && ActionImage.GetComponent<Image>().sprite == Trash && state)
		{
			ActionImage.GetComponent<Image>().sprite = validation;
			ActionImage.GetComponent<Image>().color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
		}
		else if (ActionImage.activeSelf && ActionImage.GetComponent<Image>().sprite == validation && !state)
		{
			ActionImage.GetComponent<Image>().sprite = Trash;
			ActionImage.GetComponent<Image>().color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
		}
	}

	public void AddThisPlayerPref(string Playerpref)
	{
		PPTODELETE = PPTODELETE + Playerpref + "/";
	}

	public void DeletePP()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		if (!PPTODELETE.Contains("/"))
		{
			return;
		}
		string[] array = PPTODELETE.Split('/');
		text = array[0];
		ObscuredPrefs.SetInt(text, 999);
		text = text.Replace("BestRunTime", "");
		ObscuredPrefs.SetInt("UsedCars" + text, 0);
		Debug.Log("TAILLE : " + array.Length);
		Debug.Log("1");
		if (array.Length >= 2)
		{
			text2 = array[1];
			if (text2 != "")
			{
				ObscuredPrefs.SetInt(text2, 999);
				text2 = text2.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text2, 0);
				Debug.Log("2");
			}
		}
		if (array.Length >= 3)
		{
			text3 = array[2];
			if (text3 != "")
			{
				ObscuredPrefs.SetInt(text3, 999);
				text3 = text3.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text3, 0);
				Debug.Log("3");
			}
		}
		if (array.Length >= 4)
		{
			text4 = array[3];
			if (text4 != "")
			{
				ObscuredPrefs.SetInt(text4, 999);
				text4 = text4.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text4, 0);
				Debug.Log("4");
			}
		}
		if (array.Length >= 5)
		{
			text5 = array[4];
			if (text5 != "")
			{
				ObscuredPrefs.SetInt(text5, 999);
				text5 = text5.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text5, 0);
				Debug.Log("5");
			}
		}
		if (array.Length >= 6)
		{
			text6 = array[5];
			if (text6 != "")
			{
				ObscuredPrefs.SetInt(text6, 999);
				text6 = text6.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text6, 0);
				Debug.Log("6");
			}
		}
		if (array.Length >= 7)
		{
			text7 = array[6];
			if (text7 != "")
			{
				ObscuredPrefs.SetInt(text7, 999);
				text7 = text7.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text7, 0);
				Debug.Log("7");
			}
		}
		if (array.Length >= 8)
		{
			text8 = array[7];
			if (text8 != "")
			{
				ObscuredPrefs.SetInt(text8, 999);
				text8 = text8.Replace("BestRunTime", "");
				ObscuredPrefs.SetInt("UsedCars" + text8, 0);
				Debug.Log("8");
			}
		}
		Menu.SetActive(value: false);
	}
}
