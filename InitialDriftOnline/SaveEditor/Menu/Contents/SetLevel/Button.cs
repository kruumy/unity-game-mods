using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetLevel
{
    internal static class Button
    {
        internal static readonly Rect Rectangle = new Rect
                (
                BaseMenu.x + Margin,
                BaseMenu.y + TitleBarHeight + Margin + RowHeight * 1 + Margin * 1,
                100,
                RowHeight
                );

        internal static void Draw()
        {
            if (GUI.Button(Rectangle, "Set Level"))
            {
                Save.MyLvl = int.Parse(TextField.Text);
            }
        }
    }
}