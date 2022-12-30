using UnityEngine;
using XInputDotNetPure;

public class Vibration_Event : MonoBehaviour
{
	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Joystick1Button1))
		{
			GamePad.SetVibration(playerIndex, 0f, 0.5f);
		}
		if (Input.GetKeyDown(KeyCode.Joystick1Button0))
		{
			GamePad.SetVibration(playerIndex, -0f, -0f);
		}
	}
}
