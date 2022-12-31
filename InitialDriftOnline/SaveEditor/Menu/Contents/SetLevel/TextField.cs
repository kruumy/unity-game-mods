using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetLevel
{
    internal static class TextField
    {
        internal static string Text { get; private set; } = Save.MyLvl.ToString();

        internal static readonly Rect Dimensions = new Rect
                (
                Button.Dimensions.x + Button.Dimensions.width + Margin,
                Button.Dimensions.y,
                150,
                RowHeight
                );

        internal static void Draw()
        {
            Text = GUI.TextField(Dimensions, Text);
        }
    }
}