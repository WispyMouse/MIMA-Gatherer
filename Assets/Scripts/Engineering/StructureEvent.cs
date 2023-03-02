using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "StructureEvent.asset",
    menuName = "Game Events/Custom/Structure Event")]
public class StructureEvent : GameEventBase<Structure>
{

}
