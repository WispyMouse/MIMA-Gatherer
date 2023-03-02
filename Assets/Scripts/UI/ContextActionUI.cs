using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextActionUI : MonoBehaviour
{
    Structure SelectedStructure { get; set; }
    [SerializeReference]
    BuildThingButton BuildThingButtonPF;

    [SerializeReference]
    Transform BuildThingHolder;
    private List<BuildThingButton> ActiveButtons { get; set; } = new List<BuildThingButton>();
    public void OnStructureSelected(Structure selected)
    {
        ClearButtons();
        SelectedStructure = selected;

        foreach (UnitSkeleton curSkeleton in ConfigurationManagement.UnitSkeletons.Values)
        {
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
}
