using EasyIMGUI.Controls.Base;
using System.Collections.Generic;
using UnityEngine;

namespace EasyIMGUI.Controls.Extra
{
    public class SelectorItems : List<SelectorItem>
    {
        public GUIContent[] Contents
        {
            get
            {
                GUIContent[] result = new GUIContent[Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = base[i].Content;
                }
                return result;
            }
        }

        public Control[] Controls
        {
            get
            {
                Control[] result = new Control[Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = base[i].Control;
                }
                return result;
            }
        }

        public static implicit operator Control[](SelectorItems si)
        {
            return si.Controls;
        }

        public static implicit operator GUIContent[](SelectorItems si)
        {
            return si.Contents;
        }
    }
}
