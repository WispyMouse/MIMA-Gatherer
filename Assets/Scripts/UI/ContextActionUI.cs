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
    [SerializeField]
    Button GenericButtonPF;

    [SerializeReference]
    Transform BuildThingHolder;
    private List<UIBehaviour> ActiveButtons { get; set; } = new List<UIBehaviour>();
    public void OnStructureSelected(Structure selected)
    {
        ClearButtons();
        SelectedStructure = selected;

        if (SelectedStructure.IsPlan)
        {
            ShowPlanUI(SelectedStructure);
            return;
        }

        foreach (UnitSkeleton curSkeleton in ConfigurationManagement.UnitSkeletons.Values)
        {
            if (!selected.StructureSkeletonData.ProducedUnits.Contains(curSkeleton.FriendlyName))
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

    void ShowPlanUI(Structure structure)
    {
        ClearButtons();

        Button addButton = Instantiate(GenericButtonPF, BuildThingHolder);
        addButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Add Worker";
        addButton.onClick.AddListener(() => { structure.WorkersDesired++; });
        ActiveButtons.Add(addButton);

        Button removeButton = Instantiate(GenericButtonPF, BuildThingHolder);
        removeButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Remove Worker";
        removeButton.onClick.AddListener(() => { structure.WorkersDesired = Mathf.Max(0, structure.WorkersDesired); });
        ActiveButtons.Add(removeButton);
    }

    public void OnStructurePlanBuildFinished(Structure finished)
    {
        if (SelectedStructure == finished)
        {
            OnStructureSelected(finished);
        }
    }
}
