using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[AddComponentMenu("AudioImporter/Mobile Importer")]
public class MobileImporter : AudioImporter
{
	private UnityWebRequest webRequest;

	private AsyncOperation operation;

	public override float progress
	{
		get
		{
			if (operation == null)
			{
				return 0f;
			}
			return operation.progress;
		}
	}

	public override bool isDone
	{
		get
		{
			if (operation == null)
			{
				return false;
			}
			return operation.isDone;
		}
	}

	public override bool isInitialized => isDone;

	public override bool isError
	{
		get
		{
			if (webRequest == null)
			{
				return false;
			}
			if (!webRequest.isNetworkError)
			{
				return webRequest.isHttpError;
			}
			return true;
		}
	}

	public override string error
	{
		get
		{
			if (webRequest == null)
			{
				return string.Empty;
			}
			return webRequest.error;
		}
	}

	public override void Abort()
	{
		if (webRequest != null)
		{
			webRequest.Abort();
			webRequest.Dispose();
			webRequest = null;
			StopAllCoroutines();
		}
	}

	protected override void Import()
	{
		webRequest = UnityWebRequestMultimedia.GetAudioClip(base.uri.AbsoluteUri, AudioType.UNKNOWN);
		operation = webRequest.SendWebRequest();
		StartCoroutine(WaitForWebRequest());
	}

	private IEnumerator WaitForWebRequest()
	{
		yield return operation;
		audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
		webRequest.Dispose();
		webRequest = null;
		OnLoaded();
	}
}
