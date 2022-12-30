using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SRSrollView : MonoBehaviour
{
	private RectTransform scrollRectTransform;

	private RectTransform contentPanel;

	private RectTransform selectedRectTransform;

	private GameObject lastSelected;

	private void Start()
	{
		scrollRectTransform = GetComponent<RectTransform>();
		contentPanel = GetComponent<ScrollRect>().content;
	}

	private void Update()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (!(currentSelectedGameObject == null) && !(currentSelectedGameObject.transform.parent != contentPanel.transform) && !(currentSelectedGameObject == lastSelected))
		{
			selectedRectTransform = currentSelectedGameObject.GetComponent<RectTransform>();
			float num = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
			float y = contentPanel.anchoredPosition.y;
			float num2 = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;
			if (num > num2)
			{
				float y2 = num - scrollRectTransform.rect.height;
				contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, y2);
			}
			else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < y)
			{
				contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y));
			}
			lastSelected = currentSelectedGameObject;
		}
	}
}
