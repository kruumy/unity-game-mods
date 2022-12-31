using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetLevel
{
    internal static class TextField
    {
        internal static string Text { get; private set; } = Save.MyLvl.ToString();

        internal readonly static Rect Rectangle = new Rect
                (
                Button.Rectangle.x + Button.Rectangle.width + Margin,
                Button.Rectangle.y,
                150,
                RowHeight
                );

        internal static void Draw()
        {
            Text = GUI.TextField(Rectangle, Text);
        }
    }
}
