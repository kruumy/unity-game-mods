using System.Collections;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
	public GameObject[] Lampadaires;

	public GameObject[] Magasin;

	public GameObject[] LittleLight;

	private Color Color;

	private Color lighte;

	private Color lighte2;

	private bool upok;

	private void Start()
	{
		Lampadaires = GameObject.FindGameObjectsWithTag("DayNight");
		Magasin = GameObject.FindGameObjectsWithTag("DayNightMagasin");
		LittleLight = GameObject.FindGameObjectsWithTag("DayNightLight");
		Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		upok = true;
	}

	private void Update()
	{
		if ((float)GetComponent<SRSkyManager>().Minute >= 19f && GetComponent<SRSkyManager>().Minute <= 46 && upok)
		{
			lighte = Color * 5f;
			lighte2 = Color * 1.5f;
			upok = false;
			GameObject[] littleLight = LittleLight;
			for (int i = 0; i < littleLight.Length; i++)
			{
				littleLight[i].GetComponent<Light>().enabled = true;
			}
			StopAllCoroutines();
			StartCoroutine(UpOKtime());
			SetLight();
		}
		else if (upok)
		{
			lighte = Color * 0f;
			lighte2 = Color * 0f;
			upok = false;
			GameObject[] littleLight = LittleLight;
			for (int i = 0; i < littleLight.Length; i++)
			{
				littleLight[i].GetComponent<Light>().enabled = false;
			}
			StopAllCoroutines();
			StartCoroutine(UpOKtime());
			SetLight();
		}
	}

	private IEnumerator UpOKtime()
	{
		yield return new WaitForSeconds(10f);
		upok = true;
	}

	public void SetLight()
	{
		GameObject[] lampadaires = Lampadaires;
		for (int i = 0; i < lampadaires.Length; i++)
		{
			lampadaires[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", lighte);
		}
		lampadaires = Magasin;
		for (int i = 0; i < lampadaires.Length; i++)
		{
			lampadaires[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", lighte2);
		}
	}
}
