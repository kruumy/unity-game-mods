using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetBoost
{
    internal static class Button
    {
        internal static readonly Rect Dimensions = new Rect
                (
                Menu.Dimensions.x + Margin,
                Menu.Dimensions.y + TitleBarHeight + Margin + RowHeight * 2 + Margin * 2,
                100,
                RowHeight
                );

        internal static void Draw()
        {
            if (GUI.Button(Dimensions, "Set Boost"))
            {
                Save.BoostQuantity = int.Parse(TextField.Text);
            }
        }
    }
}