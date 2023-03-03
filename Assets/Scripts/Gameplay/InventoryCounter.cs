using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCounter
{
    public int Count { get; set; } = 0;
    public GatherableSkeleton GatherableSkeletonData { get; private set; }

    public InventoryCounter(int startingCount, GatherableSkeleton skeleton)
    {
        Count = startingCount;
        this.GatherableSkeletonData = skeleton;
    }
}
