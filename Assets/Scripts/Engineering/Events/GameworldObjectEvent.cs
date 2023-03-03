using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "GameworldObjectEvent.asset",
    menuName = "Game Events/Custom/Gameworld Object")]
public class GameworldObjectEvent : GameEventBase<GameworldObject>
{

}
