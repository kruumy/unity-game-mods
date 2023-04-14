using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class HorizontalCenter : NestedControl
    {
        public override void Draw()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            base.Draw();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
