using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : GameworldObject
{
    enum UnitCommandState
    {
        GatherClosestResource,
        ReturnResourceToCenter,
        FollowPath,
        None,
        GatheringNow,
        ReturningNow
    }

    UnitCommandState curState { get; set; } = UnitCommandState.GatherClosestResource;

    public const float CloseEnoughToCorner = .01f;
    public const float CloseEnoughToResource = .15f;

    public int CrystalUnitCost = 50;
    public float ProductionTimeSeconds = 1f;
    public float MovementPerSecond = 8f;

    public float TimeToReturnResource = 1f;

    public Vector3 DestinationPoint { get; set; }

    [SerializeReference]
    private NavMeshAgent Agent;

    [SerializeReference]
    private Rigidbody AttachedRigidbody;

    [SerializeReference]
    private UnitEvent DestinationReachedEvent;

    NavMeshPath CurrentPath { get; set; }
    int CurrentPathCornerIndex { get; set; }
    Crystals CurrentTargetCrystals { get; set; }

    bool IsHoldingResource { get; set; } = false;
    string HeldResource { get; set; }
    int HeldResourceCount { get; set; }

    private void Awake()
    {
        CurrentTargetCrystals = GoToNeareset<Crystals>();
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

        CurrentPathCornerIndex = 0;
        CurrentPath = pathToNearestThing;
        return nearestThing;
    }

    private void FixedUpdate()
    {
        if (CurrentPath == null)
        {
            return;
        }

        float movementAllowed = Time.deltaTime * MovementPerSecond;
        while (movementAllowed > 0)
        {
            if (CurrentPath == null)
            {
                break;
            }

            movementAllowed = FollowPathToNextCorner(movementAllowed);
        }
    }

    /// <summary>
    /// Instructs the Unit to move down the path it's on.
    /// </summary>
    /// <param name="maxDistance">The maximum amount of distance that can be covered. If the distance to the next corner is less than this, will only go to the next available corner.</param>
    /// <returns>The amount of space left after reaching the farthest point that could be reached, or after reaching the next corner.</returns>
    float FollowPathToNextCorner(float maxDistance)
    {
        if (CurrentPath == null)
        {
            return -1f;
        }

        if (CurrentPathCornerIndex < 0)
        {
            return -1f;
        }

        if (CurrentPathCornerIndex >= CurrentPath.corners.Length)
        {
            return -1f;
        }

        Vector3 targetPosition = CurrentPath.corners[CurrentPathCornerIndex];
        Vector3 calculatedNewPosition = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);

        float distanceToCorner = Vector3.Distance(transform.position, targetPosition);

        AttachedRigidbody.MovePosition(calculatedNewPosition);

        if (Vector3.Distance(transform.position, targetPosition) < CloseEnoughToCorner)
        {
            CurrentPathCornerIndex++;
            if (CurrentPath.corners.Length <= CurrentPathCornerIndex)
            {
                CurrentPath = null;
                DestinationReachedEvent.Raise(this);

            }
        }

        return Mathf.Max(0, maxDistance - distanceToCorner);
    }

    public void OnPathingDestinationReached(Unit unitMessaged)
    {
        if (this != unitMessaged)
        {
            return;
        }

        if (curState == UnitCommandState.FollowPath)
        {
            if (IsHoldingResource)
            {
                curState = UnitCommandState.ReturnResourceToCenter;
                GoToNeareset<Structure>();
            }
            else
            {
                curState = UnitCommandState.GatherClosestResource;
                CurrentTargetCrystals = GoToNeareset<Crystals>();
            }
            
            return;
        }

        if (curState == UnitCommandState.ReturnResourceToCenter)
        {
            // Need to find a next crystal
            curState = UnitCommandState.ReturningNow;
            StartCoroutine(StartSeekingNextResourceAfterReturningDelay(TimeToReturnResource));
            return;
        }

        if (curState == UnitCommandState.GatherClosestResource &&
            CurrentTargetCrystals != null && Vector3.Distance(transform.position, CurrentTargetCrystals.transform.position) < CloseEnoughToResource)
        {
            StartCoroutine(StartReturningToBaseAfterDelay(CurrentTargetCrystals.TimeToGatherResource));
            return;
        }

        Debug.Log("Reached destination. Now what?");
    }

    public void SendTowardsPosition(Vector3 position)
    {
        NavMeshPath pathToPoint = new NavMeshPath();
        if (!Agent.CalculatePath(position, pathToPoint))
        {
            return;
        }

        CurrentPathCornerIndex = 0;
        CurrentPath = pathToPoint;
        curState = UnitCommandState.FollowPath;
    }

    public string GetCurrentTask()
    {
        switch (curState)
        {
            case UnitCommandState.None:
                return "No job";
            case UnitCommandState.FollowPath:
                return "Follow Path";
            case UnitCommandState.ReturnResourceToCenter:
                return "Returning Resource";
            case UnitCommandState.GatherClosestResource:
                return "Gather Closest Resource";
            case UnitCommandState.ReturningNow:
                return "Returning Now";
            case UnitCommandState.GatheringNow:
                return "Gathering Now";
            default:
                return "?";
        }
    }

    IEnumerator StartReturningToBaseAfterDelay(float delay)
    {
        curState = UnitCommandState.GatheringNow;

        yield return new WaitForSeconds(delay);

        IsHoldingResource = true;
        HeldResource = CurrentTargetCrystals.ResourceName;
        HeldResourceCount = CurrentTargetCrystals.AmountOfResourcesGathered;

        CurrentTargetCrystals = null;
        curState = UnitCommandState.ReturnResourceToCenter;
        GoToNeareset<Structure>();
    }

    IEnumerator StartSeekingNextResourceAfterReturningDelay(float delay)
    {
        curState = UnitCommandState.ReturningNow;

        yield return new WaitForSeconds(delay);

        InventoryManagement.ChangeResource(HeldResource, HeldResourceCount);

        IsHoldingResource = false;
        HeldResource = string.Empty;
        HeldResourceCount = 0;

        curState = UnitCommandState.GatherClosestResource;
        GoToNeareset<Crystals>();
    }
}
