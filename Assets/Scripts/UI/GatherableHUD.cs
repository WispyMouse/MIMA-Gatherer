using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableHUD : MonoBehaviour
{
    [SerializeReference]
    private GatherableCountUI GatherableCountUIPrefab;

    [SerializeReference]
    private Transform GatherableCountUIParent;

    Dictionary<string, GatherableCountUI> ResourceNameToGatherableMap { get; set; } = new Dictionary<string, GatherableCountUI>();

    public void ResourceCountUpdated(ResourceChangeData resourceChangeData)
    {
        if (ResourceNameToGatherableMap.ContainsKey(resourceChangeData.FriendlyName))
        {
            // Nothing to do here
            return;
        }

        GatherableCountUI newCountUI = Instantiate(GatherableCountUIPrefab, GatherableCountUIParent);
        ResourceNameToGatherableMap.Add(resourceChangeData.FriendlyName, newCountUI);
        newCountUI.SetResource(resourceChangeData.FriendlyName);
    }
}
