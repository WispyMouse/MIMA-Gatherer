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

    public bool PayCosts()
    {
        if (InventoryManagement.CanAfford(UnitToConstruct.Costs))
        {
            InventoryManagement.ChangeResource(UnitToConstruct.Costs);
            return true;
        }

        return false;
    }

    public void ExecutePlanOnce()
    {
        ObjectThatConstructs.SpawnUnit(UnitToConstruct);
    }
}
