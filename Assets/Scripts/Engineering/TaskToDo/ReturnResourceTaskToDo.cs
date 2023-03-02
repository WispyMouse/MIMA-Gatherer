using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnResourceTaskToDo : TaskToDo
{
    public Unit PerformingObjectAsUnit { get; private set; }
    public Structure ReturnTarget { get; private set; }

    public ReturnResourceTaskToDo(Unit performingUnit, Structure returnTarget) : base(performingUnit, $"Returning {performingUnit.HeldResources?.Cost} {performingUnit.HeldResources?.Resource} to {returnTarget.name}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.ReturnTarget = returnTarget;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (this.TimeSpentOnTask >= PerformingObjectAsUnit.UnitSkeletonData.TimeToReturnResource)
        {
            TaskCompleted();
        }
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        InventoryManagement.ChangeResource(PerformingObjectAsUnit.HeldResources.Resource, PerformingObjectAsUnit.HeldResources.Cost);
        PerformingObjectAsUnit.ClearHeldResources();
    }
}
