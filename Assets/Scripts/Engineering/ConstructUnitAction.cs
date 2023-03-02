using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructUnitAction
{
    public UnitSkeleton UnitToConstruct { get; set; }
    public GameworldObject ObjectThatConstructs { get; set; }

    public ConstructUnitAction(UnitSkeleton unitToConstruct, GameworldObject objectThatConstructs)
    {
        this.UnitToConstruct = unitToConstruct;
        this.ObjectThatConstructs = objectThatConstructs;
    }

    public void ExecutePlanOnce()
    {
        if (InventoryManagement.CanAfford(UnitToConstruct.Costs))
        {
            InventoryManagement.ChangeResource(UnitToConstruct.Costs);
            ObjectThatConstructs.StartCoroutine(ObjectThatConstructs.SpawnUnitAfterWaiting(UnitToConstruct));
        }
        else
        {
            Debug.Log("Could not afford all of the costs");
        }
    }
}
