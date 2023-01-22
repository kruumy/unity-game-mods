using MelonLoader;
using UnityEngine;

namespace WallHack
{
    public class Main : MelonMod
    {
        public override void OnGUI()
        {
            foreach (EnemyIdentifier enemy in EnemyTracker.Instance.enemies)
            {
                if (!enemy.dead)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.gameObject.transform.position);
                    screenPos.y = Screen.height - screenPos.y;
                    if (screenPos.z > 0)
                    {
                        string name = enemy.name ?? "Unnamed";
                        int width = name.Length * 8;
                        int height = 25;
                        GUI.Box(new Rect(screenPos.x - width / 2, screenPos.y - height / 2, width, height), name);
                    }
                }
            }
        }
    }
}
