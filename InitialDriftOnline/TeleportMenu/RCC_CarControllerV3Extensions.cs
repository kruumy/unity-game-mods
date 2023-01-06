﻿using TMPro;

namespace TeleportMenu
{
    public static class RCC_CarControllerV3Extensions
    {
        /// Does not work on player car.
        public static string get_PlayerName(this RCC_CarControllerV3 car)
        {
            UnityEngine.Transform TextTMP = car.transform.Find("Text (TMP)");
            TextMeshPro textObj = TextTMP.gameObject.GetComponent<TextMeshPro>();
            int startOfName = textObj.text.LastIndexOf('>') + 2;
            return textObj.text.Substring(startOfName);
        }
    }
}
