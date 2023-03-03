using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestTaskToDo : TaskToDo
{
    public Unit PerformingObjectAsUnit { get; private set; }
    public Crystals CrystalsTarget { get; private set; }

    public HarvestTaskToDo(Unit performingUnit, Crystals crystalsTarget) : base(performingUnit, $"Gathering {crystalsTarget.ResourceName}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.CrystalsTarget = crystalsTarget;
    }

    public override void ProcessAndCalculate()
    {
        base.ProcessAndCalculate();

        this.SetEstimationAndSpawnTimer(CrystalsTarget.TimeToGatherResource.Value);
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        PerformingObjectAsUnit.StartHoldingResources(CrystalsTarget.ResourceName, CrystalsTarget.AmountOfResourcesGathered.Value);
    }
}
