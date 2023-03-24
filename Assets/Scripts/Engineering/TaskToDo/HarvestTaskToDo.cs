using System.Collections;
using System.Collections.Generic;


public class HarvestTaskToDo : LocationBasedTaskToDo
{
    public Unit PerformingObjectAsUnit { get; private set; }
    public Gatherable GatherablesTarget { get; private set; }

    public ConfiguredStatistic<float> AcceptableDistance { get; private set; }

    public HarvestTaskToDo(Unit performingUnit, Gatherable gatherablesTarget) : base(performingUnit, gatherablesTarget.transform.position, $"Gathering {gatherablesTarget.GatherableSkeletonData.ResourceName}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.GatherablesTarget = gatherablesTarget;
    }

    public HarvestTaskToDo(Unit performingUnit, Gatherable gatherablesTarget, UnityEngine.AI.NavMeshPath alreadyDeterminedPath) : base(performingUnit, gatherablesTarget.transform.position, alreadyDeterminedPath, $"Gathering {gatherablesTarget.GatherableSkeletonData.ResourceName}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.GatherablesTarget = gatherablesTarget;
    }

    public override void ProcessAndCalculate()
    {
        base.ProcessAndCalculate();
        this.SetEstimationAndSpawnTimer(GatherablesTarget.GatherableSkeletonData.TimeToGatherResource);
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        PerformingObjectAsUnit.StartHoldingResources(GatherablesTarget.GatherableSkeletonData.FriendlyName, GatherablesTarget.GatherableSkeletonData.AmountOfResourcesGathered);
    }
}
