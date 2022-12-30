using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatGui))]
public class NamePickGui : MonoBehaviour
{
	private const string UserNamePlayerPref = "NamePickUserName";

	public ChatGui chatNewComponent;

	public InputField idInput;

	public GameObject OutputGo;

	public void Start()
	{
		chatNewComponent = Object.FindObjectOfType<ChatGui>();
		string.IsNullOrEmpty(PlayerPrefs.GetString("NamePickUserName"));
		StartCoroutine(Jack());
	}

	private IEnumerator Jack()
	{
		yield return new WaitForSeconds(0.5f);
		StartChat();
	}

	public void EndEditOnEnter()
	{
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
			StartChat();
		}
	}

	public void StopCoro()
	{
		StopAllCoroutines();
	}

	public void StartChat()
	{
		ChatGui chatGui = Object.FindObjectOfType<ChatGui>();
		chatGui.UserName = idInput.text.Trim();
		chatGui.Connect();
		base.enabled = false;
	}

	public void NewTextt()
	{
		StartCoroutine(NewText());
	}

	private IEnumerator NewText()
	{
		yield return new WaitForSeconds(2f);
		yield return new WaitForSeconds(1f);
		OutputGo.SetActive(value: false);
	}

	public void AnimFadeIn()
	{
	}

	public void FalseAnim()
	{
	}

	public void JusteOn()
	{
	}

	public void DPAPTEMPOENCORE()
	{
		StartCoroutine(DPAPTEMPO());
	}

	private IEnumerator DPAPTEMPO()
	{
		yield return new WaitForSeconds(0.7f);
		PlayerPrefs.SetInt("TEMPODPAD", 0);
	}
}
