using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringResourceToPlanTaskToDo : LocationBasedTaskToDo
{
    public Structure StructurePlan { get; private set; }
    public Unit PerformingObjectAsUnit { get; private set; }

    public BringResourceToPlanTaskToDo(Unit performingObject, Structure structurePlan) : base(performingObject, structurePlan.transform.position, $"Providing {performingObject.HeldResources.Cost} {ConfigurationManagement.GatherableSkeletons[performingObject.HeldResources.Resource].FriendlyName} to {structurePlan.StructureSkeletonData.FriendlyName}")
    {
        this.StructurePlan = structurePlan;
        this.PerformingObjectAsUnit = performingObject;
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        StructurePlan.ContributeResource(PerformingObjectAsUnit.HeldResources.Resource, PerformingObjectAsUnit.HeldResources.Cost);
        PerformingObjectAsUnit.ClearHeldResources();
    }
}
