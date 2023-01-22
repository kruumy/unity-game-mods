﻿using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace HealthBars
{
    public class Main : MelonMod
    {
        public readonly static Dictionary<EnemyType, float> MaxHealth = new Dictionary<EnemyType, float>();
        public readonly static Texture2D RedTexture = MakeTex(1, 1, Color.red);
        public readonly static Texture2D YellowTexture = MakeTex(1, 1, Color.yellow);
        public readonly static Texture2D GreenTexture = MakeTex(1, 1, Color.green);
        public readonly static Texture2D BlackTexture = MakeTex(1, 1, Color.black);
        public readonly static GUIStyle RedStyle = new GUIStyle()
        {
            normal =
            {
                background = RedTexture,
                textColor = Color.black
            },
            richText = false,
            fontSize = 14,
            fontStyle = FontStyle.Italic,
            wordWrap = false,
            alignment = TextAnchor.MiddleCenter
        };
        public readonly static GUIStyle YellowStyle = new GUIStyle(RedStyle)
        {
            normal =
            {
                background = YellowTexture,
            },
        };
        public readonly static GUIStyle GreenStyle = new GUIStyle(RedStyle)
        {
            normal =
            {
                background = GreenTexture,
            },
        };
        public readonly static GUIStyle BlackStyle = new GUIStyle(RedStyle)
        {
            normal =
            {
                background = BlackTexture,
            },
        };

        public override void OnGUI()
        {
            foreach (EnemyIdentifier enemy in EnemyTracker.Instance.enemies)
            {
                if (!enemy.dead)
                {
                    Vector3 enemyPosOnScreen = Camera.main.WorldToScreenPoint(enemy.gameObject.transform.position);
                    enemyPosOnScreen.y = Screen.height - enemyPosOnScreen.y;
                    if (enemyPosOnScreen.z > 0)
                    {
                        float globalScale = 20.0f;
                        int width = (int)Mathf.Clamp(75 * globalScale / enemyPosOnScreen.z, 0, 100);
                        int height = (int)Mathf.Clamp(15 * globalScale / enemyPosOnScreen.z, 0, 20);
                        int fontSize = (int)Mathf.Clamp(14 * globalScale / enemyPosOnScreen.z, 0, 18);
                        RedStyle.fontSize = fontSize;
                        YellowStyle.fontSize = fontSize;
                        GreenStyle.fontSize = fontSize;
                        int healthWidth = (int)Map(enemy.health, 0, MaxHealth[enemy.enemyType], 0, width);
                        int healthPercentage = ((int)(healthWidth * (100.0f / width)));
                        enemyPosOnScreen.x -= width / 2;
                        enemyPosOnScreen.y -= height / 2;
                        GUI.Box(new Rect(enemyPosOnScreen.x, enemyPosOnScreen.y, width, height), GUIContent.none, BlackStyle);
                        GUIStyle healthStyle = healthPercentage > 80 ? GreenStyle : healthPercentage > 40 ? YellowStyle : RedStyle;
                        GUI.Box(new Rect(enemyPosOnScreen.x, enemyPosOnScreen.y, healthWidth, height), healthPercentage.ToString() + "%", healthStyle);
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
