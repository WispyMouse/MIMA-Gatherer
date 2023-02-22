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

    private void Awake()
    {
        Instance = this;

        ActiveInventoryInstance = new Inventory();
    }

    private void Start()
    {
        ChangeResource(nameof(Crystals), 50);
    }

    public static void ChangeResource(string resourceName, int delta)
    {
        int previousCount = ActiveInventoryInstance.GetGatherableCount(resourceName).Count;
        int newCount = previousCount + delta;
        ActiveInventoryInstance.SetGatherableCount(resourceName, newCount);
        Instance.ResourceChangeEvent.Raise(new ResourceChangeData() { ResourceName = resourceName, OldAmount = previousCount, NewAmount = newCount });
    }
}
