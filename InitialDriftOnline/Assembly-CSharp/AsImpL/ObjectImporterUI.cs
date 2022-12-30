using UnityEngine;
using UnityEngine.UI;

namespace AsImpL;

[RequireComponent(typeof(ObjectImporter))]
public class ObjectImporterUI : MonoBehaviour
{
	[Tooltip("Text for activity messages")]
	public Text progressText;

	[Tooltip("Slider for the overall progress")]
	public Slider progressSlider;

	[Tooltip("Panel with the Image Type set to Filled")]
	public Image progressImage;

	private ObjectImporter objImporter;

	private void Awake()
	{
		if (progressSlider != null)
		{
			progressSlider.maxValue = 100f;
			progressSlider.gameObject.SetActive(value: false);
		}
		if (progressImage != null)
		{
			progressImage.gameObject.SetActive(value: false);
		}
		if (progressText != null)
		{
			progressText.gameObject.SetActive(value: false);
		}
		objImporter = GetComponent<ObjectImporter>();
	}

	private void OnEnable()
	{
		objImporter.ImportingComplete += OnImportComplete;
		objImporter.ImportingStart += OnImportStart;
	}

	private void OnDisable()
	{
		objImporter.ImportingComplete -= OnImportComplete;
		objImporter.ImportingStart -= OnImportStart;
	}

	private void Update()
	{
		bool flag = Loader.totalProgress.singleProgress.Count > 0;
		if (!flag)
		{
			return;
		}
		int numImportRequests = objImporter.NumImportRequests;
		int num = numImportRequests - Loader.totalProgress.singleProgress.Count;
		if (flag)
		{
			float num2 = 100f * (float)num / (float)numImportRequests;
			float num3 = 0f;
			foreach (SingleLoadingProgress item in Loader.totalProgress.singleProgress)
			{
				if (num3 < item.percentage)
				{
					num3 = item.percentage;
				}
			}
			num2 += num3 / (float)numImportRequests;
			if (progressSlider != null)
			{
				progressSlider.value = num2;
				progressSlider.gameObject.SetActive(flag);
			}
			if (progressImage != null)
			{
				progressImage.fillAmount = num2 / 100f;
				progressImage.gameObject.SetActive(flag);
			}
			if (!(progressText != null))
			{
				return;
			}
			if (flag)
			{
				progressText.gameObject.SetActive(flag);
				progressText.text = "Loading " + Loader.totalProgress.singleProgress.Count + " objects...";
				string text = "";
				int num4 = 0;
				foreach (SingleLoadingProgress item2 in Loader.totalProgress.singleProgress)
				{
					if (num4 > 4)
					{
						text += "...";
						break;
					}
					if (!string.IsNullOrEmpty(item2.message))
					{
						if (num4 > 0)
						{
							text += "; ";
						}
						text += item2.message;
						num4++;
					}
				}
				if (text != "")
				{
					Text text2 = progressText;
					text2.text = text2.text + "\n" + text;
				}
			}
			else
			{
				progressText.gameObject.SetActive(value: false);
				progressText.text = "";
			}
		}
		else
		{
			OnImportComplete();
		}
	}

	private void OnImportStart()
	{
		if (progressText != null)
		{
			progressText.text = "";
		}
		if (progressSlider != null)
		{
			progressSlider.value = 0f;
			progressSlider.gameObject.SetActive(value: true);
		}
		if (progressImage != null)
		{
			progressImage.fillAmount = 0f;
			progressImage.gameObject.SetActive(value: true);
		}
	}

	private void OnImportComplete()
	{
		if (progressText != null)
		{
			progressText.text = "";
		}
		if (progressSlider != null)
		{
			progressSlider.value = 100f;
			progressSlider.gameObject.SetActive(value: false);
		}
		if (progressImage != null)
		{
			progressImage.fillAmount = 1f;
			progressImage.gameObject.SetActive(value: false);
		}
	}
}
