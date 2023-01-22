using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace HealthBars
{
    public class Main : MelonMod
    {
        public static Dictionary<EnemyType, float> MaxHealth = new Dictionary<EnemyType, float>();
        public static Texture2D RedTexture = MakeTex(2, 2, Color.red);
        public static Texture2D YellowTexture = MakeTex(2, 2, Color.yellow);
        public static Texture2D GreenTexture = MakeTex(2, 2, Color.green);
        public static GUIStyle RedStyle = new GUIStyle()
        {
            normal =
            {
                background = RedTexture,
                textColor = Color.black
            },
            fontSize = 14,
            fontStyle = FontStyle.Italic,
            wordWrap = false,
            alignment = TextAnchor.MiddleCenter
        };
        public static GUIStyle YellowStyle = new GUIStyle(RedStyle)
        {
            normal =
            {
                background = YellowTexture,
            },
        };
        public static GUIStyle GreenStyle = new GUIStyle(RedStyle)
        {
            normal =
            {
                background = GreenTexture,
            },
        };

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
                        int width = 75;
                        int healthWidth = (int)Map(enemy.health, 0, MaxHealth[enemy.enemyType], 0, width);
                        int height = 15;
                        int healthPercentage = ((int)(healthWidth * (100.0f / width)));
                        GUI.Box(new Rect(screenPos.x - width / 2, screenPos.y - height / 2, width, height), string.Empty);
                        GUIStyle healthStyle;
                        healthStyle = healthPercentage > 80 ? GreenStyle : healthPercentage > 40 ? YellowStyle : RedStyle;
                        GUI.Box(new Rect(screenPos.x - width / 2, screenPos.y - height / 2, healthWidth, height), healthPercentage.ToString() + "%", healthStyle);
                    }
                }
            }
        }
        private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        [HarmonyPatch(typeof(EnemyTracker), nameof(EnemyTracker.AddEnemy))]
        private class AddEnemyPatch
        {
            private static void Prefix(EnemyIdentifier eid)
            {
                MaxHealth[eid.enemyType] = eid.health;
            }
        }
    }
}
