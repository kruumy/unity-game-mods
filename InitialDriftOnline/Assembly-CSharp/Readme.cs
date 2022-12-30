using System;
using UnityEngine;

public class Readme : ScriptableObject
{
	[Serializable]
	public class Section
	{
		public string heading;

		public string text;

		public string linkText;

		public string url;
	}

	public Texture2D icon;

	public float iconMaxWidth = 128f;

	public string title;

	public string titlesub;

	public Section[] sections;

	public bool loadedLayout;
}
