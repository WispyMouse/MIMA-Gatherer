using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextActionUI : MonoBehaviour
{
    Structure SelectedStructure { get; set; }
    [SerializeReference]
    BuildThingButton BuildThingButtonPF;
    [SerializeReference]
    PlaceBuildingPlanButton PlaceBuildPlanButtonPF;

    [SerializeReference]
    Transform BuildThingHolder;
    private List<UIBehaviour> ActiveButtons { get; set; } = new List<UIBehaviour>();
    public void OnStructureSelected(Structure selected)
    {
        ClearButtons();
        SelectedStructure = selected;

        foreach (UnitSkeleton curSkeleton in ConfigurationManagement.UnitSkeletons.Values)
        {
            if (selected.StructureSkeletonData.ProducedUnits.Contains(curSkeleton.FriendlyName))
            {
                // Can't make this unit!
                continue;
            }

            BuildThingButton newButton = Instantiate(BuildThingButtonPF, BuildThingHolder);
            newButton.SetBuildTarget(new ConstructUnitAction(curSkeleton, selected));
            ActiveButtons.Add(newButton);
        }
    }

    public void OnDismiss()
    {
        ClearButtons();
        SelectedStructure = null;
    }

    void ClearButtons()
    {
        for (int ii = 0; ii < ActiveButtons.Count; ii++)
        {
            Destroy(ActiveButtons[ii].gameObject);
        }
        ActiveButtons.Clear();
    }

    public void OnStructureBuildPressed()
    {
        ClearButtons();

        foreach (StructureSkeleton curSkeleton in ConfigurationManagement.StructureSkeletons.Values)
        {
            PlaceBuildingPlanButton newButton = Instantiate(PlaceBuildPlanButtonPF, BuildThingHolder);
            newButton.SetBuildTarget(curSkeleton);
            ActiveButtons.Add(newButton);
        }
    }
}
