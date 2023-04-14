using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class VerticalCenter : NestedControl
    {
        public override void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            base.Draw();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}
