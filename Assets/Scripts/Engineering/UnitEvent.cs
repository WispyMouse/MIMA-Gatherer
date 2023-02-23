using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "ResourceChangeEvent.asset",
    menuName = "Game Events/Custom/Unit Event")]
public class UnitEvent : GameEventBase<Unit>
{

}
