using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildThingButton : MonoBehaviour
{
    [SerializeReference]
    private TMP_Text Text;

    [SerializeReference]
    private ResourceLabel ResourceCostPrefab;

    [SerializeReference]
    private Transform ResourceCostParent;

    private ConstructUnitAction Construction { get; set; }

    public void SetBuildTarget(ConstructUnitAction constructUnitAction)
    {
        Construction = constructUnitAction;
        Text.text = constructUnitAction.UnitToConstruct.UnitName;

        foreach (ResourceCost cost in constructUnitAction.UnitToConstruct.Costs)
        {
            ResourceLabel label = Instantiate(ResourceCostPrefab, ResourceCostParent);
            label.SetResourceCost(cost);
        }
    }

    public void OnClick()
    {
        SpawnUnitTaskToDo newTask = new SpawnUnitTaskToDo(Construction.ObjectThatConstructs, Construction);
        Construction.ObjectThatConstructs.AddTaskToDo(newTask);
    }
}
