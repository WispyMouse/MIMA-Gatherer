using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameworldObject : MonoBehaviour
{
    [SerializeReference]
    private Unit GameworldUnitPF;

    [SerializeReference]
    private GameworldObjectEvent GameworldObjectSelectedEvent;

    [SerializeField]
    private ConfiguredStatistic<float> DistanceToSpawnUnit { get; set; } = new ConfiguredStatistic<float>(1f, $"{nameof(Structure)}.{nameof(DistanceToSpawnUnit)}");

    protected Stack<TaskToDo> TaskStack { get; set; } = new Stack<TaskToDo>();

    protected bool IsRunningTasks { get; private set; } = false;

    public virtual string FriendlyName => this.name;
    public virtual string DisplayName => this.name;

    public TaskToDo CurrentTask
    {
        get
        {
            if (TaskStack.Count > 0)
            {
                return TaskStack.Peek();
            }

            return null;
        }
    }

    public virtual void Interact()
    {
        GameworldObjectSelectedEvent.Raise(this);
    }

    public virtual void SpawnUnit(UnitSkeleton toSpawn)
    {
        Unit newUnit = Instantiate(GameworldUnitPF);
        newUnit.AssignUnitSkeleton(toSpawn);

        Vector2 randomOffset = Random.insideUnitCircle.normalized * DistanceToSpawnUnit.Value;
        newUnit.transform.position = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

        newUnit.StartOperations();
    }

    public virtual void AssignTaskToDo(TaskToDo task)
    {
        if (CurrentTask != null)
        {
            CurrentTask.InterruptAndStop();
        }

        TaskStack.Clear();

        TaskStack.Push(task);
    }

    public void AddTaskToDo(TaskToDo task)
    {
        if (CurrentTask != null)
        {
            CurrentTask.InterruptForNewItemOnStack();
        }

        TaskStack.Push(task);
    }

    private void FixedUpdate()
    {
        ProcessTaskStack();
    }

    protected virtual void ProcessTaskStack()
    {
        if (CurrentTask == null)
        {
            if (IsRunningTasks)
            {
                ClearedTaskStack();
                IsRunningTasks = false;
            }
            return;
        }

        IsRunningTasks = true;

        if (CurrentTask.Rejected)
        {
            TaskStack.Pop();
            ProcessTaskStack();
            return;
        }

        if (CurrentTask.Complete)
        {
            TaskStack.Pop();
            ProcessTaskStack();
            return;
        }

        if (!CurrentTask.Processed)
        {
            bool didPush = CurrentTask.ConsiderAndPushPrerequisiteTasks();

            if (didPush)
            {
                ProcessTaskStack();
                return;
            }

            CurrentTask.ProcessAndCalculate();
            ProcessTaskStack();
            return;
        }

        CurrentTask.Tick(Time.deltaTime);
    }

    protected virtual void ClearedTaskStack()
    {

    }

    public virtual string GetCurrentTask()
    {
        if (CurrentTask != null)
        {
            return CurrentTask.OperationDescription;
        }

        return "Nothing!";
    }
}
