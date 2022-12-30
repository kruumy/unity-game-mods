﻿using System.Reflection;

namespace FOVModifier
{
    public static class Camera
    {
        public static float? FieldOfView
        {
            get
            {
                if (TPSMinimumFOV != null)
                    return TPSMinimumFOV;
                else if (targetFieldOfView != null)
                    return targetFieldOfView - 10.0f;
                else if (TPSMaximumFOV != null)
                    return TPSMaximumFOV - 20.0f;
                else
                    return null;
            }
            set
            {
                TPSMinimumFOV = value;
                targetFieldOfView = value + 10.0f;
                TPSMaximumFOV = value + 20.0f;
            }
        }
        private static float? TPSMinimumFOV
        {
            get
            {
                try
                {
                    return RCC_SceneManager.Instance.activePlayerCamera.TPSMinimumFOV;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    RCC_SceneManager.Instance.activePlayerCamera.TPSMinimumFOV = (float)value;
                }
            }
        }
        private static float? TPSMaximumFOV
        {
            get
            {
                try
                {
                    return RCC_SceneManager.Instance.activePlayerCamera.TPSMaximumFOV;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    RCC_SceneManager.Instance.activePlayerCamera.TPSMaximumFOV = (float)value;
                }
            }
        }
        private static float? targetFieldOfView
        {
            get
            {
                if (RCC_SceneManager.Instance.activePlayerCamera == null)
                    return null;
                FieldInfo fieldInfo = typeof(RCC_Camera).GetField("targetFieldOfView", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo == null)
                    return null;
                return (float)fieldInfo.GetValue(RCC_SceneManager.Instance.activePlayerCamera);
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