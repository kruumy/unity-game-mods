using UnityEngine;
using UnityEngine.Audio;

public class RCC_CreateAudioSource : MonoBehaviour
{
	public static AudioSource NewAudioSource(AudioMixerGroup mixer, GameObject go, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished)
	{
		GameObject gameObject = new GameObject(audioName);
		if ((bool)go.transform.Find("All Audio Sources"))
		{
			gameObject.transform.SetParent(go.transform.Find("All Audio Sources"));
		}
		else
		{
			GameObject gameObject2 = new GameObject("All Audio Sources");
			gameObject2.transform.SetParent(go.transform, worldPositionStays: false);
			gameObject.transform.SetParent(gameObject2.transform, worldPositionStays: false);
		}
		gameObject.transform.position = go.transform.position;
		gameObject.transform.rotation = go.transform.rotation;
		gameObject.AddComponent<AudioSource>();
		AudioSource component = gameObject.GetComponent<AudioSource>();
		if ((bool)mixer)
		{
			component.outputAudioMixerGroup = mixer;
		}
		component.minDistance = minDistance;
		component.maxDistance = maxDistance;
		component.volume = volume;
		component.clip = audioClip;
		component.loop = loop;
		component.dopplerLevel = 0.5f;
		if (minDistance == 0f && maxDistance == 0f)
		{
			component.spatialBlend = 0f;
		}
		else
		{
			component.spatialBlend = 1f;
		}
		if (playNow)
		{
			component.playOnAwake = true;
			component.Play();
		}
		else
		{
			component.playOnAwake = false;
		}
		if (destroyAfterFinished)
		{
			if ((bool)audioClip)
			{
				Object.Destroy(gameObject, audioClip.length);
			}
			else
			{
				Object.Destroy(gameObject);
			}
		}
		return component;
	}

	public static AudioSource NewAudioSource(AudioMixerGroup mixer, GameObject go, Vector3 localPosition, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished)
	{
		GameObject gameObject = new GameObject(audioName);
		if ((bool)go.transform.Find("All Audio Sources"))
		{
			gameObject.transform.SetParent(go.transform.Find("All Audio Sources"));
		}
		else
		{
			GameObject gameObject2 = new GameObject("All Audio Sources");
			gameObject2.transform.SetParent(go.transform, worldPositionStays: false);
			gameObject.transform.SetParent(gameObject2.transform, worldPositionStays: false);
		}
		gameObject.transform.position = go.transform.position;
		gameObject.transform.rotation = go.transform.rotation;
		gameObject.transform.localPosition = localPosition;
		gameObject.AddComponent<AudioSource>();
		AudioSource component = gameObject.GetComponent<AudioSource>();
		if ((bool)mixer)
		{
			component.outputAudioMixerGroup = mixer;
		}
		component.minDistance = minDistance;
		component.maxDistance = maxDistance;
		component.volume = volume;
		component.clip = audioClip;
		component.loop = loop;
		component.dopplerLevel = 0.5f;
		if (minDistance == 0f && maxDistance == 0f)
		{
			component.spatialBlend = 0f;
		}
		else
		{
			component.spatialBlend = 1f;
		}
		if (playNow)
		{
			component.playOnAwake = true;
			component.Play();
		}
		else
		{
			component.playOnAwake = false;
		}
		if (destroyAfterFinished)
		{
			if ((bool)audioClip)
			{
				Object.Destroy(gameObject, audioClip.length);
			}
			else
			{
				Object.Destroy(gameObject);
			}
		}
		return component;
	}

	public static void NewHighPassFilter(AudioSource source, float freq, int level)
	{
		if (!(source == null))
		{
			AudioHighPassFilter audioHighPassFilter = source.gameObject.AddComponent<AudioHighPassFilter>();
			audioHighPassFilter.cutoffFrequency = freq;
			audioHighPassFilter.highpassResonanceQ = level;
		}
	}

	public static void NewLowPassFilter(AudioSource source, float freq)
	{
		if (!(source == null))
		{
			source.gameObject.AddComponent<AudioLowPassFilter>().cutoffFrequency = freq;
		}
	}
}
