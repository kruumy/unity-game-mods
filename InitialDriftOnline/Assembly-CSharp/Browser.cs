using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Browser : MonoBehaviour
{
	public List<string> extensions;

	public GameObject listItemPrefab;

	public GameObject upButton;

	public ScrollRect scrollRect;

	public GameObject folderPanel;

	public GameObject filePanel;

	private string currentDirectory;

	private string[] drives;

	private List<string> directories;

	private List<string> files;

	private bool selectDrive;

	private bool scrolling;

	public InputField PathForUI;

	public event Action<string> FileSelected;

	private void Awake()
	{
		directories = new List<string>();
		files = new List<string>();
		drives = Directory.GetLogicalDrives();
		currentDirectory = PlayerPrefs.GetString("currentDirectory", "");
		PathForUI.text = currentDirectory ?? "";
		selectDrive = string.IsNullOrEmpty(currentDirectory) || !Directory.Exists(currentDirectory);
		BuildContent();
	}

	public void Up()
	{
		if (currentDirectory == Path.GetPathRoot(currentDirectory))
		{
			PathForUI.text = currentDirectory ?? "";
			selectDrive = true;
			ClearContent();
			BuildContent();
		}
		else
		{
			currentDirectory = Directory.GetParent(currentDirectory).FullName;
			PathForUI.text = currentDirectory ?? "";
			ClearContent();
			BuildContent();
		}
		PathForUI.caretPosition = PathForUI.text.Length;
		StartCoroutine(endcaret());
	}

	public void BuildContent()
	{
		directories.Clear();
		files.Clear();
		if (selectDrive)
		{
			directories.AddRange(drives);
			StopAllCoroutines();
			StartCoroutine(refreshDirectories());
			return;
		}
		try
		{
			directories.AddRange(Directory.GetDirectories(currentDirectory));
			string[] array = Directory.GetFiles(currentDirectory);
			foreach (string text in array)
			{
				if (extensions.Contains(Path.GetExtension(text)))
				{
					files.Add(text);
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
		}
		StopAllCoroutines();
		StartCoroutine(refreshFiles());
		StartCoroutine(refreshDirectories());
		EventSystem.current.SetSelectedGameObject(upButton);
	}

	public void ClearContent()
	{
		Button[] componentsInChildren = filePanel.GetComponentsInChildren<Button>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		componentsInChildren = folderPanel.GetComponentsInChildren<Button>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	private void OnFileSelected(int index)
	{
		string obj = files[index];
		if (this.FileSelected != null)
		{
			this.FileSelected(obj);
		}
		PlayerPrefs.SetString("currentDirectory", currentDirectory);
	}

	private void OnDirectorySelected(int index)
	{
		if (selectDrive)
		{
			currentDirectory = drives[index];
			selectDrive = false;
			PathForUI.text = currentDirectory ?? "";
			PathForUI.caretPosition = PathForUI.text.Length;
		}
		else
		{
			currentDirectory = directories[index];
			PathForUI.text = currentDirectory ?? "";
			PathForUI.caretPosition = PathForUI.text.Length;
		}
		StartCoroutine(endcaret());
		ClearContent();
		BuildContent();
	}

	private IEnumerator endcaret()
	{
		yield return new WaitForEndOfFrame();
		PathForUI.caretPosition = PathForUI.text.Length;
		yield return new WaitForSeconds(0.05f);
		PathForUI.caretPosition = PathForUI.text.Length;
		PathForUI.ForceLabelUpdate();
	}

	private IEnumerator refreshFiles()
	{
		for (int i = 0; i < files.Count; i++)
		{
			AddFileItem(i);
			yield return null;
		}
	}

	private IEnumerator refreshDirectories()
	{
		for (int i = 0; i < directories.Count; i++)
		{
			AddDirectoryItem(i);
			yield return null;
		}
	}

	private void AddFileItem(int index)
	{
		GameObject obj = UnityEngine.Object.Instantiate(listItemPrefab);
		obj.GetComponent<Button>().onClick.AddListener(delegate
		{
			OnFileSelected(index);
			Debug.Log("CLICK");
		});
		obj.GetComponentInChildren<Text>().text = Path.GetFileName(files[index]);
		obj.transform.SetParent(filePanel.transform, worldPositionStays: false);
	}

	private void AddDirectoryItem(int index)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(listItemPrefab);
		gameObject.GetComponent<Button>().onClick.AddListener(delegate
		{
			OnDirectorySelected(index);
			Debug.Log("CLICK FOLDER");
		});
		if (selectDrive)
		{
			gameObject.GetComponentInChildren<Text>().text = directories[index];
		}
		else
		{
			gameObject.GetComponentInChildren<Text>().text = Path.GetFileName(directories[index]);
		}
		gameObject.transform.SetParent(folderPanel.transform, worldPositionStays: false);
	}

	private void Update()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		scrollRect.movementType = ScrollRect.MovementType.Elastic;
		if (!(currentSelectedGameObject != null) || !currentSelectedGameObject.transform.IsChildOf(base.transform))
		{
			return;
		}
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.3f)
		{
			if (currentSelectedGameObject.transform.IsChildOf(base.transform))
			{
				scrollRect.movementType = ScrollRect.MovementType.Clamped;
				RectTransform component = currentSelectedGameObject.GetComponent<RectTransform>();
				Vector2 vector = scrollRect.transform.position - component.position;
				if (Mathf.Abs(vector.y) > 0.5f)
				{
					Vector2 zero = Vector2.zero;
					zero.y = vector.y * 3f;
					scrollRect.velocity = zero;
				}
				scrolling = true;
			}
		}
		else if (scrolling)
		{
			if (scrollRect.verticalNormalizedPosition > 0.99f || scrollRect.verticalNormalizedPosition < 0.01f)
			{
				scrollRect.StopMovement();
			}
			scrolling = false;
		}
	}
}
