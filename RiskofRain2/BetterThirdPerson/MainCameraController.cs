using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterThirdPerson
{
    public static class MainCameraController
    {
        public static void Init()
        {
            SceneManager.activeSceneChanged += async (arg0, arg1) =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Camera.main == null)
                    {
                        await Task.Delay(500);
                    }
                    else
                    {
                        LocalPosition.Enforce();
                        FieldOfView.Enforce();
                        break;
                    }
                }
            };
        }

        public static class LocalPosition
        {
            public readonly static Vector3 Default = new Vector3(0, 0, 0);
            private static float _X = Default.x;

            private static float _Y = Default.y;

            private static float _Z = Default.z;

            public static float X
            {
                get => Camera.main.transform.localPosition.x;
                set
                {
                    _X = value;
                    Set(_X, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
                }
            }

            public static float Y
            {
                get => Camera.main.transform.localPosition.y;
                set
                {
                    _Y = value;
                    Set(Camera.main.transform.localPosition.x, _Y, Camera.main.transform.localPosition.z);
                }
            }

            public static float Z
            {
                get => Camera.main.transform.localPosition.z;
                set
                {
                    _Z = value;
                    Set(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, _Z);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector3 Get()
            {
                return new Vector3(X, Y, Z);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Set(float x, float y, float z)
            {
                Set(new Vector3(x, y, z));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Enforce()
            {
                Set(_X, _Y, _Z);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Set(Vector3 postion)
            {
                _X = postion.x;
                _Y = postion.y;
                _Z = postion.z;
                if (Camera.main != null)
                {
                    Camera.main.transform.localPosition = postion;
                }
            }
        }
        public static class FieldOfView
        {
            public readonly static float Default = 60f;
            private static float _Value = Default;
            public static float Value
            {
                get
                {
                    foreach (RoR2.CameraRigController cam in UnityEngine.Object.FindObjectsOfType<RoR2.CameraRigController>())
                    {
                        if (cam.isActiveAndEnabled)
                        {
                            return cam.baseFov;
                        }
                    }
                    return _Value;
                }
                set
                {
                    _Value = value;
                    foreach (RoR2.CameraRigController cam in UnityEngine.Object.FindObjectsOfType<RoR2.CameraRigController>())
                    {
                        if (cam.isActiveAndEnabled)
                        {
                            cam.baseFov = value;
                        }
                    }
                }
            }
            public static void Enforce()
            {
                Value = _Value;
            }
        }

    }
}
