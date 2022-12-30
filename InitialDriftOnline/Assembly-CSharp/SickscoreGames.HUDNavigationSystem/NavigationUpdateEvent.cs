using System;
using UnityEngine;
using UnityEngine.Events;

namespace SickscoreGames.HUDNavigationSystem;

[Serializable]
public class NavigationUpdateEvent : UnityEvent<Vector3, Vector3, float>
{
}
