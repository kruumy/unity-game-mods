using System.Collections;
using UnityEngine;

namespace UniStorm.Example;

public class SaveAndLoad : MonoBehaviour
{
	public enum SaveTypeEnum
	{
		Manual,
		Auto
	}

	public enum LoadOnStartEnum
	{
		Enabled,
		Disabled
	}

	public enum DebugLogsEnum
	{
		Enabled,
		Disabled
	}

	public SaveTypeEnum SaveType;

	public LoadOnStartEnum LoadOnStart;

	public DebugLogsEnum DebugLogs;

	public int AutoSaveSeconds = 10;

	public Transform PlayerTransform;

	public Transform PlayerCamera;

	private float m_AutoSaveTimer;

	private void Start()
	{
		if (PlayerPrefs.HasKey("UniStorm Player Position") && LoadOnStart == LoadOnStartEnum.Enabled)
		{
			StartCoroutine(LoadAutoSavedData());
		}
	}

	private void Update()
	{
		m_AutoSaveTimer += Time.deltaTime;
		if ((Input.GetKeyDown(KeyCode.T) && SaveType == SaveTypeEnum.Manual) || (m_AutoSaveTimer >= (float)AutoSaveSeconds && SaveType == SaveTypeEnum.Auto))
		{
			PlayerPrefs.SetInt("UniStorm Hour", UniStormSystem.Instance.Hour);
			PlayerPrefs.SetInt("UniStorm Minute", UniStormSystem.Instance.Minute);
			PlayerPrefs.SetInt("UniStorm Temperature", UniStormSystem.Instance.Temperature);
			PlayerPrefs.SetString("UniStorm Weather", UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName);
			PlayerPrefs.SetInt("UniStorm Month", UniStormSystem.Instance.Month);
			PlayerPrefs.SetInt("UniStorm Day", UniStormSystem.Instance.Day);
			PlayerPrefs.SetInt("UniStorm Year", UniStormSystem.Instance.Year);
			PlayerPrefs.SetString("UniStorm Player Position", PlayerTransform.position.ToString());
			PlayerPrefs.SetString("UniStorm Player Rotation", PlayerTransform.eulerAngles.ToString());
			PlayerPrefs.SetString("UniStorm Camera Rotation", PlayerCamera.eulerAngles.ToString());
			m_AutoSaveTimer = 0f;
			if (DebugLogs == DebugLogsEnum.Enabled)
			{
				Debug.Log("Data Saved: UniStorm Time: " + UniStormSystem.Instance.Hour + ":" + UniStormSystem.Instance.Minute.ToString("00") + " - UniStorm Weather: " + UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName + " - UniStorm Temperature: " + UniStormSystem.Instance.Temperature + "° - UniStorm Date: " + UniStormSystem.Instance.Month + "/" + UniStormSystem.Instance.Day + "/" + UniStormSystem.Instance.Year);
			}
		}
		if (Input.GetKeyDown(KeyCode.Y) && SaveType == SaveTypeEnum.Manual)
		{
			LoadData();
		}
	}

	private void LoadData()
	{
		if (!UniStormSystem.Instance.UniStormInitialized || !PlayerPrefs.HasKey("UniStorm Player Position"))
		{
			return;
		}
		UniStormManager.Instance.SetTime(PlayerPrefs.GetInt("UniStorm Hour"), PlayerPrefs.GetInt("UniStorm Minute"));
		UniStormSystem.Instance.Temperature = PlayerPrefs.GetInt("UniStorm Temperature");
		UniStormManager.Instance.SetDate(PlayerPrefs.GetInt("UniStorm Month"), PlayerPrefs.GetInt("UniStorm Day"), PlayerPrefs.GetInt("UniStorm Year"));
		PlayerTransform.position = StringToVector3(PlayerPrefs.GetString("UniStorm Player Position"));
		PlayerTransform.eulerAngles = StringToVector3(PlayerPrefs.GetString("UniStorm Player Rotation"));
		PlayerCamera.eulerAngles = StringToVector3(PlayerPrefs.GetString("UniStorm Camera Rotation"));
		string @string = PlayerPrefs.GetString("UniStorm Weather");
		WeatherType[] array = UniStormSystem.Instance.AllWeatherTypes.ToArray();
		foreach (WeatherType weatherType in array)
		{
			if (weatherType.WeatherTypeName == @string)
			{
				UniStormManager.Instance.ChangeWeatherInstantly(weatherType);
			}
		}
		if (DebugLogs == DebugLogsEnum.Enabled)
		{
			Debug.Log("Data Loaded");
		}
	}

	private IEnumerator LoadAutoSavedData()
	{
		yield return new WaitUntil(() => UniStormSystem.Instance.UniStormInitialized);
		LoadData();
	}

	public static Vector3 StringToVector3(string StringVector)
	{
		if (StringVector.StartsWith("(") && StringVector.EndsWith(")"))
		{
			StringVector = StringVector.Substring(1, StringVector.Length - 2);
		}
		string[] array = StringVector.Split(',');
		return new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
	}
}
