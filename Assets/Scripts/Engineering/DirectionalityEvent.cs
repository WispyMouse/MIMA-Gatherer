using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "DirectionalityEvent.asset",
    menuName = "Game Events/Custom/Directionality Event")]
public class DirectionalityEvent : GameEventBase<Directionality>
{

}
