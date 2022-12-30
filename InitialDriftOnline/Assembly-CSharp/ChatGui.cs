using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatGui : MonoBehaviour, IChatClientListener
{
	public string[] ChannelsToJoinOnConnect;

	public string[] FriendsList;

	public int HistoryLengthToFetch;

	public Text LastLine;

	private string selectedChannelName;

	public GameObject UIlistPlayer;

	public ChatClient chatClient;

	protected internal ChatAppSettings chatAppSettings;

	private string tmptp;

	private int tempoDpad;

	private int playerlistint;

	public GameObject AnimLastMsg;

	public GameObject missingAppIdErrorPanel;

	public GameObject ConnectingLabel;

	public GameObject ZoneDeText;

	public GameObject OutputGo;

	public Text OutPutLenght;

	public RectTransform ChatPanel;

	public GameObject UserIdFormPanel;

	public InputField InputFieldChat;

	public Text CurrentChannelText;

	public Toggle ChannelToggleToInstantiate;

	private string TempText = "";

	public GameObject SaisiEncours;

	public GameObject FriendListUiItemtoInstantiate;

	private EventSystem m_EventSystem;

	private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

	private string inputchatselectedchar;

	private string DERNIERCHAR;

	private string DERNIERCHAR2;

	private readonly Dictionary<string, FriendItem> friendListItemLUT = new Dictionary<string, FriendItem>();

	public bool ShowState = true;

	public GameObject Title;

	public Text StateText;

	public Text UserIdText;

	public Text HistoriqueOnChange;

	public Button ResetchatBtn;

	private static string HelpText = "\n    -- HELP --\nTo subscribe to channel(s) (channelnames are case sensitive) :  \n\t<color=#E07B00>\\subscribe</color> <color=green><list of channelnames></color>\n\tor\n\t<color=#E07B00>\\s</color> <color=green><list of channelnames></color>\n\nTo leave channel(s):\n\t<color=#E07B00>\\unsubscribe</color> <color=green><list of channelnames></color>\n\tor\n\t<color=#E07B00>\\u</color> <color=green><list of channelnames></color>\n\nTo switch the active channel\n\t<color=#E07B00>\\join</color> <color=green><channelname></color>\n\tor\n\t<color=#E07B00>\\j</color> <color=green><channelname></color>\n\nTo send a private message: (username are case sensitive)\n\t\\<color=#E07B00>msg</color> <color=green><username></color> <color=green><message></color>\n\nTo change status:\n\t\\<color=#E07B00>state</color> <color=green><stateIndex></color> <color=green><message></color>\n<color=green>0</color> = Offline <color=green>1</color> = Invisible <color=green>2</color> = Online <color=green>3</color> = Away \n<color=green>4</color> = Do not disturb <color=green>5</color> = Looking For Group <color=green>6</color> = Playing\n\nTo clear the current chat tab (private chats get closed):\n\t<color=#E07B00>\\clear</color>";

	public int TestLength = 2048;

	private byte[] testBytes = new byte[2048];

	public string UserName { get; set; }

	public void Start()
	{
		playerlistint = 0;
		inputchatselectedchar = "";
		if (PlayerPrefs.GetInt("OneLoad") == 20)
		{
			PlayerPrefs.SetInt("InputOn", 0);
		}
		if (PlayerPrefs.GetInt("OneLoad") == 0)
		{
			HistoriqueOnChange.text = PlayerPrefs.GetString("HistoriqueDesMessages");
			PlayerPrefs.SetString("DERNIERMESSAGE", "");
		}
		if (PlayerPrefs.GetInt("InputOn") == 10)
		{
			OutputGo.SetActive(value: true);
			ZoneDeText.SetActive(value: true);
			LastLine.gameObject.SetActive(value: false);
		}
		else if (PlayerPrefs.GetInt("InputOn") == 0)
		{
			LastLine.gameObject.SetActive(value: true);
			OutputGo.SetActive(value: false);
			ZoneDeText.SetActive(value: false);
		}
		UserIdText.text = "";
		StateText.text = "";
		Title.SetActive(value: true);
		ConnectingLabel.SetActive(value: false);
		if (string.IsNullOrEmpty(UserName))
		{
			UserName = "user" + Environment.TickCount % 99;
		}
		chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
		bool flag = !string.IsNullOrEmpty(chatAppSettings.AppId);
		missingAppIdErrorPanel.SetActive(!flag);
		if (!flag)
		{
			Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
		}
		StartCoroutine(AllowBtnReset());
	}

	private IEnumerator AllowBtnReset()
	{
		yield return new WaitForSeconds(60f);
		ResetchatBtn.interactable = true;
	}

	public void UpdateChat()
	{
		chatClient.Disconnect();
		StartCoroutine(Resetchat());
	}

	private IEnumerator Resetchat()
	{
		PlayerPrefs.SetString("HistoriqueDesMessages", "            | |     | |\n                \\/            INITIAL DRIFT ONLINE\n             \\____/  \n\n\n\n\n" + PlayerPrefs.GetString("DERNIERMESSAGEPLUS") + "\n");
		yield return new WaitForSeconds(1f);
		Start();
		yield return new WaitForSeconds(1f);
		Connect();
		yield return new WaitForSeconds(30f);
		ResetchatBtn.interactable = true;
	}

	public void Connect()
	{
		UserIdFormPanel.gameObject.SetActive(value: false);
		chatClient = new ChatClient(this);
		if (PlayerPrefs.GetString("SelectedRegion") == "sa")
		{
			chatClient.ChatRegion = "us";
		}
		chatClient.UseBackgroundWorkerForSending = true;
		chatClient.AuthValues = new Photon.Chat.AuthenticationValues(UserName);
		chatClient.ConnectUsingSettings(chatAppSettings);
		ChannelToggleToInstantiate.gameObject.SetActive(value: false);
		ConnectingLabel.SetActive(value: true);
		Debug.Log("REGIOON : " + chatClient.ChatRegion);
	}

	public void OnDestroy()
	{
		if (chatClient != null)
		{
			chatClient.Disconnect();
		}
	}

	public void OnApplicationQuit()
	{
		if (chatClient != null)
		{
			chatClient.Disconnect();
		}
	}

	public void Update()
	{
		if (chatClient != null)
		{
			chatClient.Service();
		}
		if (StateText == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		string text = OutPutLenght.text;
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (ZoneDeText.activeSelf && SaisiEncours.GetComponent<Text>().text == "")
			{
				TempText = text;
				OutputGo.SetActive(value: false);
				ZoneDeText.SetActive(value: false);
				AnimLastMsg.GetComponent<Animator>().Play("LastMessage");
				GetComponent<NamePickGui>().FalseAnim();
				GetComponent<NamePickGui>().StopCoro();
			}
			else
			{
				GetComponent<NamePickGui>().JusteOn();
				ZoneDeText.SetActive(value: true);
				InputFieldChat.ActivateInputField();
			}
		}
		if (InputFieldChat.isFocused)
		{
			ObscuredPrefs.SetInt("ONTYPING", 10);
		}
		else
		{
			ObscuredPrefs.SetInt("ONTYPING", 0);
		}
		if (Input.GetKeyDown(KeyCode.Escape) && ZoneDeText.activeSelf)
		{
			TempText = text;
			AnimLastMsg.GetComponent<Animator>().Play("LastMessage");
			OutputGo.SetActive(value: false);
			ZoneDeText.SetActive(value: false);
			GetComponent<NamePickGui>().FalseAnim();
			OffPlayerlist();
		}
		if (ZoneDeText.activeSelf)
		{
			PlayerPrefs.SetInt("InputOn", 10);
			GetComponent<NamePickGui>().JusteOn();
			OutputGo.SetActive(value: true);
			TempText = text;
			LastLine.gameObject.SetActive(value: false);
		}
		else
		{
			LastLine.gameObject.SetActive(value: true);
			PlayerPrefs.SetInt("InputOn", 0);
		}
		if (!ZoneDeText.activeSelf)
		{
			if (text != TempText)
			{
				TempText = text;
			}
		}
		else
		{
			TempText = text;
		}
		if (InputFieldChat.text.Length >= 2)
		{
			inputchatselectedchar = InputFieldChat.text.Substring(InputFieldChat.text.Length - 2);
		}
		if (InputFieldChat.text.Contains("@"))
		{
			if ((inputchatselectedchar.Contains(" @") && playerlistint == 0) || (InputFieldChat.text.Length == 1 && InputFieldChat.text == "@" && playerlistint == 0))
			{
				playerlistint = 1;
				GameObject[] array = GameObject.FindGameObjectsWithTag("3DPSEUDO");
				GameObject[] array2 = GameObject.FindGameObjectsWithTag("identifyplayer");
				int num = 0;
				GameObject[] array3 = array;
				foreach (GameObject gameObject in array3)
				{
					array2[num].GetComponentInParent<Image>().enabled = true;
					array2[num].GetComponentInParent<Button>().interactable = true;
					array2[num].GetComponent<Text>().text = gameObject.GetComponent<TextMeshPro>().text;
					num++;
				}
			}
			else if (playerlistint == 1 && InputFieldChat.text != "@" && !inputchatselectedchar.Contains(" @"))
			{
				UIlistPlayer.GetComponentInChildren<Text>().text = "";
				playerlistint = 0;
				GameObject[] array3 = GameObject.FindGameObjectsWithTag("identifyplayer");
				foreach (GameObject obj in array3)
				{
					obj.GetComponentInParent<Image>().enabled = false;
					obj.GetComponentInParent<Button>().interactable = false;
					obj.GetComponent<Text>().text = "";
				}
			}
		}
		else if (playerlistint == 1)
		{
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("identifyplayer");
			foreach (GameObject obj2 in array3)
			{
				obj2.GetComponentInParent<Image>().enabled = false;
				obj2.GetComponentInParent<Button>().interactable = false;
				obj2.GetComponent<Text>().text = "";
			}
			playerlistint = 0;
		}
	}

	public void IdentifyThisMen(GameObject Targettxtmen)
	{
		string text = Targettxtmen.GetComponentInChildren<Text>().text.Replace("<color=#F3E400>", "").Replace("<color=#ACACAC>", "").Replace("</color>", "");
		string text2 = text;
		string text3 = text.Substring(text.Length - (text.Length - 4));
		if (text2.Replace(text3, "").Contains("] ") && text2.Replace(text3, "").Contains("["))
		{
			text2 = text3;
			InputFieldChat.text = InputFieldChat.text.Replace("@", " ") + text2 + " ";
			InputFieldChat.Select();
			StartCoroutine(endcaret());
			PlayerPrefs.SetString("ReplacetxtColor", text2);
			return;
		}
		text3 = text.Substring(text.Length - (text.Length - 5));
		if (text2.Replace(text3, "").Contains("] ") && text2.Replace(text3, "").Contains("["))
		{
			text2 = text3;
		}
		else
		{
			text3 = text.Substring(text.Length - (text.Length - 6));
			text2 = text3;
		}
		InputFieldChat.text = (InputFieldChat.text.Replace("@", "") + text2) ?? "";
		InputFieldChat.Select();
		StartCoroutine(endcaret());
		PlayerPrefs.SetString("ReplacetxtColor", text2);
	}

	private IEnumerator endcaret()
	{
		yield return new WaitForEndOfFrame();
		InputFieldChat.caretPosition = InputFieldChat.text.Length;
		yield return new WaitForSeconds(0.05f);
		InputFieldChat.caretPosition = InputFieldChat.text.Length;
		InputFieldChat.ForceLabelUpdate();
	}

	public void OffPlayerlist()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("identifyplayer");
		foreach (GameObject obj in array)
		{
			obj.GetComponentInParent<Image>().enabled = false;
			obj.GetComponentInParent<Button>().interactable = false;
			obj.GetComponent<Text>().text = "";
		}
	}

	public void ChangeMapChat()
	{
		PlayerPrefs.SetString("HistoriqueDesMessages", OutPutLenght.text);
	}

	public void OnEnterSend()
	{
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
			if (PlayerPrefs.GetString("ReplacetxtColor") != "")
			{
				InputFieldChat.text = InputFieldChat.text.Replace(PlayerPrefs.GetString("ReplacetxtColor"), "<color=yellow>" + PlayerPrefs.GetString("ReplacetxtColor") + "</color>");
				SendChatMessage(InputFieldChat.text);
				InputFieldChat.text = "";
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendNotificationChat(PlayerPrefs.GetString("ReplacetxtColor"));
				PlayerPrefs.SetString("ReplacetxtColor", "");
			}
			else
			{
				SendChatMessage(InputFieldChat.text);
				InputFieldChat.text = "";
			}
		}
	}

	public void OnClickSend()
	{
		if (InputFieldChat != null)
		{
			if (PlayerPrefs.GetString("ReplacetxtColor") != "")
			{
				InputFieldChat.text = InputFieldChat.text.Replace(PlayerPrefs.GetString("ReplacetxtColor"), "<color=yellow>" + PlayerPrefs.GetString("ReplacetxtColor") + "</color>");
				SendChatMessage(InputFieldChat.text);
				InputFieldChat.text = "";
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendNotificationChat(PlayerPrefs.GetString("ReplacetxtColor"));
				PlayerPrefs.SetString("ReplacetxtColor", "");
			}
			else
			{
				SendChatMessage(InputFieldChat.text);
				InputFieldChat.text = "";
			}
		}
	}

	private void SendChatMessage(string inputLine)
	{
		if (string.IsNullOrEmpty(inputLine))
		{
			return;
		}
		if ("test".Equals(inputLine))
		{
			if (TestLength != testBytes.Length)
			{
				testBytes = new byte[TestLength];
			}
			chatClient.SendPrivateMessage(chatClient.AuthValues.UserId, testBytes, forwardAsWebhook: true);
		}
		bool flag = chatClient.PrivateChannels.ContainsKey(selectedChannelName);
		string target = string.Empty;
		if (flag)
		{
			target = selectedChannelName.Split(':')[1];
		}
		if (inputLine[0].Equals('\\'))
		{
			string[] array = inputLine.Split(new char[1] { ' ' }, 2);
			if (array[0].Equals("\\help"))
			{
				PostHelpToCurrentChannel();
			}
			if (array[0].Equals("\\state"))
			{
				int num = 0;
				List<string> list = new List<string>();
				list.Add("i am state " + num);
				string[] array2 = array[1].Split(' ', ',');
				if (array2.Length != 0)
				{
					num = int.Parse(array2[0]);
				}
				if (array2.Length > 1)
				{
					list.Add(array2[1]);
				}
				chatClient.SetOnlineStatus(num, list.ToArray());
			}
			else if ((array[0].Equals("\\subscribe") || array[0].Equals("\\s")) && !string.IsNullOrEmpty(array[1]))
			{
				chatClient.Subscribe(array[1].Split(' ', ','));
			}
			else if ((array[0].Equals("\\unsubscribe") || array[0].Equals("\\u")) && !string.IsNullOrEmpty(array[1]))
			{
				chatClient.Unsubscribe(array[1].Split(' ', ','));
			}
			else if (array[0].Equals("\\clear"))
			{
				ChatChannel channel;
				if (flag)
				{
					chatClient.PrivateChannels.Remove(selectedChannelName);
				}
				else if (chatClient.TryGetChannel(selectedChannelName, flag, out channel))
				{
					channel.ClearMessages();
				}
			}
			else if (array[0].Equals("\\msg") && !string.IsNullOrEmpty(array[1]))
			{
				string[] array3 = array[1].Split(new char[2] { ' ', ',' }, 2);
				if (array3.Length >= 2)
				{
					string target2 = array3[0];
					string message = array3[1];
					chatClient.SendPrivateMessage(target2, message);
				}
			}
			else if ((array[0].Equals("\\join") || array[0].Equals("\\j")) && !string.IsNullOrEmpty(array[1]))
			{
				string[] array4 = array[1].Split(new char[2] { ' ', ',' }, 2);
				if (channelToggles.ContainsKey(array4[0]))
				{
					ShowChannel(array4[0]);
					return;
				}
				chatClient.Subscribe(new string[1] { array4[0] });
			}
			else
			{
				Debug.Log("The command '" + array[0] + "' is invalid.");
			}
		}
		else if (flag)
		{
			chatClient.SendPrivateMessage(target, inputLine);
		}
		else
		{
			chatClient.PublishMessage(selectedChannelName, inputLine);
		}
	}

	public void PostHelpToCurrentChannel()
	{
		CurrentChannelText.text += HelpText;
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		switch (level)
		{
		case DebugLevel.ERROR:
			Debug.LogError(message);
			break;
		case DebugLevel.WARNING:
			Debug.LogWarning(message);
			break;
		default:
			Debug.Log(message);
			break;
		}
	}

	public void OnConnected()
	{
		if (ChannelsToJoinOnConnect != null && ChannelsToJoinOnConnect.Length != 0)
		{
			chatClient.Subscribe(ChannelsToJoinOnConnect, HistoryLengthToFetch);
		}
		ConnectingLabel.SetActive(value: false);
		UserIdText.text = "Connected as " + UserName;
		ChatPanel.gameObject.SetActive(value: true);
		if (FriendsList != null && FriendsList.Length != 0)
		{
			chatClient.AddFriends(FriendsList);
			string[] friendsList = FriendsList;
			foreach (string text in friendsList)
			{
				if (FriendListUiItemtoInstantiate != null && text != UserName)
				{
					InstantiateFriendButton(text);
				}
			}
		}
		if (FriendListUiItemtoInstantiate != null)
		{
			FriendListUiItemtoInstantiate.SetActive(value: false);
		}
		chatClient.SetOnlineStatus(2);
	}

	public void OnDisconnected()
	{
		ConnectingLabel.SetActive(value: false);
	}

	public void OnChatStateChange(ChatState state)
	{
		StateText.text = state.ToString();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		foreach (string channelName in channels)
		{
			if (PlayerPrefs.GetInt("OneLoad") == 20)
			{
				PlayerPrefs.SetInt("IcomeFromOtherMap", 0);
				if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka"))
				{
					chatClient.PublishMessage(channelName, "join the run !");
				}
				PlayerPrefs.SetInt("OneLoad", 0);
			}
			if (ChannelToggleToInstantiate != null)
			{
				InstantiateChannelButton(channelName);
			}
		}
		ShowChannel(channels[0]);
	}

	public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
	{
		Debug.LogFormat("OnSubscribed: {0}, users.Count: {1} Channel-props: {2}.", channel, users.Length, properties.ToStringFull());
	}

	private void InstantiateChannelButton(string channelName)
	{
		if (channelToggles.ContainsKey(channelName))
		{
			Debug.Log("Skipping creation for an existing channel toggle.");
			return;
		}
		Toggle toggle = UnityEngine.Object.Instantiate(ChannelToggleToInstantiate);
		toggle.gameObject.SetActive(value: true);
		toggle.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
		toggle.transform.SetParent(ChannelToggleToInstantiate.transform.parent, worldPositionStays: false);
		channelToggles.Add(channelName, toggle);
	}

	private void InstantiateFriendButton(string friendId)
	{
		GameObject obj = UnityEngine.Object.Instantiate(FriendListUiItemtoInstantiate);
		obj.gameObject.SetActive(value: true);
		FriendItem component = obj.GetComponent<FriendItem>();
		component.FriendId = friendId;
		obj.transform.SetParent(FriendListUiItemtoInstantiate.transform.parent, worldPositionStays: false);
		friendListItemLUT[friendId] = component;
	}

	public void OnUnsubscribed(string[] channels)
	{
		foreach (string text in channels)
		{
			if (channelToggles.ContainsKey(text))
			{
				UnityEngine.Object.Destroy(channelToggles[text].gameObject);
				channelToggles.Remove(text);
				Debug.Log("Unsubscribed from channel '" + text + "'.");
				if (text == selectedChannelName && channelToggles.Count > 0)
				{
					IEnumerator<KeyValuePair<string, Toggle>> enumerator = channelToggles.GetEnumerator();
					enumerator.MoveNext();
					ShowChannel(enumerator.Current.Key);
					enumerator.Current.Value.isOn = true;
				}
			}
			else
			{
				Debug.Log("Can't unsubscribe from channel '" + text + "' because you are currently not subscribed to it.");
			}
		}
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		if (channelName.Equals(selectedChannelName))
		{
			ShowChannel(selectedChannelName);
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		InstantiateChannelButton(channelName);
		if (message is byte[] array)
		{
			Debug.Log("Message with byte[].Length: " + array.Length);
		}
		if (selectedChannelName.Equals(channelName))
		{
			ShowChannel(channelName);
		}
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		Debug.LogWarning("status: " + $"{user} is {status}. Msg:{message}");
		if (friendListItemLUT.ContainsKey(user))
		{
			FriendItem friendItem = friendListItemLUT[user];
			if (friendItem != null)
			{
				friendItem.OnFriendStatusUpdate(status, gotMessage, message);
			}
		}
	}

	public void OnUserSubscribed(string channel, string user)
	{
		Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
	}

	public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
	{
		Debug.LogFormat("OnChannelPropertiesChanged: {0} by {1}. Props: {2}.", channel, userId, properties.ToStringFull());
	}

	public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
	{
		Debug.LogFormat("OnUserPropertiesChanged: (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, properties.ToStringFull());
	}

	public void OnErrorInfo(string channel, string error, object data)
	{
		Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
	}

	public void AddMessageToSelectedChannel(string msg)
	{
		ChatChannel channel = null;
		if (!chatClient.TryGetChannel(selectedChannelName, out channel))
		{
			Debug.Log("AddMessageToSelectedChannel failed to find channel: " + selectedChannelName);
		}
		else
		{
			channel?.Add("Bot", msg, 0);
		}
	}

	public void ShowChannel(string channelName)
	{
		channelName = "Region";
		if (string.IsNullOrEmpty(channelName))
		{
			return;
		}
		ChatChannel channel = null;
		if (!chatClient.TryGetChannel(channelName, out channel))
		{
			Debug.Log("ShowChannel failed to find channel: " + channelName);
			return;
		}
		selectedChannelName = channelName;
		string text = PlayerPrefs.GetString("HistoriqueDesMessages") + channel.ToStringMessages();
		GameObject.FindGameObjectsWithTag("3DPSEUDO");
		CurrentChannelText.text = text;
		if (CurrentChannelText.text.Length > 3000)
		{
			UpdateChat();
			ResetchatBtn.interactable = false;
		}
		LastLine.text = PlayerPrefs.GetString("DERNIERMESSAGE");
		if (ObscuredPrefs.GetBool("TogglePopUpChat"))
		{
			AnimLastMsg.GetComponent<Animator>().Play("Reset");
			StartCoroutine(NewMessage());
		}
		foreach (KeyValuePair<string, Toggle> channelToggle in channelToggles)
		{
			channelToggle.Value.isOn = ((channelToggle.Key == channelName) ? true : false);
		}
	}

	private IEnumerator NewMessage()
	{
		yield return new WaitForSeconds(0.3f);
		AnimLastMsg.GetComponent<Animator>().Play("LastMessage");
	}

	public void OpenDashboard()
	{
		Application.OpenURL("https://dashboard.photonengine.com");
	}
}
