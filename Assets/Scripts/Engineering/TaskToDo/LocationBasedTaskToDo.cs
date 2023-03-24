using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class LocationBasedTaskToDo : TaskToDo
{
    public Vector3 TargetPosition { get; protected set; }
    protected NavMeshPath AlreadyCalculatedPath { get; set; }

    public ConfiguredStatistic<float> AcceptableDistanceToTask { get; private set; }

    protected LocationBasedTaskToDo(GameworldObject performingObject) : base(performingObject, "Doing something!")
    {
        AcceptableDistanceToTask = new ConfiguredStatistic<float>(1f, $"{GetType().Name}.{nameof(AcceptableDistanceToTask)}");
    }

    protected LocationBasedTaskToDo(GameworldObject performingObject, Vector3 destination, string operationDescription) : base(performingObject, operationDescription)
    {
        AcceptableDistanceToTask = new ConfiguredStatistic<float>(1f, $"{GetType().Name}.{nameof(AcceptableDistanceToTask)}");
        TargetPosition = destination;
    }

    protected LocationBasedTaskToDo(GameworldObject performingObject, Vector3 destination, NavMeshPath alreadycalculatedPath, string operationDescription) : base(performingObject, operationDescription)
    {
        AcceptableDistanceToTask = new ConfiguredStatistic<float>(1f, $"{GetType().Name}.{nameof(AcceptableDistanceToTask)}");
        TargetPosition = destination;
        AlreadyCalculatedPath = alreadycalculatedPath;
    }

    public override bool ConsiderAndPushPrerequisiteTasks()
    {
        base.ConsiderAndPushPrerequisiteTasks();

        float distanceToTarget = Vector3.Distance(PerformingObject.transform.position, TargetPosition);
        if (distanceToTarget > AcceptableDistanceToTask.Value)
        {
            PerformingObject.AddTaskToDo(new MovementTaskToDo(PerformingObject as Unit, TargetPosition, AlreadyCalculatedPath, AcceptableDistanceToTask.Value));
            return true;
        }

        return false;
    }
}
