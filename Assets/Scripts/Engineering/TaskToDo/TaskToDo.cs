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

    public TaskToDo(GameworldObject performingObject, string operationDescription)
    {
        this.PerformingObject = performingObject;
        this.OperationDescription = operationDescription;
        Processed = false;
        Rejected = false;
        Complete = false;
    }

    public void InterruptAndStop()
    {

    }

    public void InterruptForNewItemOnStack()
    {

    }

    public virtual void ProcessAndCalculate()
    {
        Processed = true;
    }

    public void RejectAsNotPossible()
    {
        Rejected = true;
    }

    public virtual void Tick(float deltaTime)
    {
        TimeSpentOnTask += deltaTime;
    }

    public virtual void TaskCompleted()
    {
        Complete = true;
    }
}
