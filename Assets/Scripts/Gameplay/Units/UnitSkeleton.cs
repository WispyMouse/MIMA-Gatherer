using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSkeleton
{
    public static UnitSkeleton LoadFromFile(string path)
    {
        UnitSkeleton skeleton = ConfigurationManagement.LoadFromConfiguration<UnitSkeleton>(path);
        return skeleton;
    }

    public string UnitName;
    public string FriendlyName;

    public List<ResourceCost> Costs;

    public float ProductionTimeSeconds;

    public float MovementPerSecond;
    public float TimeToReturnResource;

}
