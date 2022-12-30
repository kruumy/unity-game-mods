using System;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	public Animator[] animators;

	public void SwapVisibility(GameObject obj)
	{
		obj.SetActive(!obj.activeSelf);
	}

	public void SetFloat(string parameter = "key,value")
	{
		char[] separator = new char[2] { ',', ';' };
		string[] array = parameter.Split(separator);
		string text = array[0];
		float num = (float)Convert.ToDouble(array[1]);
		Debug.Log(text + " " + num);
		Animator[] array2 = animators;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetFloat(text, num);
		}
	}

	public void SetInt(string parameter = "key,value")
	{
		char[] separator = new char[2] { ',', ';' };
		string[] array = parameter.Split(separator);
		string text = array[0];
		int num = Convert.ToInt32(array[1]);
		Debug.Log(text + " " + num);
		Animator[] array2 = animators;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetInteger(text, num);
		}
	}

	public void SetBool(string parameter = "key,value")
	{
		char[] separator = new char[2] { ',', ';' };
		string[] array = parameter.Split(separator);
		string text = array[0];
		bool value = Convert.ToBoolean(array[1]);
		Debug.Log(text + " " + value);
		Animator[] array2 = animators;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetBool(text, value);
		}
	}

	public void SetTrigger(string parameter = "key,value")
	{
		char[] separator = new char[2] { ',', ';' };
		string text = parameter.Split(separator)[0];
		Debug.Log(text);
		Animator[] array = animators;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetTrigger(text);
		}
	}
}
