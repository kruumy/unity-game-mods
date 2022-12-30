using UnityEngine;
using XInputDotNetPure;

public class XInputTestCS : MonoBehaviour
{
	private bool playerIndexSet;

	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
	}

	private void Update()
	{
		if (!playerIndexSet || !prevState.IsConnected)
		{
			for (int i = 0; i < 4; i++)
			{
				PlayerIndex playerIndex = (PlayerIndex)i;
				if (GamePad.GetState(playerIndex).IsConnected)
				{
					Debug.Log($"GamePad found {playerIndex}");
					this.playerIndex = playerIndex;
					playerIndexSet = true;
				}
			}
		}
		prevState = state;
		state = GamePad.GetState(this.playerIndex);
		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
		{
			GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1f);
		}
		if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
		{
			GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
		}
		base.transform.localRotation *= Quaternion.Euler(0f, state.ThumbSticks.Left.X * 25f * Time.deltaTime, 0f);
	}

	private void OnGUI()
	{
		string text = "Use left stick to turn the cube, hold A to change color\n";
		text += $"IsConnected {state.IsConnected} Packet #{state.PacketNumber}\n";
		text += $"\tTriggers {state.Triggers.Left} {state.Triggers.Right}\n";
		text += $"\tD-Pad {state.DPad.Up} {state.DPad.Right} {state.DPad.Down} {state.DPad.Left}\n";
		text += $"\tButtons Start {state.Buttons.Start} Back {state.Buttons.Back} Guide {state.Buttons.Guide}\n";
		text += $"\tButtons LeftStick {state.Buttons.LeftStick} RightStick {state.Buttons.RightStick} LeftShoulder {state.Buttons.LeftShoulder} RightShoulder {state.Buttons.RightShoulder}\n";
		text += $"\tButtons A {state.Buttons.A} B {state.Buttons.B} X {state.Buttons.X} Y {state.Buttons.Y}\n";
		text += $"\tSticks Left {state.ThumbSticks.Left.X} {state.ThumbSticks.Left.Y} Right {state.ThumbSticks.Right.X} {state.ThumbSticks.Right.Y}\n";
		GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), text);
	}
}
