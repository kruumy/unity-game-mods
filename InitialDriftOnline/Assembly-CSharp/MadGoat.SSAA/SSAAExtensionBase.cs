using System;
using UnityEngine;

namespace MadGoat.SSAA;

[Serializable]
public class SSAAExtensionBase
{
	[SerializeField]
	[HideInInspector]
	public bool inspectorFoldout;

	[SerializeField]
	[HideInInspector]
	public bool enabled;

	public virtual bool IsSupported()
	{
		return true;
	}

	public virtual void OnInitialize(MadGoatSSAA ssaaInstance)
	{
	}

	public virtual void OnUpdate(MadGoatSSAA ssaaInstance)
	{
	}

	public virtual void OnDeinitialize(MadGoatSSAA ssaaInstance)
	{
	}
}
