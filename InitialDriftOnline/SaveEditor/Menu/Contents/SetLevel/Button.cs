using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetLevel
{
    internal static class Button
    {
        internal static readonly Rect Dimensions = new Rect
                (
                Margin,
                TitleBarHeight + Margin + RowHeight * 1 + Margin * 1,
                100,
                RowHeight
                );

        internal static void Draw()
        {
            if (GUI.Button(Dimensions, "Set Level"))
            {
                PlayerSaveWrapper.MyLvl = int.Parse(TextField.Text);
            }
        }
    }
}