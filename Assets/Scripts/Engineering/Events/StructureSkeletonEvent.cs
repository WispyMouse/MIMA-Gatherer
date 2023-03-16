using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "StructureSkeletonEvent.asset",
    menuName = "Game Events/Custom/Structure Skeleton Event")]
public class StructureSkeletonEvent : GameEventBase<StructureSkeleton>
{

}
