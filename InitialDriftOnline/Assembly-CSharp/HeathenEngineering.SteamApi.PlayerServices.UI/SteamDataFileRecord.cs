using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.PlayerServices.UI;

public class SteamDataFileRecord : Button
{
	[Header("Display Data")]
	public Text FileName;

	public Text Timestamp;

	public GameObject SelectedIndicator;

	public SteamworksRemoteStorageManager.FileAddress Address;

	[HideInInspector]
	public SteamDataFileList parentList;

	protected override void Start()
	{
		base.onClick.AddListener(HandleClick);
	}

	private void HandleClick()
	{
		if (parentList != null)
		{
			parentList.SelectedFile = Address;
		}
	}

	private void Update()
	{
		if (parentList != null && parentList.SelectedFile.HasValue && parentList.SelectedFile.Value.fileName == Address.fileName)
		{
			if (!SelectedIndicator.activeSelf)
			{
				SelectedIndicator.SetActive(value: true);
			}
		}
		else if (SelectedIndicator.activeSelf)
		{
			SelectedIndicator.SetActive(value: false);
		}
	}
}
