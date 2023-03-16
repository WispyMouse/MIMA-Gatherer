using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceBuildingPlanButton : UIBehaviour
{
    [SerializeReference]
    private TMP_Text Text;

    [SerializeReference]
    private ResourceLabel ResourceCostPrefab;

    [SerializeReference]
    private Transform ResourceCostParent;

    private StructureSkeleton Skeleton { get; set; }

    [SerializeReference]
    private StructureSkeletonEvent PlaceStructureEvent;

    public void SetBuildTarget(StructureSkeleton skeleton)
    {
        Skeleton = skeleton;
        Text.text = skeleton.StructureName;

        foreach (ResourceCost cost in skeleton.Costs)
        {
            ResourceLabel label = Instantiate(ResourceCostPrefab, ResourceCostParent);
            label.SetResourceCost(cost);
        }
    }

    public void OnClick()
    {
        PlaceStructureEvent.Raise(Skeleton);
    }
}
