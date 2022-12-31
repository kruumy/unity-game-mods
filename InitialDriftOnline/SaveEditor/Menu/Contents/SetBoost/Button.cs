using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetBoost
{
    internal static class Button
    {
        internal readonly static Rect Rectangle = new Rect
                (
                BaseMenu.x + Margin,
                BaseMenu.y + TitleBarHeight + Margin + RowHeight * 2 + Margin * 2,
                100,
                RowHeight
                );
        internal static void Draw()
        {
            if (GUI.Button(Rectangle, "Set Boost"))
            {
                Save.BoostQuantity = int.Parse(TextField.Text);
            }
        }
    }
}
