using MelonLoader;
using UnityEngine;

namespace AmmoDisplay
{
    public static class AmmoDisplay
    {
        public static T GetPrivateField<T>( this object obj, string fieldName )
        {
            System.Reflection.FieldInfo fightingField = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)fightingField.GetValue(obj);
        }
        public static void Show()
        {
            MelonEvents.OnGUI.Subscribe(Draw);
            MelonLogger.Msg("AmmoDisplay Shown");
        }
        public static void Hide()
        {
            MelonEvents.OnGUI.Unsubscribe(Draw);
            MelonLogger.Msg("AmmoDisplay Hidden");
        }

        private static readonly GUIStyle AmmoStyle = new GUIStyle()
        {
            normal =
            {
                textColor = Color.white
            },
            fontSize = 24,
            fontStyle = FontStyle.Bold,
        };

        private static void Draw()
        {
            System.Collections.Generic.IEnumerable<Controller> players = GameManager.Instance.playersAlive;
            foreach ( Controller player in players )
            {
                if ( player.HasControl ) // is local player
                {
                    Fighting playerFighting = player.GetComponent<Fighting>();
                    if ( playerFighting.weapon != null )
                    {
                        Vector3 torso = player.gameObject.transform.GetChild(0).GetChild(8).position;
                        torso.y -= 1.30f;
                        torso.z -= 0.5f;
                        Vector3 torsoOnScreen = Camera.main.WorldToScreenPoint(torso);
                        torsoOnScreen.y = Screen.height - torsoOnScreen.y;

                        string text = $"{playerFighting.GetPrivateField<int>("bulletsLeft")}";
                        GUI.Box(new Rect(torsoOnScreen.x, torsoOnScreen.y, 100, 25), text, AmmoStyle);
                    }
                }
            }
        }
    }
}
