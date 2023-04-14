using System.Collections.Generic;
using UnityEngine;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// A base <see cref="Control"/> that represents a tool bar.
    /// </summary>
    public abstract class Toolbar : ValueControl<int>
    {
        /// <inheritdoc/>
        public List<GUIContent> Contents { get; set; } = new List<GUIContent>();
    }
}
