using System.Collections;
using HSVPicker;
using UnityEngine;

public class SRSkinColorManager : MonoBehaviour
{
	public Renderer[] renderer;

	public ColorPicker picker;

	public Color Color = Color.red;

	public bool SetColorOnStart;

	private void Start()
	{
		picker.onValueChanged.AddListener(delegate(Color color)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().jack = color;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().UpdatePP();
		});
	}

	private void Update()
	{
	}

	public void Setcolor()
	{
		StartCoroutine(CestUnPeuJeennre());
	}

	private IEnumerator CestUnPeuJeennre()
	{
		yield return new WaitForSeconds(3f);
		picker.GetComponent<ColorPicker>().CurrentColor = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().jack;
	}
}
