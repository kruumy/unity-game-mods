using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class IconPosition : MonoBehaviour
{
	private bool OneshotAnim;

	private bool OneshotAnim2;

	public Text NewMessage;

	private Color ColorDeBaseNewMessage;

	public GameObject Icon;

	private void Start()
	{
		ColorDeBaseNewMessage = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		OneshotAnim = true;
	}

	private void Update()
	{
		if (PlayerPrefs.GetInt("InputOn") == 10 && OneshotAnim)
		{
			OneshotAnim = false;
			GetComponent<Animator>().Play("IconPositionRight");
		}
		else if (PlayerPrefs.GetInt("InputOn") == 0)
		{
			OneshotAnim = true;
			GetComponent<Animator>().Play("IconPositionNormal");
		}
		if (NewMessage.color != ColorDeBaseNewMessage && OneshotAnim2)
		{
			OneshotAnim2 = false;
			Icon.SetActive(value: false);
		}
		else if (PlayerPrefs.GetInt("InputOn") == 0 && NewMessage.color == ColorDeBaseNewMessage && !OneshotAnim2)
		{
			if (Icon.gameObject.transform.name == "Running_Icon_bg" && PlayerPrefs.GetInt("ImInRun") == 1)
			{
				OneshotAnim2 = true;
				Icon.SetActive(value: true);
			}
			else if (Icon.gameObject.transform.name == "Tofu_Icon_bg" && ObscuredPrefs.GetBool("TOFU RUN"))
			{
				OneshotAnim2 = true;
				Icon.SetActive(value: true);
			}
		}
	}
}
