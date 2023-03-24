using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ContributeToPlanTaskToDo : TaskToDo
{
    public Unit PerformingObjectAsUnit { get; private set; }
    public Structure PlanTarget { get; private set; }

    public ConfiguredStatistic<float> AcceptableDistanceToTask { get; private set; }

    public ContributeToPlanTaskToDo(Unit performingUnit, Structure planTarget) : base(performingUnit, $"Helping Build {planTarget.FriendlyName}")
    {
        this.PerformingObjectAsUnit = performingUnit;
        this.PlanTarget = planTarget;
        AcceptableDistanceToTask = new ConfiguredStatistic<float>(1f, $"{GetType().Name}.{nameof(AcceptableDistanceToTask)}");
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        PlanTarget.PutTimeIntoPlan();

        if (!PlanTarget.IsPlan)
        {
            TaskCompleted();
        }
    }

    public override bool ConsiderAndPushPrerequisiteTasks()
    {
        // Does the target plan need the resources you're currently holding? If yes, turn it in. If no, return them to a nearby structure
        if (PerformingObjectAsUnit.HeldResources != null)
        {
            if (!PlanTarget.IsResourceNeeded(PerformingObjectAsUnit.HeldResources.Resource, out _))
            {
                PerformingObject.AddTaskToDo(new ReturnResourceTaskToDo(PerformingObjectAsUnit));
                return true;
            }

            PerformingObject.AddTaskToDo(new BringResourceToPlanTaskToDo(PerformingObjectAsUnit, PlanTarget));
            return true;
        }

        // Do we still need any resources? If yes, then go gather what is needed
        foreach (ResourceCost need in PlanTarget.StructureSkeletonData.Costs)
        {
            int amountNeeded;
            if (PlanTarget.IsResourceNeeded(need.Resource, out amountNeeded))
            {
                PerformingObject.AddTaskToDo(new GetResourcesFromStructureTaskToDo(PerformingObjectAsUnit, need.Resource, Mathf.Min(amountNeeded, ConfigurationManagement.GatherableSkeletons[need.Resource].GatherForBuildingAmount)));
                return true;
            }
        }

        // Must be ready to finalize, right? Let's work on it by having this job tick
        // Let's make sure we're close enough to start doing things
        if (Vector3.Distance(PerformingObject.transform.position, PlanTarget.transform.position) > AcceptableDistanceToTask.Value)
        {
            PerformingObject.AddTaskToDo(new MovementTaskToDo(PerformingObjectAsUnit, PlanTarget.transform.position));
            return true;
        }
        return false; 
    }
}
