using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskToDo
{
    public GameworldObject PerformingObject { get; private set;}
    public string OperationDescription { get; private set; }

    public bool Processed { get; private set; } = false;
    public bool Rejected { get; private set; } = false;
    public bool Complete { get; private set; } = false;

    public float TimeSpentOnTask { get; private set; } = 0;
    public float EstimatedTime { get; private set; } = 0;

    public Timer RepresentingTimer { get; private set; }

    public ConfiguredStatistic<bool> ShowTimer { get; private set; }
    public ConfiguredStatistic<float> DistanceAboveTaskPerformer { get; private set; }
    public ConfiguredStatistic<float> Scale { get; private set; }
    public ConfiguredStatistic<string> TimerFrontColor { get; private set; }

    public TaskToDo(GameworldObject performingObject, string operationDescription)
    {
        this.PerformingObject = performingObject;
        this.OperationDescription = operationDescription;
        Processed = false;
        Rejected = false;
        Complete = false;

        ShowTimer = new ConfiguredStatistic<bool>(false, $"{GetType().Name}.{nameof(ShowTimer)}");
        DistanceAboveTaskPerformer = new ConfiguredStatistic<float>(5f, $"{GetType().Name}.{nameof(DistanceAboveTaskPerformer)}");
        Scale = new ConfiguredStatistic<float>(1f, $"{GetType().Name}.{nameof(Scale)}");
        TimerFrontColor = new ConfiguredStatistic<string>("#FFFFFF", $"{GetType().Name}.{nameof(TimerFrontColor)}");
    }

    protected virtual Timer SetEstimationAndSpawnTimer(float estimatedTime)
    {
        this.EstimatedTime = estimatedTime;

        if (ShowTimer.Value)
        {
            RepresentingTimer = TimerManagement.GetTimer(estimatedTime, PerformingObject.transform.position + Vector3.up * DistanceAboveTaskPerformer.Value, TimeSpentOnTask);
            RepresentingTimer.Scale = Scale.Value;
            RepresentingTimer.TimerFrontColor = TimerFrontColor.Value;
            return RepresentingTimer;
        }

        return null;
    }

    public void InterruptAndStop()
    {
        if (this.RepresentingTimer != null)
        {
            TimerManagement.ReturnTimerToPool(this.RepresentingTimer);
        }
    }

    public void InterruptForNewItemOnStack()
    {
        if (this.RepresentingTimer != null)
        {
            TimerManagement.ReturnTimerToPool(this.RepresentingTimer);
        }
    }

    public virtual void ProcessAndCalculate()
    {
        Processed = true;

        this.RepresentingTimer?.Dismiss();
    }

    public void RejectAsNotPossible()
    {
        Rejected = true;

        this.RepresentingTimer?.Dismiss();
    }

    public virtual void Tick(float deltaTime)
    {
        TimeSpentOnTask += deltaTime;

        if (this.RepresentingTimer != null)
        {
            this.RepresentingTimer.CurrentValue = TimeSpentOnTask;
        }

        if (this.EstimatedTime > 0 && this.TimeSpentOnTask >= this.EstimatedTime)
        {
            TaskCompleted();
        }
    }

    public virtual void TaskCompleted()
    {
        Complete = true;

        this.RepresentingTimer?.Dismiss();
    }

    public virtual string TaskDetails()
    {
        return string.Empty;
    }
}
