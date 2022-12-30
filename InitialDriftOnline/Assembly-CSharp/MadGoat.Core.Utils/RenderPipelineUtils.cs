using UnityEngine.Rendering;

namespace MadGoat.Core.Utils;

public static class RenderPipelineUtils
{
	public enum PipelineType
	{
		Unsupported,
		BuiltInPipeline,
		UniversalPipeline,
		HDPipeline
	}

	public static PipelineType DetectPipeline()
	{
		if (GraphicsSettings.renderPipelineAsset != null)
		{
			string text = GraphicsSettings.renderPipelineAsset.GetType().ToString();
			if (text.Contains("HDRenderPipelineAsset"))
			{
				return PipelineType.HDPipeline;
			}
			if (text.Contains("UniversalRenderPipelineAsset") || text.Contains("LightweightRenderPipelineAsset"))
			{
				return PipelineType.UniversalPipeline;
			}
			return PipelineType.Unsupported;
		}
		return PipelineType.BuiltInPipeline;
	}
}
