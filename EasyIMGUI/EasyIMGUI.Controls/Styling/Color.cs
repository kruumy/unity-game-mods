using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace EasyIMGUI.Controls.Styling
{
    public class Color : NestedControl
    {
        public UnityEngine.Color BackgroundColor { get; set; } = GUI.backgroundColor;
        public UnityEngine.Color MainColor { get; set; } = GUI.color;
        public UnityEngine.Color ContentColor { get; set; } = GUI.contentColor;
        public override void Draw()
        {
            UnityEngine.Color oldbgc = GUI.backgroundColor;
            UnityEngine.Color oldc = GUI.color;
            UnityEngine.Color oldcc = GUI.contentColor;
            GUI.backgroundColor = BackgroundColor;
            GUI.color = MainColor;
            GUI.contentColor = ContentColor;
            base.Draw();
            GUI.backgroundColor = oldbgc;
            GUI.color = oldc;
            GUI.contentColor = oldcc;
        }
    }
}
