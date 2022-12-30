using UnityEngine;

public class RCC_TrailerAttachPoint : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		RCC_TrailerAttachPoint component = col.gameObject.GetComponent<RCC_TrailerAttachPoint>();
		if ((bool)component)
		{
			RCC_CarControllerV3 componentInParent = component.gameObject.GetComponentInParent<RCC_CarControllerV3>();
			if ((bool)componentInParent)
			{
				base.transform.root.SendMessage("AttachTrailer", componentInParent, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
