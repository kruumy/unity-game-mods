using MelonLoader;
using UnityEngine;

namespace WallHackTest
{
    public class Main : MelonMod
    {
        public bool WallHackEnabled = true;
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                WallHackEnabled = !WallHackEnabled;
            }
        }
        public override void OnGUI()
        {
            if (WallHackEnabled && Camera.main != null)
            {
                foreach (RCC_CarControllerV3 car in UnityEngine.Object.FindObjectsOfType<RCC_CarControllerV3>())
                {
                    if (car != RCC_SceneManager.Instance.activePlayerVehicle)
                    {
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(car.gameObject.transform.position);
                        screenPos.y = Screen.height - screenPos.y;
                        if (screenPos.z > 0)
                        {
                            int boxWidth = car.name.Length * 8;
                            int boxHeight = 25;
                            GUI.Box(new Rect(screenPos.x - (boxWidth / 2), screenPos.y - (boxHeight / 2), boxWidth, boxHeight), car.name);
                        }
                    }
                }
            }
        }
    }
}
