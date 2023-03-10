using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, InventoryCounter> ResourceNamesToCounters { get; set; } = new Dictionary<string, InventoryCounter>();

    public Inventory()
    {
    }

    public InventoryCounter GetGatherableCount(string gatherableName)
    {
        InventoryCounter counter;

        if (!ResourceNamesToCounters.TryGetValue(gatherableName, out counter))
        {
            counter = new InventoryCounter(0, ConfigurationManagement.GatherableSkeletons[gatherableName.ToLower()]);
            ResourceNamesToCounters.Add(gatherableName, counter);
        }

        return counter;
    }

    public void SetGatherableCount(string gatherableName, int newCount)
    {
        InventoryCounter counter = GetGatherableCount(gatherableName);
        counter.Count = newCount;
    }
}
