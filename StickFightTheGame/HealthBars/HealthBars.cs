using MelonLoader;
using UnityEngine;

namespace HealthBars
{
    public static class HealthBars
    {
        private static float Map( float value, float fromMin, float fromMax, float toMin, float toMax )
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        private static Texture2D MakeTex( int width, int height, Color col )
        {
            Color[] array = new Color[ width * height ];
            for ( int i = 0; i < array.Length; i++ )
            {
                array[ i ] = col;
            }
            Texture2D texture2D = new Texture2D(width, height);
            texture2D.SetPixels(array);
            texture2D.Apply();
            return texture2D;
        }

        public static readonly Texture2D RedTexture = MakeTex(1, 1, Color.red);

        public static readonly Texture2D YellowTexture = MakeTex(1, 1, Color.yellow);

        public static readonly Texture2D GreenTexture = MakeTex(1, 1, Color.green);

        public static readonly Texture2D BlackTexture = MakeTex(1, 1, Color.black);

        public static readonly GUIStyle RedStyle = new GUIStyle()
        {
            normal =
            {
                background = RedTexture
            }
        };


        public static readonly GUIStyle YellowStyle = new GUIStyle()
        {
            normal =
            {
                background = YellowTexture
            }
        };

        public static readonly GUIStyle GreenStyle = new GUIStyle()
        {
            normal =
            {
                background = GreenTexture
            }
        };

        public static readonly GUIStyle BlackStyle = new GUIStyle()
        {
            normal =
            {
                background = BlackTexture
            }
        };

        public static void Show()
        {
            MelonEvents.OnGUI.Subscribe(Draw);
            MelonLogger.Msg("HeathBars Shown");
        }
        public static void Hide()
        {
            MelonEvents.OnGUI.Unsubscribe(Draw);
            MelonLogger.Msg("HeathBars Hidden");
        }
        private static void Draw()
        {
            foreach ( Controller player in GameManager.Instance.playersAlive )
            {
                Vector3 vector = player.gameObject.transform.GetChild(0).GetChild(8).position;
                vector.y += 1;
                vector.z += 0.70f;
                Vector3 posOnScreen = Camera.main.WorldToScreenPoint(vector);
                posOnScreen.y = Screen.height - posOnScreen.y;
                float width = 75;
                float height = 5;
                HealthHandler health = player.GetHealthHandler();
                float healthWidth = Map(health.health, 0, health.maxHealth, 0, width);
                float healthPercentage = healthWidth * (100.0f / width);
                GUI.Box(new Rect(posOnScreen.x, posOnScreen.y, width, height), GUIContent.none, BlackStyle);
                GUIStyle healthStyle = healthPercentage > 80 ? GreenStyle : healthPercentage > 40 ? YellowStyle : RedStyle;
                GUI.Box(new Rect(posOnScreen.x, posOnScreen.y, healthWidth, height), GUIContent.none, healthStyle);
            }
        }
    }
}
