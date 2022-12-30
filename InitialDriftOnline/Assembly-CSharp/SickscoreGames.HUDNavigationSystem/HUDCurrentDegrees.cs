using UnityEngine;
using UnityEngine.UI;

namespace SickscoreGames.HUDNavigationSystem;

[RequireComponent(typeof(Text))]
public class HUDCurrentDegrees : MonoBehaviour
{
	protected Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void Update()
	{
		text.text = ((int)HUDNavigationCanvas.Instance.CompassBarCurrentDegrees).ToString();
	}
}
