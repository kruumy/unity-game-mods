using HeathenEngineering.Tools;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.GameServices;

public class HeathenWorkshopItemDisplay : HeathenUIBehaviour, IWorkshopItemDisplay, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[HideInInspector]
	public RawImage PreviewImage;

	public Text Title;

	public Vector3 TipOffset;

	public GameObject TipRoot;

	public Transform TipTransform;

	public Text Description;

	public CanvasGroup toggleGroup;

	public Toggle Subscribed;

	public Image ScoreImage;

	private bool loading;

	private bool hasMouse;

	public HeathenWorkshopReadCommunityItem Data { get; private set; }

	public void RegisterData(HeathenWorkshopReadCommunityItem data)
	{
		loading = true;
		Data = data;
		PreviewImage.texture = Data.previewImage;
		Title.text = Data.Title;
		Subscribed.isOn = Data.IsSubscribed;
		ScoreImage.fillAmount = Data.VoteScore;
		loading = false;
	}

	private void Update()
	{
		if (PreviewImage.texture != Data.previewImage)
		{
			PreviewImage.texture = Data.previewImage;
		}
		if (hasMouse)
		{
			TipTransform.position = base.selfTransform.position + TipOffset;
			toggleGroup.alpha = 1f;
			toggleGroup.interactable = true;
		}
		else
		{
			toggleGroup.alpha = 0f;
			toggleGroup.interactable = false;
		}
	}

	public void SetSubscribe(bool subscribed)
	{
		if (!loading)
		{
			if (subscribed)
			{
				SteamUGC.SubscribeItem(Data.FileId);
			}
			else
			{
				SteamUGC.UnsubscribeItem(Data.FileId);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hasMouse = true;
		TipTransform.position = base.selfTransform.position + TipOffset;
		Description.text = Data.Description.Replace("[b]", "<b>").Replace("[/b]", "</b>").Replace("[table]", "")
			.Replace("[tr]", "")
			.Replace("[td]", "")
			.Replace("[/table]", "")
			.Replace("[/tr]", "")
			.Replace("[/td]", "")
			.Replace("[h1]", "<b>")
			.Replace("[/h1]", "</b>");
		TipRoot.SetActive(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hasMouse = false;
		TipRoot.SetActive(value: false);
	}
}
