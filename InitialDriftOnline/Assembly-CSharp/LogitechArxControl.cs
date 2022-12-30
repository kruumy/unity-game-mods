using System;
using System.IO;
using UnityEngine;

public class LogitechArxControl : MonoBehaviour
{
	private string descriptionLabel;

	private void Start()
	{
		LogitechGSDK.logiArxCbContext callback = default(LogitechGSDK.logiArxCbContext);
		LogitechGSDK.logiArxCB logiArxCB = (callback.arxCallBack = ArxSDKCallback);
		callback.arxContext = IntPtr.Zero;
		LogitechGSDK.LogiArxInit("com.logitech.unitysample", "Unity Sample", ref callback);
		descriptionLabel = "Click the left mouse button to update the progress bar, Press G to switch to a different index file, press I to go back to the original one.";
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 350f, 500f, 50f), descriptionLabel);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			int num = new System.Random().Next(0, 100);
			LogitechGSDK.LogiArxSetTagPropertyById("progressbarProgress", "style.width", num + "%");
		}
		if (Input.GetKey(KeyCode.I))
		{
			LogitechGSDK.LogiArxSetIndex("applet.html");
		}
		if (Input.GetKey(KeyCode.G))
		{
			LogitechGSDK.LogiArxSetIndex("gameover.html");
		}
	}

	public static string getHtmlString()
	{
		return "" + "<html><center><body bgcolor='black'><a href='applet.html'><img src='gameover.png'/></a></body></center></html>";
	}

	private void ArxSDKCallback(int eventType, int eventValue, string eventArg, IntPtr context)
	{
		Debug.Log("CALLBACK: type:" + eventType + ", value:" + eventValue + ", arg:" + eventArg);
		switch (eventType)
		{
		case 8:
		{
			if (!LogitechGSDK.LogiArxAddFileAs("Assets//Logitech SDK//AppletData//applet.html", "applet.html", ""))
			{
				Debug.Log("Could not send applet.html : " + LogitechGSDK.LogiArxGetLastError());
			}
			if (!LogitechGSDK.LogiArxAddFileAs("Assets//Logitech SDK//AppletData//background.png", "background.png", ""))
			{
				Debug.Log("Could not send background.png : " + LogitechGSDK.LogiArxGetLastError());
			}
			if (!LogitechGSDK.LogiArxAddUTF8StringAs(getHtmlString(), "gameover.html"))
			{
				Debug.Log("Could not send gameover.html  : " + LogitechGSDK.LogiArxGetLastError());
			}
			byte[] array = File.ReadAllBytes("Assets//Logitech SDK//AppletData//gameover.png");
			if (!LogitechGSDK.LogiArxAddContentAs(array, array.Length, "gameover.png"))
			{
				Debug.Log("Could not send gameover.png  : " + LogitechGSDK.LogiArxGetLastError());
			}
			if (!LogitechGSDK.LogiArxSetIndex("applet.html"))
			{
				Debug.Log("Could not set index : " + LogitechGSDK.LogiArxGetLastError());
			}
			break;
		}
		case 16:
			Debug.Log("NO DEVICES");
			break;
		case 4:
			Debug.Log("Tap on tag with id :" + eventArg);
			break;
		}
	}

	private void OnDestroy()
	{
		LogitechGSDK.LogiArxShutdown();
	}
}
