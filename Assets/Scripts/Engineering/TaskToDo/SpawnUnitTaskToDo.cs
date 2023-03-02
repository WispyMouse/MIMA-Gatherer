using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitTaskToDo : TaskToDo
{
    public ConstructUnitAction ConstructUnit { get; private set; }

    public SpawnUnitTaskToDo(GameworldObject structureCreatingUnit, ConstructUnitAction constructUnit) : base(structureCreatingUnit, $"Creating {constructUnit.UnitToConstruct.FriendlyName}")
    {
        this.ConstructUnit = constructUnit;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (this.TimeSpentOnTask > ConstructUnit.UnitToConstruct.ProductionTimeSeconds)
        {
            TaskCompleted();
        }
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        ConstructUnit.ExecutePlanOnce();
    }
}
