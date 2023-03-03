using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GatherableSkeleton
{
    public static GatherableSkeleton LoadFromFile(string path)
    {
        GatherableSkeleton skeleton = ConfigurationManagement.LoadFromConfiguration<GatherableSkeleton>(path);
        return skeleton;
    }

    public string ResourceName;
    public string FriendlyName;

    public string ModelColor;
    public float ModelScale;

    public float TimeToGatherResource;
    public int AmountOfResourcesGathered;
}
