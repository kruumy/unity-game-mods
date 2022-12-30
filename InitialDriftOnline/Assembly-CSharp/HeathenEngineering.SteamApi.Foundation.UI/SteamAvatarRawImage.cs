using HeathenEngineering.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Foundation.UI;

[RequireComponent(typeof(RawImage))]
public class SteamAvatarRawImage : HeathenUIBehaviour
{
	private RawImage image;

	public SteamUserData userData;

	private void Awake()
	{
		image = GetComponent<RawImage>();
		LinkSteamUser(userData);
	}

	public void LinkSteamUser(SteamUserData newUserData)
	{
		if (userData != null)
		{
			userData.OnAvatarChanged.RemoveListener(handleAvatarChange);
		}
		userData = newUserData;
		if (userData != null)
		{
			if (image == null)
			{
				image = GetComponent<RawImage>();
			}
			image.texture = userData.avatar;
			userData.OnAvatarChanged.AddListener(handleAvatarChange);
		}
	}

	private void handleAvatarChange()
	{
		image.texture = userData.avatar;
	}

	private void OnDestroy()
	{
		if (userData != null)
		{
			userData.OnAvatarChanged.RemoveListener(handleAvatarChange);
		}
	}
}
