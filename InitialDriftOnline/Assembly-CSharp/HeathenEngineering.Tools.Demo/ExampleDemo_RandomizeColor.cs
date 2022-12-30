using HeathenEngineering.Scriptable;
using UnityEngine;

namespace HeathenEngineering.Tools.Demo;

public class ExampleDemo_RandomizeColor : MonoBehaviour
{
	public void RandomizeColor(ColorVariable target)
	{
		target.SetValue(Random.ColorHSV());
	}
}
