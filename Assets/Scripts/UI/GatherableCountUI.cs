using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatherableCountUI : MonoBehaviour
{
    [SerializeField]
    private string ResourceToTrack;

    [SerializeReference]
    private TMP_Text ResourceNameLabel;

    [SerializeReference]
    private TMP_Text CountLabel;

    public void Awake()
    {
        UpdateCount();
    }

    public void GatherableIntegerCountUpdated(ResourceChangeData resourceChangeData)
    {
        if (!resourceChangeData.ResourceName.Equals(ResourceToTrack, System.StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        UpdateCount();
    }

    void UpdateCount()
    {
        if (!InventoryManagement.InventoryIsLoaded)
        {
            Debug.Log($"{nameof(InventoryManagement.InventoryIsLoaded)} is false");
            return;
        }

        Inventory inventoryToUse = InventoryManagement.ActiveInventoryInstance;
        InventoryCounter trackedCounter = inventoryToUse.GetGatherableCount(ResourceToTrack);

        if (trackedCounter == null)
        {
            Debug.Log($"Resource {ResourceToTrack} is not in the inventory");
            return;
        }

        CountLabel.text = $"x{trackedCounter.Count.ToString()}";
    }

    public void SetResource(string resourceName)
    {
        ResourceToTrack = resourceName;
        ResourceNameLabel.text = resourceName;
        UpdateCount();
    }
}
