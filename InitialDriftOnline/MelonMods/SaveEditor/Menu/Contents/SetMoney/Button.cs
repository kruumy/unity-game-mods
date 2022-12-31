using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetMoney
{
    internal static class Button
    {
        internal readonly static Rect Rectangle = new Rect
                (
                BaseMenu.x + Margin,
                BaseMenu.y + TitleBarHeight + Margin + RowHeight * 0 + Margin * 0,
                100,
                RowHeight
                );
        internal static void Draw()
        {
            if (GUI.Button(Rectangle, "Set Money"))
            {
                Save.MyBalance = int.Parse(TextField.Text);
            }
        }
    }
}
