using HeathenEngineering.Scriptable;
using HeathenEngineering.Tools;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Foundation.UI;

public class SteamUserFullIcon : HeathenUIBehaviour
{
	[FormerlySerializedAs("SteamSettings")]
	public SteamSettings steamSettings;

	[FormerlySerializedAs("UserData")]
	public SteamUserData userData;

	[FormerlySerializedAs("ShowStatusLabel")]
	public BoolReference showStatusLabel;

	[Header("References")]
	[FormerlySerializedAs("Avatar")]
	public RawImage avatar;

	[FormerlySerializedAs("PersonaName")]
	public Text personaName;

	[FormerlySerializedAs("StatusLabel")]
	public Text statusLabel;

	[FormerlySerializedAs("IconBorder")]
	public Image iconBorder;

	[FormerlySerializedAs("StatusLabelContainer")]
	public GameObject statusLabelContainer;

	[FormerlySerializedAs("ColorThePersonaName")]
	public bool colorThePersonaName = true;

	[FormerlySerializedAs("ColorTheStatusLabel")]
	public bool colorTheStatusLabel = true;

	[Header("Border Colors")]
	[FormerlySerializedAs("OfflineColor")]
	public ColorReference offlineColor;

	[FormerlySerializedAs("OnlineColor")]
	public ColorReference onlineColor;

	[FormerlySerializedAs("AwayColor")]
	public ColorReference awayColor;

	[FormerlySerializedAs("BuisyColor")]
	public ColorReference buisyColor;

	[FormerlySerializedAs("SnoozeColor")]
	public ColorReference snoozeColor;

	[FormerlySerializedAs("WantPlayColor")]
	public ColorReference wantPlayColor;

	[FormerlySerializedAs("WantTradeColor")]
	public ColorReference wantTradeColor;

	[FormerlySerializedAs("InGameColor")]
	public ColorReference inGameColor;

	[FormerlySerializedAs("ThisGameColor")]
	public ColorReference thisGameColor;

	private void Start()
	{
		if (userData != null)
		{
			LinkSteamUser(userData);
		}
	}

	private void Update()
	{
		if (showStatusLabel.Value != statusLabelContainer.activeSelf)
		{
			statusLabelContainer.SetActive(showStatusLabel.Value);
		}
	}

	public void LinkSteamUser(SteamUserData newUserData)
	{
		if (userData != null)
		{
			if (userData.OnAvatarChanged != null)
			{
				userData.OnAvatarChanged.RemoveListener(handleAvatarChange);
			}
			if (userData.OnStateChange != null)
			{
				userData.OnStateChange.RemoveListener(handleStateChange);
			}
			if (userData.OnNameChanged != null)
			{
				userData.OnNameChanged.RemoveListener(handleNameChanged);
			}
			if (userData.OnAvatarLoaded != null)
			{
				userData.OnAvatarLoaded.RemoveListener(handleAvatarChange);
			}
		}
		userData = newUserData;
		handleAvatarChange();
		handleNameChanged();
		handleStateChange();
		if (userData != null)
		{
			if (!userData.iconLoaded)
			{
				steamSettings.client.RefreshAvatar(userData);
			}
			avatar.texture = userData.avatar;
			if (userData.OnAvatarChanged == null)
			{
				userData.OnAvatarChanged = new UnityEvent();
			}
			userData.OnAvatarChanged.AddListener(handleAvatarChange);
			if (userData.OnStateChange == null)
			{
				userData.OnStateChange = new UnityEvent();
			}
			userData.OnStateChange.AddListener(handleStateChange);
			if (userData.OnNameChanged == null)
			{
				userData.OnNameChanged = new UnityEvent();
			}
			userData.OnNameChanged.AddListener(handleNameChanged);
			if (userData.OnAvatarLoaded == null)
			{
				userData.OnAvatarLoaded = new UnityEvent();
			}
			userData.OnAvatarLoaded.AddListener(handleAvatarChange);
		}
	}

	private void handleNameChanged()
	{
		personaName.text = userData.DisplayName;
	}

	private void handleAvatarChange()
	{
		avatar.texture = userData.avatar;
	}

	private void handleStateChange()
	{
		switch (userData.State)
		{
		case EPersonaState.k_EPersonaStateAway:
			if (userData.InGame)
			{
				if (userData.GameInfo.m_gameID.AppID().m_AppId == steamSettings.applicationId.m_AppId)
				{
					statusLabel.text = "Playing";
					iconBorder.color = thisGameColor.Value;
				}
				else
				{
					statusLabel.text = "In-Game";
					iconBorder.color = inGameColor.Value;
				}
			}
			else
			{
				statusLabel.text = "Away";
				iconBorder.color = awayColor.Value;
			}
			break;
		case EPersonaState.k_EPersonaStateBusy:
			if (userData.InGame)
			{
				if (userData.GameInfo.m_gameID.AppID().m_AppId == steamSettings.applicationId.m_AppId)
				{
					statusLabel.text = "Playing";
					iconBorder.color = thisGameColor.Value;
				}
				else
				{
					statusLabel.text = "In-Game";
					iconBorder.color = inGameColor.Value;
				}
			}
			else
			{
				statusLabel.text = "Buisy";
				iconBorder.color = buisyColor.Value;
			}
			break;
		case EPersonaState.k_EPersonaStateLookingToPlay:
			statusLabel.text = "Looking to Play";
			iconBorder.color = wantPlayColor.Value;
			break;
		case EPersonaState.k_EPersonaStateLookingToTrade:
			statusLabel.text = "Looking to Trade";
			iconBorder.color = wantTradeColor.Value;
			break;
		case EPersonaState.k_EPersonaStateOffline:
			statusLabel.text = "Offline";
			iconBorder.color = offlineColor.Value;
			break;
		case EPersonaState.k_EPersonaStateOnline:
			if (userData.InGame)
			{
				if (userData.GameInfo.m_gameID.AppID().m_AppId == steamSettings.applicationId.m_AppId)
				{
					statusLabel.text = "Playing";
					iconBorder.color = thisGameColor.Value;
				}
				else
				{
					statusLabel.text = "In-Game";
					iconBorder.color = inGameColor.Value;
				}
			}
			else
			{
				statusLabel.text = "Online";
				iconBorder.color = onlineColor.Value;
			}
			break;
		case EPersonaState.k_EPersonaStateSnooze:
			if (userData.InGame)
			{
				if (userData.GameInfo.m_gameID.AppID().m_AppId == steamSettings.applicationId.m_AppId)
				{
					statusLabel.text = "Playing";
					iconBorder.color = thisGameColor.Value;
				}
				else
				{
					statusLabel.text = "In-Game";
					iconBorder.color = inGameColor.Value;
				}
			}
			else
			{
				statusLabel.text = "Snooze";
				iconBorder.color = snoozeColor.Value;
			}
			break;
		}
		if (colorTheStatusLabel)
		{
			statusLabel.color = iconBorder.color;
		}
		if (colorThePersonaName)
		{
			personaName.color = iconBorder.color;
		}
	}

	private void OnDestroy()
	{
		if (userData != null)
		{
			userData.OnAvatarChanged.RemoveListener(handleAvatarChange);
		}
	}
}
