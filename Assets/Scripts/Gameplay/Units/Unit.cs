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

    [SerializeReference]
    private MeshRenderer ColorableModel;

    [SerializeReference]
    private Transform ScalableModel;

    public override string FriendlyName => UnitSkeletonData.FriendlyName;
    public override string DisplayName => UnitSkeletonData.UnitName;

    public void AssignUnitSkeleton(UnitSkeleton data)
    {
        this.UnitSkeletonData = data;

        this.ScalableModel.localScale = Vector3.one * data.ModelScale;

        if (ColorUtility.TryParseHtmlString(data.ModelColor, out Color parsedColor))
        {
            this.ColorableModel.material.color = parsedColor;
        }
    }

    public void StartOperations()
    {
        ActivateBrainCell();
    }

    T GetNearestObject<T>(out NavMeshPath pathToResource) where T : GameworldObject
    {
        T[] allTargets = GameObject.FindObjectsOfType<T>();
        return GetNearestObject<T>(out pathToResource, new List<T>(allTargets));
    }

    T GetNearestObject<T>(out NavMeshPath pathToResource, List<T> consideredResources) where T : GameworldObject
    {
        pathToResource = null;
        if (consideredResources.Count == 0)
        {
            return null;
        }

        float shortestDistance = float.MaxValue;
        NavMeshPath pathUsed = null;
        T closestTarget = null;
        foreach (T curTarget in consideredResources)
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

    Structure GetNearestStructureThatAcceptsHeldResource(out NavMeshPath pathToResource)
    {
        Structure[] allTargets = GameObject.FindObjectsOfType<Structure>();
        List<Structure> consideredStructures = new List<Structure>();

        foreach (Structure target in allTargets)
        {
            if (target.StructureSkeletonData.AcceptedResources == null)
            {
                continue;
            }

            if (target.StructureSkeletonData.AcceptedResources.Contains(HeldResources.Resource))
            {
                consideredStructures.Add(target);
            }
        }

        return GetNearestObject<Structure>(out pathToResource, consideredStructures);
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

            return;
        }

        NavMeshPath foundPathToGatherable = null;
        Gatherable nearestGatherable = GetNearestObject<Gatherable>(out foundPathToGatherable);
        if (nearestGatherable == null)
        {
            Debug.Log("There are no resources, so what should I be doing?");
            return;
        }

        AddTaskToDo(new HarvestTaskToDo(this, nearestGatherable));
        AddTaskToDo(new MovementTaskToDo(this, nearestGatherable.transform.position, foundPathToGatherable));
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
