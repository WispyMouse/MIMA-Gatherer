using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnResourceTaskToDo : LocationBasedTaskToDo
{
    public Unit PerformingObjectAsUnit { get; private set; }
    public Structure ReturnTarget { get; private set; }

    public ReturnResourceTaskToDo(Unit performingUnit) : base(performingUnit)
    {
        Vector3 target = Vector3.zero;
        this.PerformingObjectAsUnit = performingUnit;
    }

    public ReturnResourceTaskToDo(Unit performingUnit, Structure returnTarget) : base(performingUnit, returnTarget.transform.position, $"Returning {performingUnit.HeldResources?.Cost} {performingUnit.HeldResources?.Resource} to {returnTarget.name}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.ReturnTarget = returnTarget;
    }

    public ReturnResourceTaskToDo(Unit performingUnit, Structure returnTarget, NavMeshPath alreadyFoundPath) : base(performingUnit, returnTarget.transform.position, alreadyFoundPath, $"Returning {performingUnit.HeldResources?.Cost} {performingUnit.HeldResources?.Resource} to {returnTarget.name}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.ReturnTarget = returnTarget;
    }

    public override void ProcessAndCalculate()
    {
        base.ProcessAndCalculate();

        if (this.ReturnTarget == null)
        {
            NavMeshPath foundPath;
            Structure nearestStructure = PerformingObjectAsUnit.GetNearestStructureThatAcceptsHeldResource(out foundPath);
            if (nearestStructure == null)
            {
                Debug.Log($"Can't find any structures to return this resource to: {PerformingObjectAsUnit.HeldResources.Resource}");
                return;
            }
            this.ReturnTarget = nearestStructure;
            this.TargetPosition = nearestStructure.transform.position;
            AlreadyCalculatedPath = foundPath;
        }

        this.SetEstimationAndSpawnTimer(PerformingObjectAsUnit.UnitSkeletonData.TimeToReturnResource);
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        InventoryManagement.ChangeResource(PerformingObjectAsUnit.HeldResources.Resource, PerformingObjectAsUnit.HeldResources.Cost);
        PerformingObjectAsUnit.ClearHeldResources();
    }
}
