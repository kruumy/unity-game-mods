using UnityEngine.PostProcessing;

namespace GraphicsEditor
{
    public static class CameraExtensions
    {
        public static void EnablePostProcessing(this UnityEngine.Camera camera)
        {
            PostProcessingBehaviour ppb = null;
            if (camera.gameObject.TryGetComponent<PostProcessingBehaviour>(out PostProcessingBehaviour res))
            {
                ppb = res;
            }
            else
            {
                ppb = camera.gameObject.AddComponent<PostProcessingBehaviour>();
            }

            if (ppb.profile == null)
            {
                ppb.profile = new PostProcessingProfile();
            }
        }

        public static PostProcessingBehaviour get_PostProcessingBehaviour(this UnityEngine.Camera camera)
        {
            return camera.gameObject.GetComponent<PostProcessingBehaviour>();
        }
    }
}
