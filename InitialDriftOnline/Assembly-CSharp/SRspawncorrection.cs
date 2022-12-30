using System.Collections;
using UnityEngine;

public class SRspawncorrection : MonoBehaviour
{
	public GameObject TP;

	public GameObject correcteur1;

	public GameObject correcteur2;

	public GameObject repousseur;

	private void Start()
	{
		TP.SetActive(value: false);
		correcteur2.SetActive(value: true);
		repousseur.SetActive(value: true);
		StartCoroutine(EnableTP());
	}

	private void Update()
	{
	}

	private IEnumerator EnableTP()
	{
		yield return new WaitForSeconds(15f);
		TP.SetActive(value: true);
		correcteur2.SetActive(value: false);
		repousseur.SetActive(value: false);
	}
}
