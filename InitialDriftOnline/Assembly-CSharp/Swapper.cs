using UnityEngine;

public class Swapper : MonoBehaviour
{
	public GameObject[] character;

	public int index;

	public Texture btn_tex;

	private void Awake()
	{
		GameObject[] array = character;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		character[0].SetActive(value: true);
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 100, 0f, 100f, 100f), btn_tex))
		{
			character[index].SetActive(value: false);
			index++;
			index %= character.Length;
			character[index].SetActive(value: true);
		}
	}
}
