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

    public override void ProcessAndCalculate()
    {
        base.ProcessAndCalculate();

        if (!ConstructUnit.PayCosts())
        {
            this.RejectAsNotPossible();
            return;
        }

        this.SetEstimationAndSpawnTimer(ConstructUnit.UnitToConstruct.ProductionTimeSeconds);
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        ConstructUnit.ExecutePlanOnce();
    }
}
