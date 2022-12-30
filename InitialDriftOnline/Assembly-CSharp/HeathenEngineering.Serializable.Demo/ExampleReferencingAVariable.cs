using HeathenEngineering.Scriptable;
using UnityEngine;

namespace HeathenEngineering.Serializable.Demo;

public class ExampleReferencingAVariable : MonoBehaviour
{
	public BoolReference useAsAConstant;

	public BoolReference useAsAReference;

	private void Start()
	{
		Debug.Log("The constant value is " + useAsAConstant.Value);
		Debug.Log("The constant value is " + useAsAReference.Value);
	}
}
