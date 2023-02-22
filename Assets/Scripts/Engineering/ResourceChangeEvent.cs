using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "ResourceChangeEvent.asset",
    menuName = "Game Events/Custom/Resource Change")]
public class ResourceChangeEvent : GameEventBase<ResourceChangeData>
{

}
