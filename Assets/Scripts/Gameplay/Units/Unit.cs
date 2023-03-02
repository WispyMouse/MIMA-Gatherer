using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : GameworldObject
{
    public const float CloseEnoughToResource = .15f;

    public UnitSkeleton UnitSkeletonData { get; private set; }

    [SerializeReference]
    private NavMeshAgent _Agent;

    public NavMeshAgent Agent { get { return _Agent; } }

    public Rigidbody AttachedRigidbody;

    public ResourceCost HeldResources { get; private set; } = null;

    public void AssignUnitSkeleton(UnitSkeleton data)
    {
        this.UnitSkeletonData = data;
    }

    public void StartOperations()
    {
        ActivateBrainCell();
    }

    T GetNearestObject<T>(out NavMeshPath pathToResource) where T : GameworldObject
    {
        pathToResource = null;
        T[] allTargets = GameObject.FindObjectsOfType<T>();
        if (allTargets.Length == 0)
        {
            return null;
        }

        float shortestDistance = float.MaxValue;
        NavMeshPath pathUsed = null;
        T closestTarget = null;
        foreach (T curTarget in allTargets)
        {
            NavMeshPath thisPath = new NavMeshPath();
            if (!Agent.CalculatePath(curTarget.transform.position, thisPath))
            {
                // Couldn't reach
                continue;
            }

            float totalDistance = 0;
            Vector3 lastCorner = transform.position;
            foreach (Vector3 corner in thisPath.corners)
            {
                totalDistance += Vector3.Distance(lastCorner, corner);
                lastCorner = corner;
            }

            if (totalDistance < shortestDistance)
            {
                shortestDistance = totalDistance;
                pathUsed = thisPath;
                closestTarget = curTarget;
            }
        }

        if (closestTarget == null)
        {
            return null;
        }

        pathToResource = pathUsed;
        return closestTarget;
    }

    T GoToNeareset<T>() where T : GameworldObject
    {
        NavMeshPath pathToNearestThing;
        T nearestThing = GetNearestObject<T>(out pathToNearestThing);

        if (nearestThing == null)
        {
            Debug.Log("Can't find it, boss!");
            return null;
        }

        AddTaskToDo(new MovementTaskToDo(this, nearestThing.transform.position));
        return nearestThing;
    }

    public string GetCurrentTask()
    {
        if (CurrentTask != null)
        {
            return CurrentTask.OperationDescription;
        }

        return "Nothing!";
    }

    public void ActivateBrainCell()
    {
        if (CurrentTask != null)
        {
            return;
        }

        if (HeldResources != null)
        {
            NavMeshPath foundPathToStructure = null;
            Structure nearestStructure = GetNearestObject<Structure>(out foundPathToStructure);
            if (nearestStructure == null)
            {
                Debug.Log("There are no structures, so where should I return this?");
                return;
            }
            AddTaskToDo(new ReturnResourceTaskToDo(this, nearestStructure));
            AddTaskToDo(new MovementTaskToDo(this, nearestStructure.transform.position, foundPathToStructure));
        }

        NavMeshPath foundPathToCrystals = null;
        Crystals nearestCrystals = GetNearestObject<Crystals>(out foundPathToCrystals);
        if (nearestCrystals == null)
        {
            Debug.Log("There are no resources, so what should I be doing?");
            return;
        }
        AddTaskToDo(new HarvestTaskToDo(this, nearestCrystals));
        AddTaskToDo(new MovementTaskToDo(this, nearestCrystals.transform.position, foundPathToCrystals));
    }

    public void StartHoldingResources(string resourceName, int amount)
    {
        this.HeldResources = new ResourceCost(resourceName, amount);
    }

    public void ClearHeldResources()
    {
        this.HeldResources = null;
    }

    protected override void ClearedTaskStack()
    {
        ActivateBrainCell();
    }
}
