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

    public ConfiguredStatistic<int> InitialCrystalCount { get; set; } = new ConfiguredStatistic<int>(50, $"{nameof(InventoryManagement)}.{nameof(InitialCrystalCount)}");

    private void Awake()
    {
        Instance = this;

        ActiveInventoryInstance = new Inventory();
        InitialCrystalCount.LoadFromConfiguration(ConfigurationManagement.ActiveConfiguration);
    }

    private void Start()
    {
        ChangeResource(nameof(Crystals), InitialCrystalCount.Value);
    }

    public static void ChangeResource(string resourceName, int delta)
    {
        int previousCount = ActiveInventoryInstance.GetGatherableCount(resourceName).Count;
        int newCount = previousCount + delta;
        ActiveInventoryInstance.SetGatherableCount(resourceName, newCount);
        Instance.ResourceChangeEvent.Raise(new ResourceChangeData() { ResourceName = resourceName, OldAmount = previousCount, NewAmount = newCount });
    }
}
