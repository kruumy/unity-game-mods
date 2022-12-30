using System;
using UnityEngine;

public class LogitechLed : MonoBehaviour
{
	private int red;

	private int blue;

	private int green;

	public string effectLabel;

	private void Start()
	{
		blue = 0;
		red = 0;
		green = 0;
		LogitechGSDK.LogiLedInit();
		LogitechGSDK.LogiLedSaveCurrentLighting();
		effectLabel = "Press F to test flashing effect, P to test pulsing effect \n Press mouse1 to set all lighting to random color, mouse 2 to set G910 to random bitmap \nPress E to start per-key effects (F1-F12) show on supported devices \nPress S to stop the effects \n";
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 250f, 500f, 200f), effectLabel);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			System.Random random = new System.Random();
			red = random.Next(0, 100);
			blue = random.Next(0, 100);
			green = random.Next(0, 100);
			LogitechGSDK.LogiLedSetLighting(red, blue, green);
		}
		if (Input.GetKey(KeyCode.Mouse1))
		{
			byte[] array = new byte[504];
			System.Random random2 = new System.Random();
			for (int i = 0; i < 504; i++)
			{
				array[i] = (byte)random2.Next(0, 255);
			}
			LogitechGSDK.LogiLedSetLightingFromBitmap(array);
			red = random2.Next(0, 100);
			blue = random2.Next(0, 100);
			green = random2.Next(0, 100);
			LogitechGSDK.LogiLedSetLightingForTargetZone(LogitechGSDK.DeviceType.Speaker, 0, red, blue, green);
		}
		if (Input.GetKey(KeyCode.F))
		{
			System.Random random3 = new System.Random();
			red = random3.Next(0, 100);
			blue = random3.Next(0, 100);
			green = random3.Next(0, 100);
			LogitechGSDK.LogiLedFlashLighting(red, blue, green, 0, 200);
		}
		if (Input.GetKey(KeyCode.P))
		{
			System.Random random4 = new System.Random();
			red = random4.Next(0, 100);
			blue = random4.Next(0, 100);
			green = random4.Next(0, 100);
			LogitechGSDK.LogiLedPulseLighting(red, blue, green, 0, 100);
		}
		if (Input.GetKey(KeyCode.E))
		{
			System.Random random5 = new System.Random();
			red = random5.Next(0, 100);
			blue = random5.Next(0, 100);
			green = random5.Next(0, 100);
			int startRedPercentage = random5.Next(0, 100);
			int startBluePercentage = random5.Next(0, 100);
			int startGreenPercentage = random5.Next(0, 100);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F1, startRedPercentage, startGreenPercentage, startBluePercentage, red, green, blue, 100, isInfinite: true);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F2, startRedPercentage, startGreenPercentage, startBluePercentage, red, green, blue, 100, isInfinite: true);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F3, startRedPercentage, startGreenPercentage, startBluePercentage, red, green, blue, 100, isInfinite: true);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F4, startRedPercentage, startGreenPercentage, startBluePercentage, red, green, blue, 100, isInfinite: true);
			red = random5.Next(0, 100);
			blue = random5.Next(0, 100);
			green = random5.Next(0, 100);
			int msInterval = random5.Next(50, 200);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F5, red, green, blue, 0, msInterval);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F6, red, green, blue, 0, msInterval);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F7, red, green, blue, 0, msInterval);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F8, red, green, blue, 0, msInterval);
		}
		if (Input.GetKey(KeyCode.S))
		{
			LogitechGSDK.LogiLedStopEffects();
		}
	}

	private void OnDestroy()
	{
		LogitechGSDK.LogiLedRestoreLighting();
		LogitechGSDK.LogiLedShutdown();
	}
}
