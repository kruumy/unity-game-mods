using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpielmannSpiel_Launcher;

public class LauncherManager : MonoBehaviour
{
	public string playerPrefsPrefix = "";

	public string loadNextSceneName;

	[Tooltip("Sets the resolution of the launcher scene to those values and uses WINDOW mode. Disable this if you want the launcher to be in fullscreen/what Unity has detected on start/what you have set in Project Settings with default resolution.")]
	public bool enforceResolution;

	public int width;

	public int height;

	public List<FpsInfo> availableFps = new List<FpsInfo>();

	public int defaultFpsIndex;

	[Header("Available Settings")]
	public Dropdown dropdownResolution;

	public Dropdown dropdownQuality;

	public Dropdown dropdownFps;

	public Toggle toggleFullScreen;

	public Toggle toggleVsync;

	public Toggle toggleVHS;

	public GameObject CameraForVhs;

	private List<ResolutionInfo> resolutions = new List<ResolutionInfo>();

	private string[] qualitySettingsNames;

	public Text QualityInfo;

	private void Reset()
	{
		enforceResolution = true;
		playerPrefsPrefix = "launcherSettings_";
		width = 450;
		height = 500;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		PlayerPrefs.SetInt("OneLoad", 20);
		if (enforceResolution)
		{
			Screen.SetResolution(width, height, fullscreen: false);
			if (ScreenHelper.initialLauncherScreenSettings == null)
			{
				ScreenHelper.initialLauncherScreenSettings = new InitialLauncherScreenSettings(width, height, fullScreen: false);
			}
		}
		else if (ScreenHelper.initialLauncherScreenSettings == null)
		{
			ScreenHelper.initialLauncherScreenSettings = new InitialLauncherScreenSettings(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);
		}
		updateVariables();
		setUi();
		loadSettings();
	}

	public void updateVariables()
	{
		resolutions = ScreenHelper.getResolutionInfos();
		qualitySettingsNames = QualitySettings.names;
	}

	public void setUi()
	{
		if (dropdownFps != null)
		{
			dropdownFps.ClearOptions();
			_ = Display.displays;
			_ = Display.main;
			for (int i = 0; i < availableFps.Count; i++)
			{
				dropdownFps.options.Add(new Dropdown.OptionData
				{
					text = availableFps[i].ToString()
				});
			}
			dropdownFps.value = defaultFpsIndex;
		}
		if (dropdownResolution != null)
		{
			dropdownResolution.ClearOptions();
			Vector2 vector = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
			int value = 0;
			for (int j = 0; j < resolutions.Count; j++)
			{
				dropdownResolution.options.Add(new Dropdown.OptionData
				{
					text = resolutions[j].label
				});
				if (vector == resolutions[j].size)
				{
					value = j;
				}
			}
			dropdownResolution.value = value;
			dropdownResolution.RefreshShownValue();
		}
		if (dropdownQuality != null)
		{
			dropdownQuality.ClearOptions();
			int qualityLevel = QualitySettings.GetQualityLevel();
			for (int k = 0; k < qualitySettingsNames.Length; k++)
			{
				dropdownQuality.options.Add(new Dropdown.OptionData
				{
					text = qualitySettingsNames[k]
				});
			}
			dropdownQuality.value = qualityLevel;
			dropdownQuality.RefreshShownValue();
		}
	}

	public void updateQuality()
	{
		if (!(dropdownQuality != null))
		{
			return;
		}
		QualitySettings.SetQualityLevel(dropdownQuality.value, applyExpensiveChanges: true);
		PlayerPrefs.SetInt("QualitySetupValue", dropdownQuality.value);
		if (dropdownQuality.value == 0 || dropdownQuality.value == 1)
		{
			if ((bool)toggleVHS)
			{
				toggleVHS.interactable = false;
			}
			PlayerPrefs.SetInt("LowQualityJack", 0);
			if ((bool)GetComponentInChildren<SRShadowManager>())
			{
				GetComponentInChildren<SRShadowManager>().Start();
			}
		}
		else
		{
			if ((bool)toggleVHS)
			{
				toggleVHS.interactable = true;
			}
			PlayerPrefs.SetInt("LowQualityJack", 1);
			if (SceneManager.GetActiveScene().name != "LauncherSample" && (bool)GetComponentInChildren<SRShadowManager>())
			{
				GetComponentInChildren<SRShadowManager>().Start();
			}
		}
	}

