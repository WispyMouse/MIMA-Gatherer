using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureSkeleton
{
    public static StructureSkeleton LoadFromFile(string path)
    {
        StructureSkeleton skeleton = ConfigurationManagement.LoadFromConfiguration<StructureSkeleton>(path);
        return skeleton;
    }

    public string StructureName;
    public string FriendlyName;

    public List<ResourceCost> Costs;

    public float ProductionTimeSeconds;

    public string ModelColor;
    public float ModelScale;

    public List<string> AcceptedResources;
}
