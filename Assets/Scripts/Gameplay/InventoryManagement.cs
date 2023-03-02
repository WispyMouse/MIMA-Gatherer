using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{
    private static InventoryManagement Instance { get; set; }

    public static bool InventoryIsLoaded
    {
        get
        {
            return ActiveInventoryInstance != null;
        }
    }
    public static Inventory ActiveInventoryInstance { get; set; }

    [SerializeReference]
    private ResourceChangeEvent ResourceChangeEvent;

    public ConfiguredStatistic<int> AdditionalCrystalCount { get; set; } = new ConfiguredStatistic<int>(0, $"{nameof(InventoryManagement)}.{nameof(AdditionalCrystalCount)}");

    private void Awake()
    {
        Instance = this;

        ActiveInventoryInstance = new Inventory();
    }

    private void Start()
    {
        ChangeResource(nameof(Crystals), AdditionalCrystalCount.Value);
    }

    public static bool CanAfford(List<ResourceCost> costs)
    {
        if (costs == null)
        {
            return true;
        }

        foreach (ResourceCost cost in costs)
        {
            if (ActiveInventoryInstance.GetGatherableCount(cost.Resource).Count < cost.Cost)
            {
                return false;
            }
        }

        return true;
    }

    public static void ChangeResource(List<ResourceCost> costs)
    {
        if (costs == null)
        {
            return;
        }

        foreach (ResourceCost cost in costs)
        {
            ChangeResource(cost.Resource, -cost.Cost);
        }
    }

    public static void ChangeResource(string resourceName, int delta)
    {
        if (delta == 0)
        {
            return;
        }

        int previousCount = ActiveInventoryInstance.GetGatherableCount(resourceName).Count;
        int newCount = previousCount + delta;
        ActiveInventoryInstance.SetGatherableCount(resourceName, newCount);
        Instance.ResourceChangeEvent.Raise(new ResourceChangeData() { ResourceName = resourceName, OldAmount = previousCount, NewAmount = newCount });
    }

    public static void Grant(List<StartingInventoryElement> startingInventory)
    {
        foreach (StartingInventoryElement element in startingInventory)
        {
            ChangeResource(element.ResourceName, element.Amount);
        }
    }
}
