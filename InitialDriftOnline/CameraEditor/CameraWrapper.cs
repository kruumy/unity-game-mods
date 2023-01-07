using System.Reflection;

namespace CameraEditor
{
    public static class CameraWrapper
    {
        public static float Distance
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSDistance;
            set => RCC_SceneManager.Instance.activePlayerCamera.TPSDistance = value;
        }

        public static float FieldOfView
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSMinimumFOV;
            set
            {
                targetFieldOfView = value + 10.0f;
                RCC_SceneManager.Instance.activePlayerCamera.TPSMinimumFOV = value;
                RCC_SceneManager.Instance.activePlayerCamera.TPSMaximumFOV = value + 20.0f;
            }
        }

        public static float Height
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSHeight;
            set => RCC_SceneManager.Instance.activePlayerCamera.TPSHeight = value;
        }

        public static float OffsetX
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSOffsetX;
            set => RCC_SceneManager.Instance.activePlayerCamera.TPSOffsetX = value;
        }

        public static float OffsetY
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSOffsetY;
            set => RCC_SceneManager.Instance.activePlayerCamera.TPSOffsetY = value;
        }

        public static float PitchAngle
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSPitchAngle;
            set => RCC_SceneManager.Instance.activePlayerCamera.TPSPitchAngle = value;
        }

        public static float YawAngle
        {
            get => RCC_SceneManager.Instance.activePlayerCamera.TPSYawAngle;
            set => RCC_SceneManager.Instance.activePlayerCamera.TPSYawAngle = value;
        }

        private static float? targetFieldOfView
        {
            get
            {
                if (RCC_SceneManager.Instance.activePlayerCamera == null)
                {
                    return null;
                }

                FieldInfo fieldInfo = typeof(RCC_Camera).GetField("targetFieldOfView", BindingFlags.NonPublic | BindingFlags.Instance);
                return fieldInfo == null ? null : (float?)(float)fieldInfo.GetValue(RCC_SceneManager.Instance.activePlayerCamera);
            }
            set
            {
                if (RCC_SceneManager.Instance.activePlayerCamera != null)
                {
                    FieldInfo fieldInfo = typeof(RCC_Camera).GetField("targetFieldOfView", BindingFlags.NonPublic | BindingFlags.Instance);
                    fieldInfo?.SetValue(RCC_SceneManager.Instance.activePlayerCamera, value);
                }
            }
        }
    }
}
