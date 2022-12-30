using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
	public Text NameLabel;

	public Text StatusLabel;

	public Text Health;

	[HideInInspector]
	public string FriendId
	{
		get
		{
			return NameLabel.text;
		}
		set
		{
			NameLabel.text = value;
		}
	}

	public void Awake()
	{
		Health.text = string.Empty;
	}

	public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
	{
		StatusLabel.text = status switch
		{
			1 => "Invisible", 
			2 => "Online", 
			3 => "Away", 
			4 => "Do not disturb", 
			5 => "Looking For Game/Group", 
			6 => "Playing", 
			_ => "Offline", 
		};
		if (gotMessage)
		{
			string text = string.Empty;
			if (message != null && message is string[] array && array.Length >= 2)
			{
				text = array[1] + "%";
			}
			Health.text = text;
		}
	}
}
