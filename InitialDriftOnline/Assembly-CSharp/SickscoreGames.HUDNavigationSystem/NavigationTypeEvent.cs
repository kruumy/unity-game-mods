using System;
using UnityEngine.Events;

namespace SickscoreGames.HUDNavigationSystem;

[Serializable]
public class NavigationTypeEvent : UnityEvent<HUDNavigationElement, NavigationElementType>
{
}
