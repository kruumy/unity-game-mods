using UnityEngine;

namespace Funly.SkyStudio;

public class LightningSpawnArea : MonoBehaviour
{
	[Tooltip("Dimensions of the lightning area where lightning bolts will be spawned inside randomly.")]
	public Vector3 lightningArea = new Vector3(40f, 20f, 20f);

	public void OnDrawGizmosSelected()
	{
		_ = base.transform.localScale;
		Gizmos.color = Color.yellow;
		_ = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, lightningArea);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	private void OnEnable()
	{
		LightningRenderer.AddSpawnArea(this);
	}

	private void OnDisable()
	{
		LightningRenderer.RemoveSpawnArea(this);
	}
}
