using System;
using UnityEngine;

public class LogitechGKey : MonoBehaviour
{
	public bool usingCallback;

	private static string lastKeyPress = "";

	private string descriptionLabel = "";

	private void Start()
	{
		descriptionLabel = "Last g-key event : ";
		lastKeyPress = "No g-key event";
		if (usingCallback)
		{
			LogitechGSDK.logiGKeyCbContext cbStruct = default(LogitechGSDK.logiGKeyCbContext);
			LogitechGSDK.logiGkeyCB logiGkeyCB = (cbStruct.gkeyCallBack = GkeySDKCallback);
			cbStruct.gkeyContext = IntPtr.Zero;
			LogitechGSDK.LogiGkeyInit(ref cbStruct);
		}
		else
		{
			LogitechGSDK.LogiGkeyInitWithoutCallback();
		}
	}

	private void Update()
	{
		if (usingCallback)
		{
			return;
		}
		for (int i = 6; i <= 20; i++)
		{
			if (LogitechGSDK.LogiGkeyIsMouseButtonPressed(i) == 1)
			{
				lastKeyPress = "MOUSE DOWN Button : " + i;
			}
		}
		for (int j = 1; j <= 29; j++)
		{
			for (int k = 1; k <= 3; k++)
			{
				if (LogitechGSDK.LogiGkeyIsKeyboardGkeyPressed(j, k) == 1)
				{
					lastKeyPress = "KEYBOARD/HEADSET DOWN Button : " + j;
				}
			}
		}
	}

	private void GkeySDKCallback(LogitechGSDK.GkeyCode gKeyCode, string gKeyOrButtonString, IntPtr context)
	{
		if (gKeyCode.keyDown == 0)
		{
			if (gKeyCode.mouse == 1)
			{
				lastKeyPress = "MOUSE UP" + gKeyOrButtonString;
			}
			else
			{
				lastKeyPress = "KEYBOARD/HEADSET RELEASED " + gKeyOrButtonString;
			}
		}
		else if (gKeyCode.mouse == 1)
		{
			lastKeyPress = "MOUSE DOWN " + gKeyOrButtonString;
		}
		else
		{
			lastKeyPress = "KEYBOARD/HEADSET PRESSED " + gKeyOrButtonString;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 450f, 200f, 50f), descriptionLabel + lastKeyPress);
	}

	private void OnDestroy()
	{
		LogitechGSDK.LogiGkeyShutdown();
	}
}
