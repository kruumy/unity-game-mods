﻿using UnityEngine;
using static SaveEditor.Menu.Menu;

namespace SaveEditor.Menu.Contents.SetMoney
{
    internal static class Button
    {
        internal static readonly Rect Rectangle = new Rect
                (
                Margin,
                TitleBarHeight + Margin + RowHeight * 0 + Margin * 0,
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