	public void activateSettings()
	{
		bool flag = dropdownResolution != null;
		bool flag2 = toggleFullScreen != null;
		_ = dropdownQuality != null;
		bool flag3 = dropdownFps != null;
		bool flag4 = toggleVsync != null;
		bool flag5 = Screen.fullScreen;
		if (flag2)
		{
			flag5 = toggleFullScreen.isOn;
			PlayerPrefs.SetInt(playerPrefsPrefix + "setFullScreen", flag5 ? 1 : 0);
			if (!flag)
			{
				Screen.fullScreen = flag2;
			}
		}
		if (flag)
		{
			ResolutionInfo resolutionInfo = resolutions[dropdownResolution.value];
			Screen.SetResolution(resolutionInfo.resolution.width, resolutionInfo.resolution.height, flag5);
			PlayerPrefs.SetInt(playerPrefsPrefix + "resolutionIndex", dropdownResolution.value);
		}
		if (flag3)
		{
			Application.targetFrameRate = availableFps[dropdownFps.value].fps;
			PlayerPrefs.SetInt(playerPrefsPrefix + "fpsIndex", dropdownFps.value);
		}
		if (flag4)
		{
			QualitySettings.vSyncCount = (toggleVsync.isOn ? 1 : 0);
			PlayerPrefs.SetInt(playerPrefsPrefix + "setVsync", QualitySettings.vSyncCount);
		}
	}

	private void Update()
	{
	}

	public void loadSettings()
	{
		_ = dropdownQuality != null;
		bool flag = toggleFullScreen != null;
		bool flag2 = dropdownResolution != null;
		bool flag3 = dropdownFps != null;
		bool flag4 = toggleVsync != null;
		if (flag)
		{
			toggleFullScreen.isOn = PlayerPrefs.GetInt(playerPrefsPrefix + "setFullScreen", toggleFullScreen.isOn ? 1 : 0) == 1;
		}
		if (flag2)
		{
			dropdownResolution.value = PlayerPrefs.GetInt(playerPrefsPrefix + "resolutionIndex", dropdownResolution.value);
		}
		if (flag3)
		{
			dropdownFps.value = PlayerPrefs.GetInt(playerPrefsPrefix + "fpsIndex", dropdownFps.value);
		}
		if (flag4)
		{
			toggleVsync.isOn = PlayerPrefs.GetInt(playerPrefsPrefix + "setVsync", toggleVsync.isOn ? 1 : 0) == 1;
		}
		dropdownQuality.value = PlayerPrefs.GetInt("QualitySetupValue");
		QualitySettings.SetQualityLevel(dropdownQuality.value, applyExpensiveChanges: true);
		if (SceneManager.GetActiveScene().name != "LauncherSample")
		{
			activateSettings();
		}
	}

	public void updateVsyncFps()
	{
		if (dropdownFps != null && toggleVsync != null)
		{
			dropdownFps.interactable = !toggleVsync.isOn;
		}
	}

	public void start()
	{
		PlayerPrefs.SetInt("QualitySetupValue", dropdownQuality.value);
		if (dropdownQuality.value == 0 || dropdownQuality.value == 1)
		{
			PlayerPrefs.SetInt("LowQualityJack", 0);
		}
		else
		{
			PlayerPrefs.SetInt("LowQualityJack", 1);
		}
		PlayerPrefs.SetString("DERNIERMESSAGEPLUS", "");
		PlayerPrefs.SetString("HistoriqueDesMessages", "");
		activateSettings();
		SceneManager.LoadSceneAsync(loadNextSceneName, LoadSceneMode.Single);
	}

	public void quit()
	{
		Application.Quit();
	}
}
