using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace MadGoat.SSAA;

[Serializable]
public class SSAAExtensionPostProcessingStack2 : SSAAExtensionBase
{
	public static string description = "Enables support for Unity's Postprocessing Stack v2.\n   • updateFromOriginal - if enabled, the SSAA camera PPLayer settings will always be synced with this cameras PPLayer (if available)\n   • lwrpLegacySupport - enables features which are missing by default on SSAA with legacy LWRP";

	public static string requirement = "- Requires Post Processing Stack v2 package\n- Requires Built-In Pipeline or LWRP 2019.2 or lower";

	public bool updateFromOriginal = true;

	public bool lwrpLegacySupport = true;

	private PostProcessLayer ppLayerCurrent;

	private PostProcessLayer ppLayerRenderer;

	private bool hasInitialized;

	public override bool IsSupported()
	{
		return true;
	}

	public override void OnInitialize(MadGoatSSAA ssaaInstance)
	{
		base.OnInitialize(ssaaInstance);
		if (enabled)
		{
			ppLayerCurrent = ssaaInstance.CurrentCamera.GetComponent<PostProcessLayer>();
			ppLayerRenderer = ssaaInstance.RenderCamera.GetComponent<PostProcessLayer>();
			if (ppLayerRenderer == null)
			{
				ppLayerRenderer = ssaaInstance.RenderCamera.gameObject.AddComponent<PostProcessLayer>();
			}
			ppLayerRenderer.enabled = true;
			ssaaInstance.StartCoroutine(MotionVectorsFix());
		}
	}

	public override void OnUpdate(MadGoatSSAA ssaaInstance)
	{
		if (enabled)
		{
			if ((bool)ppLayerCurrent && ppLayerCurrent.enabled && hasInitialized)
			{
				ppLayerCurrent.enabled = false;
			}
			else if ((bool)ppLayerCurrent && !hasInitialized)
			{
				ppLayerCurrent.enabled = true;
			}
			if (updateFromOriginal && (bool)ppLayerCurrent)
			{
				ppLayerRenderer.breakBeforeColorGrading = ppLayerCurrent.breakBeforeColorGrading;
				ppLayerRenderer.finalBlitToCameraTarget = ppLayerCurrent.finalBlitToCameraTarget;
				ppLayerRenderer.fog = ppLayerCurrent.fog;
				ppLayerRenderer.stopNaNPropagation = ppLayerCurrent.stopNaNPropagation;
				ppLayerRenderer.volumeTrigger = ppLayerCurrent.transform;
				ppLayerRenderer.volumeLayer = ppLayerCurrent.volumeLayer;
			}
		}
	}

	public override void OnDeinitialize(MadGoatSSAA ssaaInstance)
	{
		base.OnDeinitialize(ssaaInstance);
		if (enabled)
		{
			if ((bool)ppLayerCurrent && !ppLayerCurrent.enabled)
			{
				ppLayerCurrent.enabled = true;
			}
			if ((bool)ppLayerRenderer && ppLayerRenderer.enabled)
			{
				ppLayerRenderer.enabled = false;
			}
		}
	}

	private IEnumerator MotionVectorsFix()
	{
		hasInitialized = false;
		yield return new WaitForSecondsRealtime(0.5f);
		hasInitialized = true;
	}
}
