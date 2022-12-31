﻿using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetMoney
{
    internal static class TextField
    {
        internal static string Text { get; private set; } = PlayerSaveWrapper.MyBalance.ToString();

        internal static readonly Rect Dimensions = new Rect
                (
                Button.Rectangle.x + Button.Rectangle.width + Margin,
                Button.Rectangle.y,
                150,
                RowHeight
                );

        internal static void Draw()
        {
            Text = GUI.TextField(Dimensions, Text);
        }
    }
}